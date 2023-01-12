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
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetPerson_All();
        TeamModel CreateTeam(TeamModel model);
        void UpdateMatchup(MatchupModel model);
        List<TeamModel> GetTeam_All();
        void CreateTournament(TournamentModel model);
        List<TournamentModel> GetTournament_All();
    }
}
