using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;
using TrackLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnections
    {
        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;
            if (people.Count() > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            people.Add(model);

            people.SaveToPeopleFile(GlobalConfig.PeopleFile);
        }

        /// <summary>
        /// Saves a new prize to the text files.
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id.</returns>
        public void CreatePrize(PrizeModel model)
        {

            // Load the text file
            // Convert the text to List<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max Id

            int currentId = 1;
            if (prizes.Count() > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            // Add the bew record with the new ID (max +1)
            prizes.Add(model);

            // Convert the prizes to list<string>
            // save the list<string> to the text file

            prizes.SaveToPrizeFile(GlobalConfig.PrizesFile);
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PeopleFile);
            int currentId = 1;
            if (teams.Count() > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamsFile(GlobalConfig.TeamFile);

        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PeopleFile);
        }
        public void CreateTournament(TournamentModel model)
        {

            // Load the text file and convert the strings inside into a list of tournament model.
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath().LoadFile().
                ConvertToTournamentModels(GlobalConfig.TeamFile, GlobalConfig.PeopleFile, GlobalConfig.PrizesFile);

            int currentId = 1;
            if (tournaments.Count() > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            model.SaveRoundsToFile(GlobalConfig.MatchupFile, GlobalConfig.MatchupEntryFile);

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(GlobalConfig.TournamentFile);
        }

        public List<TournamentModel> GetTournament_All()
        {
            throw new NotImplementedException();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            // lesson 23 53:00 
        }

        public void CompleteTournament(TournamentModel tournamentModel)
        {
            throw new NotImplementedException();
        }
    }
}
