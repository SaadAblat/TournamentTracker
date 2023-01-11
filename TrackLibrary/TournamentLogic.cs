﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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