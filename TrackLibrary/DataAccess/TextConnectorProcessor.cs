using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        //***********************************************************************************************************
        //******************************* 1) Create the path to the file*********************************************
        //************************Take in the file name and add it at the end of the path****************************
        //***********************************************************************************************************
        public static string FullFilePath(this string fileName)
        {
            // add the new file name to the path and give you the full path
            // see the video on tim's channel about extension methods
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        //***********************************************************************************************************
        //******************************* 2) Take in the file path and load the file*********************************
        //***********************************************************************************************************
        //***********************************************************************************************************
        public static List<string> LoadFile (this string file)
        {
            
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        //***********************************************************************************************************
        //******************************* 3) Decode the strings into Corresponding models****************************
        //***********************************************************************************************************
        //***********************************************************************************************************

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach(string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PrizeNumber = int.Parse(cols[1]);
                p.PrizeName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }

            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.Email = cols[3];
                p.Cellphone = cols[4];
                output.Add(p);
            }
            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');
                foreach(string id in personIds)
                {
                    //TODO didn't understand this part look the minute 9:45:00 in tim's video.
                    // This line of code, searchs in the list of personModels for the person with the correspondant id in the string and add it to the teamMembers list 
          
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }
                output.Add(t);
                
            }
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines,
            string teamModelsFile,
            string peopleFileName,
            string prizeModelsFile)
        {
            // TournamentId, TournamentName, EntryFee, Team1Id | TeamNId, Prize1Id | PrizeNId, Matchup1Id ^ MatchupNId ^ MatchupId | Matchup1Id ^ Matchup2Id | .. | 
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamModelsFile.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizeModels = prizeModelsFile.FullFilePath().LoadFile().ConvertToPrizeModels();
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = int.Parse(cols[2]);

                string[] teamsId = cols[3].Split('|');
                foreach(string id in teamsId)
                {
                    // This line of code, searchs in the teams list for the team with the correspondant id and add it to the EnteredTeam list 
                    tm.EntredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                if (cols[4].Length > 0)
                {
                    string[] prizesId = cols[4].Split('|');
                    foreach (string id in prizesId)
                    {
                        // This line of code, searchs in the Prizes list for the prize with the correspondant id and add it to the Tournament Prizes list 
                        tm.Prizes.Add(prizeModels.Where(x => x.Id == int.Parse(id)).First());
                    } 
                }
                

                // Capture round information
                string[] rounds = cols[5].Split('|');
                //// round  -  Matchup1Id ^ MatchupNId ^ MatchupId |
                foreach (string round in rounds)
                {
                    //Matchup1Id ^ MatchupNId ^ MatchupId

                    string[] matchupsTextIds = round.Split('^');
                    //Matchup1Id ^
                    List<MatchupModel> matchupModels = new List<MatchupModel>();

                    foreach (string matchupTextId in matchupsTextIds)
                    {
                        //Matchup1Id Serach for its id inside the list of matchups and Add the correspondant matchup into the list of matchups models
                        matchupModels.Add(matchups.Where(x => x.Id == int.Parse(matchupTextId)).First());
                    }
                    // // Add the list matchups to the tm.rounds
                    tm.Rounds.Add(matchupModels);
                }
                output.Add(tm);
            }
            return output;
        }

        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            // id=0, entry | entry , winner , round
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.Entries = ConvertStringToMatchupEntryModels(cols[1]);
                if (cols[2].Length == 0)
                {
                    p.Winner = null ;
                }
                else
                {
                    p.Winner = LookupTeamById(int.Parse(cols[2]));

                }

                p.MatchupRound = int.Parse(cols[3]);
                output.Add(p);
            }

            return output;
        }

        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                MatchupEntryModel p = new MatchupEntryModel();
                p.Id = int.Parse(cols[0]);
                if (cols[1].Length == 0)
                {
                    p.TeamCompeting = null;
                }
                else
                {
                    p.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                }
                p.Score = double.Parse(cols[2]);

                int parentId = 0;
                if (int.TryParse(cols[3], out parentId))
                {
                    p.ParentMatchup = LookupMatchupById(int.Parse(cols[3]));
                }
                else
                {
                    p.ParentMatchup = null;
                }

            }

            return output;
        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<string> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach(string entry in entries)
                {
                    string[] cols = entry.Split(',');
                    if (cols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
            }
            output = matchingEntries.ConvertToMatchupEntryModels();
            return output;
        }



        //***********************************************************************************************************
        //************************ 4) Convert models into string of ids to save complex models **********************
        //***************exemple to save team model we need to convert people into string of ids first***************
        //***********************************************************************************************************
        //***********************************************************************************************************


        /// <summary>
        /// This method convert a list of personModel into a string of ids separated by '|'. 
        /// The method SaveToTeamsFile depend on it.
        /// </summary>
        /// <param name="people">
        /// the people in the team
        /// </param>
        /// <returns></returns>
        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            // 9:52:00
            //initializing the output 
            string output = "";

            //returning an empty string if the list of people is empty.
            if (people.Count == 0)
            {
                return "";
            }

            // Looping into each personModel in the list of people and converting it into the id of the person + '|'.
            foreach (PersonModel p in people)
            {
                output += $"{p.Id}|";
            }

            // so far we will have something like 2|3|5|.
            //The line of code Remove the last charachter | after the loop is over.
            output = output.Substring(0, output.Length - 1);

            //now we have 2|3|5
            return output;
        }
        private static string ConvertTeamsListToString(List<TeamModel> teams)
        {
            //initializing the output 
            string output = "";

            //returning an empty string if the list of teams is empty.
            if (teams.Count == 0)
            {
                return "";
            }

            // Looping into each teamModel in the list of teams and converting it into the id of the team + '|'.
            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }

            // so far we will have something like 2|3|5|.
            //The line of code Remove the last charachter | after the loop is over.
            output = output.Substring(0, output.Length - 1);

            //now we have 2|3|5
            return output;
        }
        private static string ConvertPrizesListToString(List<PrizeModel> prizes)
        {
            //initializing the output 
            string output = "";

            //returning an empty string if the list of teams is empty.
            if (prizes.Count == 0)
            {
                return "";
            }

            // Looping into each teamModel in the list of teams and converting it into the id of the team + '|'.
            foreach (PrizeModel p in prizes)
            {
                output += $"{p.Id}|";
            }

            // so far we will have something like 2|3|5|.
            //The line of code Remove the last charachter | after the loop is over.
            output = output.Substring(0, output.Length - 1);

            //now we have 2|3|5
            return output;
        }
        /// <summary>
        /// Convert a list of mathupModels into a string separated with ^
        /// </summary>
        /// <param name="matchup"></param>
        /// <returns></returns>
        private static string ConvertMatchupListToString(List<MatchupModel> matchup)
        {
            string output = "";

            if (matchup.Count == 0)
            {
                return "";
            }
            foreach (MatchupModel p in matchup)
            {
                output += $"{p.Id}^";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }
        /// <summary>
        /// Convert a list of lists of matchupModels into a string separated as in the following example :
        /// Rounds - id^id^id|id^id^id|id^id^id
        /// </summary>
        /// <param name="rounds"></param>
        /// <returns></returns>
        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            //initializing the output 
            string output = "";

            //returning an empty string if the list of Rounds is empty.
            if (rounds.Count == 0)
            {
                return "";
            }

            // Looping into each round,
            // converting all the matchup inside it into ids separated by "^" (by using the method convertMatchupListToString),
            // then adding "|" after the last matchup.
            // As a result we will have rounds separated by "|" and inside each round matchups separated by "^"
            foreach (List<MatchupModel> r in rounds)
            {
                output = $"{ConvertMatchupListToString(r)}|";
            }

            // Remove the last charachter | after the loop is over.
            output = output.Substring(0, output.Length - 1);

            return output;
        }
        private static string ConvertMatchupEntryListToString(List<MatchupEntryModel> entries)
        {
            //initializing the output 
            string output = "";

            //returning an empty string if the list of teams is empty.
            if (entries.Count == 0)
            {
                return "";
            }

            // Looping into each teamModel in the list of teams and converting it into the id of the team + '|'.
            foreach (MatchupEntryModel p in entries)
            {
                output += $"{p.Id}|";
            }

            // so far we will have something like 2|3|5|.
            //The line of code Remove the last charachter | after the loop is over.
            output = output.Substring(0, output.Length - 1);

            //now we have 2|3|5
            return output;
        }

        //***********************************************************************************************************
        //************************* 5) Save strings into correspondant file *****************************************
        //***********************************************************************************************************
        //***********************************************************************************************************


        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PrizeNumber},{p.PrizeName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string filenName)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.Email},{p.Cellphone}");
            }
            File.WriteAllLines(filenName.FullFilePath(), lines);
        }

        public static void SaveToTeamsFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }



        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel match in round)
                {
                    match.SaveMatchupToFile(matchupFile, matchupEntryFile);
                }
            }
        }

        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
            int currentId = 1;
            if (matchups.Count() > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }
            matchup.Id = currentId;
            matchups.Add(matchup);



            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }

            List<string> lines = new List<string>();

            foreach (MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{m.Id},{""},{winner},{m.MatchupRound}");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntryModel entry, string matchupEntryFile)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
            int currentId = 1;
            if (entries.Count() > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }
            entry.Id = currentId;
            entries.Add(entry);
            List<string> lines = new List<string>();
            foreach(MatchupEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                
                lines.Add($"{e.Id},{teamCompeting},{e.Score},{parent}");
            }
            // save to file
            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TournamentModel tm in models)
            {
                lines.Add($"{tm.Id},{tm.TournamentName},{ tm.EntryFee},{ ConvertTeamsListToString(tm.EntredTeams)},{ ConvertPrizesListToString(tm.Prizes)},{ ConvertRoundListToString(tm.Rounds)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        //************************************************************************************************************
        //************************************************ others ****************************************************



        private static MatchupModel LookupMatchupById(int id)
        {
            List<string> matchups = GlobalConfig.TeamFile.FullFilePath().LoadFile();
            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConvertToMatchupModels().First();
                }
            }
            return null;
        }

        

        private static TeamModel LookupTeamById(int id)
        {
            List<string> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile(); 
            foreach(string team in teams)
            {
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();
                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeamModels(GlobalConfig.PeopleFile).First();
                }
                
            }
            return null;

        }














    }
}
