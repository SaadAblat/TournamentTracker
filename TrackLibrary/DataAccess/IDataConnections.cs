using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    /// <summary>
    /// Contains all the methods that are dealing with the database.
    /// </summary>
    public interface IDataConnections
    {
        void CreatePrize(PrizeModel model);
        void CreatePerson(PersonModel model);
        List<PersonModel> GetPerson_All();
        void CreateTeam(TeamModel model);
        void UpdateMatchup(MatchupModel model);
        List<TeamModel> GetTeam_All();
        void CreateTournament(TournamentModel model);
        List<TournamentModel> GetTournament_All();
        void CompleteTournament(TournamentModel tournamentModel);
    }
}
