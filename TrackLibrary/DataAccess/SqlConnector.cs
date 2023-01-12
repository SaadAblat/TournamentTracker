using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    /// <summary>
    /// Contains all the methods that are dealing with the sql database.
    /// </summary>
    public class SqlConnector : IDataConnections
    {
        private const string db = "Tournaments";
        public PersonModel CreatePerson(PersonModel model)
        {

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@Email", model.Email);
                p.Add("@Phonenumber", model.Cellphone);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PrizeNumber);
                p.Add("@PlaceName", model.PrizeName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }

        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                
                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                foreach(PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);
                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }
                return model;
            }
        }

        public void CreateTournament(TournamentModel tournamentModel)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                saveTournament(connection, tournamentModel);

                saveTournamentPrizes(connection, tournamentModel);

                saveTournamentEntries(connection, tournamentModel);

                saveTournamentRounds(connection, tournamentModel);
            }
        }
        /// <summary>
        /// Takes in a tournamentModel and saves it inside the Tournament Table in sql Database.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tournamentModel"></param>
        private void saveTournament(IDbConnection connection, TournamentModel tournamentModel)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", tournamentModel.TournamentName);
            p.Add("@EntryFee", tournamentModel.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            tournamentModel.Id = p.Get<int>("@id");
        }
        /// <summary>
        /// Saves a tournament prizes inside the TournamentPrizes Table.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tournamentModel"></param>
        private void saveTournamentPrizes(IDbConnection connection, TournamentModel tournamentModel)
        {
            foreach (PrizeModel prize in tournamentModel.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", tournamentModel.Id);
                p.Add("@PrizeId", prize.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentsPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Saves this tournament entries inside the TournamentEntries Table.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tournamentModel"></param>
        private void saveTournamentEntries(IDbConnection connection, TournamentModel tournamentModel)
        {
            foreach (TeamModel team in tournamentModel.EntredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", tournamentModel.Id);
                p.Add("@TeamId", team.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentsEntries_Insert", p, commandType: CommandType.StoredProcedure);
                //TODO See if i forget to add in the following line of code :
                // something.Id = p.Get<int>("@id");
            }
        }
        private void saveTournamentRounds(IDbConnection connection, TournamentModel tournamentModel)
        {
            //List<List<MatchupModels>> Rounds
            //List<MatchupEntries> Entries

            //Loop through the Rounds
            foreach(List<MatchupModel> round in tournamentModel.Rounds)
            {
                //  Loop through the Matchups
                foreach (MatchupModel matchup in round)
                {
                    
                    //Save Matchup
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", tournamentModel.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    
                    matchup.Id = p.Get<int>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        //Save MatchupEntries
                        var par = new DynamicParameters();
                        par.Add("@MatchupId", matchup.Id);
                        
                        if (entry.ParentMatchup == null)
                        {
                            par.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            par.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }
                        if (entry.TeamCompeting == null)
                        {
                            par.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            par.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }
                        
                        par.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        connection.Execute("dbo.spMatchupEntries_Insert", par, commandType: CommandType.StoredProcedure);
                    }
                    
                }
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> teams;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                // teams gonna recieve all the teams stored in the database
                teams = connection.Query<TeamModel>("dbo.spTeams_GetAll").ToList();
                
                // looping in each team
                foreach (TeamModel team in teams)
                {
                    // gathering team members in each team from the database
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return teams;

        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").ToList();
                var p = new DynamicParameters();

                foreach (TournamentModel t in output)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    //Populate prizes
                    t.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    //Populate Teams (All teams in the current tournament
                    t.EntredTeams = connection.Query<TeamModel>("dbo.spTeams_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();



                    foreach (TeamModel team in t.EntredTeams)
                    {
                        // gathering team members in each team from the database
                        p = new DynamicParameters();
                        p.Add("@TeamId", team.Id);

                        team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                    }

                    p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    //Populate Rounds
                    List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    foreach(MatchupModel m in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", m.Id);
                        m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntries_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();

                        //populate each matchup entry (2 entries)
                        // populate each matchup (1 entri)

                        List<TeamModel> allTeams = GetTeam_All();
                        if (m.WinnerId > 0)
                        {
                            m.Winner = allTeams.Where(x => x.Id == m.WinnerId).First();
                        }
                        foreach (var me in m.Entries) 
                        {
                            //TODO 25:19   22- dashboard form / timcury channel
                            if (me.TeamCompetingId > 0)
                            {
                                // find me the team in all the teams the first one that have the matching id
                                me.TeamCompeting = allTeams.Where(x => x.Id == me.TeamCompetingId).First();
                            }
                            if (me.ParentMatchupId > 0)
                            {
                                me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId).First();
                            }
                        }
                    }
                    //List<List<MatchupModel>> = list<Matchups>
                    List<MatchupModel> currRow = new List<MatchupModel>();
                    int currRound = 1;
                    foreach (MatchupModel m in matchups)
                    {
                        if (m.MatchupRound > currRound)
                        {
                            t.Rounds.Add(currRow);
                            currRow = new List<MatchupModel>();
                            currRound += 1;
                        }
                        currRow.Add(m);
                    }
                    t.Rounds.Add(currRow);
                    //TODO 17:23:00 Checkpoint
                }


            }
            return output;
        }

        public void UpdateMatchup(MatchupModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                if (model.Winner != null)
                {
                    p.Add("@id", model.Id);
                    p.Add("@WinnerId", model.Winner.Id);
                    connection.Execute("dbo.spMatchups_Update ", p, commandType: CommandType.StoredProcedure); 
                }
                //dbo.spMatchupEntries_Update     @id int,   @TeamCompetingId int = null,
                foreach (MatchupEntryModel entry in model.Entries)
                {
                    if (entry.TeamCompeting != null)
                    {
                        p = new DynamicParameters();
                        p.Add("@id", entry.Id);
                        p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        p.Add("@Score", entry.Score);
                        connection.Execute("dbo.spMatchupEntries_Update ", p, commandType: CommandType.StoredProcedure); 
                    }
                }
            }



        }

    }
}
