using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackLibrary
{
    public static class TournamentLogic
    {
        // Wire up Matchups
        // Order list of teams randomly
        // Check if big enough if not add in byes
        // Create the first round 
        // Create all other rounds
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamsOrder(model.EntredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);
            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);
        }

        public static void UpdateTournamentsResults(TournamentModel model)
        {

            int roundBeforeScoring = model.CheckCurrentRound();
            List<MatchupModel> toScore = new List<MatchupModel>();
            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel rm in round)
                {
                    if (rm.Winner == null && (rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnersInMatchups(toScore);
            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
            // equivalent to 
            // foreach(MatchupModel x in toScore)
            // {
            //  GlobalConfig.Connection.UpdateMachup(m);
            // }

            int endigingRound = model.CheckCurrentRound();

            if (roundBeforeScoring < endigingRound)
            {
                //we moved to the next round
                //EmailLogic.SendEmail();
                model.AlertUsersToNewRound();

            }

        }

        private static void AlertPersonToNewRound(PersonModel person, string personTeamName, MatchupEntryModel competitorTeam)
        {
            if (person.Email.Length == 0)
            {
                return;
            }

            string to = "";
            string subject = "";
            StringBuilder body = new StringBuilder();



            if (competitorTeam != null)
            {
                subject = $"You have a new matchup with {competitorTeam.TeamCompeting.TeamName}";


                body.AppendLine("<h1> You have a new matchup</h1>");
                body.Append("<strong>Competitor:</strong>");
                body.AppendLine(competitorTeam.TeamCompeting.TeamName);
                body.AppendLine();
                body.AppendLine();
                body.AppendLine("Good luck!");
                body.AppendLine("~Tournament Tracker");


            }
            else
            {
                subject = $"You have a bye week this round";
                body.AppendLine("Enjoy your round off! ");
                body.AppendLine("~Tournament Tracker");

            }

            to = person.Email;
            EmailLogic.SendEmail(to, subject, body.ToString());
        }

        public static void AlertUsersToNewRound(this TournamentModel model)
        {
            int currentRoundNumber = CheckCurrentRound(model);
            List<MatchupModel> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNumber).First();
            foreach(MatchupModel matchupModel in currentRound)
            {
                foreach(MatchupEntryModel me in matchupModel.Entries)
                {
                    foreach(PersonModel person in me.TeamCompeting.TeamMembers)
                    {
                        AlertPersonToNewRound(person, me.TeamCompeting.TeamName, matchupModel.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
                    }
                }
            }
        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;
            foreach(List<MatchupModel> round in model.Rounds)
            {
                //verify if every  matchup winner is not null / the x represent the matchup in this case.
                if (round.All(x => x.Winner != null))
                {
                    output += 1;
                }
                else
                {
                    return output;
                }
            }

            // the tournament is complete ! 
            CompleteTournamentLogic(model);
            return output - 1;
        }

        private static decimal CalculatePrizePayOut(this PrizeModel prize, decimal TotalIncome)
        {
            decimal output = 0;
            if (prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else
            {
                output = Decimal.Multiply(TotalIncome , Convert.ToDecimal(prize.PrizePercentage / 100));
            }
            return output;
        }

        private static void CompleteTournamentLogic(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);
            // find the last round (a list of matchups), then inside that list pick the first matchup model (which is the only one left cuz is the last round), and then find out the winner (a team model) in that matchup. 
            TeamModel winners = model.Rounds.Last().First().Winner;

            // find the last round (a list of matchups),
            // inside that list pick the first matchup model (which is the only one left it cuz is the last round),
            // in that matchup in its entries (two) find
            //the first*  entry where its team competing is not the winner in that matchup
            // *.first() cuz it's a list and you need to specify which one in case there are more than one (there aren't is this case).
            // at the end after first you will have the entry, since we want the team and not the entry, we used .TeamCompeting to take the team competing out of the entry. 
            TeamModel secondPlaceWinners = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != winners).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;
            PrizeModel firstPlace = model.Prizes.Where(x => x.PrizeNumber == 1).FirstOrDefault();
            PrizeModel secondPlace = model.Prizes.Where(x => x.PrizeNumber == 2).FirstOrDefault();


            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.EntredTeams.Count * model.EntryFee;
                if (firstPlace != null)
                {
                    winnerPrize = firstPlace.CalculatePrizePayOut(totalIncome);
                }
                if (firstPlace != null)
                {
                    runnerUpPrize = secondPlace.CalculatePrizePayOut(totalIncome);
                }
            }

            // send email to all tournament participants
           

            string subject = "";
            StringBuilder body = new StringBuilder();
            subject = $"In {model.TournamentName}, {winners.TeamName} has won! ";

            body.AppendLine("<h1> We have a WINNER! </h1>");
            body.AppendLine("<p>Congratulations! </p>");
            body.AppendLine("<br />");
            body.AppendLine();
            body.AppendLine();


            if (winnerPrize > 0)
            {
                body.AppendLine($"<p> {winners.TeamName} will recieve ${winnerPrize}</p>");
            }
            if (runnerUpPrize > 0)
            {
                body.AppendLine($"<p> {secondPlaceWinners.TeamName} will recieve ${runnerUpPrize}</p>");
            }

            body.AppendLine("<p>Thanks for participation to this tournament everyone! </p>");
            body.AppendLine("~Tournament Tracker");

            List<string> bcc = new List<string>();
            foreach(TeamModel t in model.EntredTeams)
            {
                foreach(PersonModel person in t.TeamMembers)
                {
                    if (person.Email.Length > 0)
                    {
                        bcc.Add(person.Email);
                    }
                }
            }

            EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());
            // complete it
            model.CompleteTournament();

        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournamentModel)
        {
            foreach (MatchupModel m in models)
            {
                foreach (List<MatchupModel> round in tournamentModel.Rounds)
                {
                    foreach (MatchupModel roundMatchup in round)
                    {
                        foreach (MatchupEntryModel entry in roundMatchup.Entries)
                        {
                            if (entry.ParentMatchup != null)
                            {
                                if (entry.ParentMatchup.Id == m.Id)
                                {
                                    entry.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(roundMatchup);
                                }
                            }
                        }
                    }
                } 
            }
        }

        private static void MarkWinnersInMatchups(List<MatchupModel> models)
        {
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];
            foreach (MatchupModel m in models) 
            {
                //0 means false, low score wins :
                if (greaterWins == "0")
                {
                    if (m.Entries.Count == 1)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                        continue;
                    }

                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[0].Score == m.Entries[1].Score)
                    {
                        throw new Exception("This application doesn't handle tie games");
                    }
                    else
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                }
                // else including 1 : high score wins 
                else
                {
                    //handle the bye week teams;
                    if (m.Entries.Count == 1)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                        continue;
                    }

                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[0].Score == m.Entries[1].Score)
                    {
                        throw new Exception("This application doesn't handle tie games");
                    }
                    else
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                }
            }
            //// Declaring the winner by comparing The two team's score
            //if (teamOneScore > teamTwoScore)
            //{
            //    m.Winner = m.Entries[0].TeamCompeting;
            //}
            //else if (teamOneScore == teamTwoScore)
            //{
            //    MessageBox.Show("This application doesn't handle tie games");

            //}
            //else
            //{
            //    m.Winner = m.Entries[1].TeamCompeting;

            //}
        }

        public static void CreateOtherRounds(TournamentModel model , int rounds) 
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();


            while (round <= rounds)
            {
                foreach(MatchupModel match in previousRound)
                {
                    // Add for every match in the previous round a new MatchupEntry to the currMatchup and make its parent the match of the previous
                    // round the loop is at.
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    //When the currMatchup list contains 2 matchup entries, Add the currMatchup to the currRound
                    // and reset the currMatchup
                    if (currMatchup.Entries.Count > 1)
                    {
                        // Saving the number of the current round into the current matchup proprety : 'MatchupRound'
                        currMatchup.MatchupRound = round;
                        // adding the current matchup to the current round
                        currRound.Add(currMatchup);
                        // reseting the value of currMathup to be used again
                        currMatchup = new MatchupModel();
                    }
                }
                // after looping and creating a new matchup entry for every 2 matchs in the previous round
                // adding the list of matchups into the tournament proprety : 'Rounds', and seting variables for the next round
                model.Rounds.Add(currRound);
                // copying the information from the currentRound into the variable 'previous round'
                previousRound = currRound;
                // Reseting the currRound to be used again in the next itteration
                currRound = new List<MatchupModel>();
                // incrementing the value of the round so the while loop doesn't keep looping for ever
                round += 1;
            }
        }

        /// <summary>
        ///loop into each team, add it to the currentMatchup variable until it contains two teams or the number of byes is not null,
        /// when it is one of the two cases it adds it to the list of matchups,
        /// resets the variable to be used again and subsrtacts one bye from the number of byes.
        /// </summary>
        /// <param name="numberOfByes"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        static List<MatchupModel> CreateFirstRound(int numberOfByes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();
            foreach (TeamModel team in teams)
            {
                currentMatchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
                if (numberOfByes > 0 || currentMatchup.Entries.Count > 1) 
                {
                    currentMatchup.MatchupRound = 1;
                    output.Add(currentMatchup);
                    currentMatchup = new MatchupModel();
                    // I did not understand how those byes work, in all case we only gonna need one bye per round,
                    // Catchup, the byes will be used only in the first round
                   
                    if (numberOfByes > 0)
                    {
                        numberOfByes -= 1;
                    }
                }

            }
            return output;
        }

        static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;
            for (int i=1; i<=rounds; i++)
            {
                totalTeams *= 2;
            }
            output = totalTeams - numberOfTeams;
            return output;
        }

        static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;
            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }
            return output;
        }

        static List<TeamModel> RandomizeTeamsOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
