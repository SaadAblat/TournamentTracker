using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLibrary.DataAccess;
using TrackLibrary;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "PrizeModels.csv";
        public const string PeopleFile = "PersonModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModel.csv";
        public const string MatchupFile = "MatchupModel.csv";
        public const string MatchupEntryFile = "MatchupEntryModel.csv";
        /// <summary>
        /// An interface that force every class that is using it to implement all the methods that will deal with the database. 
        /// </summary>
        public static IDataConnections Connection { get; private set; }
        /// <summary>
        /// Chooses which class (sql or textfile) the interface connection gonna represent.
        /// Called in the very beginning of the app.
        /// </summary>
        /// <param name="database">
        /// Check if there is a database as a data source.
        /// </param>
        /// <param name="textfiles">
        /// Check if there is a textfiles as a data source.
        /// </param>
        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                SqlConnector sql = new SqlConnector();
                // the interface can be anything that is agreeing to implement its methods 
                // SqlConnetor class is using the interface same thing for textConnector
                // therfore this is possible

                Connection = (sql);
            }
            else if (db == DatabaseType.TextFile)
            {
                TextConnector text = new TextConnector();
                Connection = (text);
            }
        }
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public static string AppLookupKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}
