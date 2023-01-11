using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        /// <summary>
        /// A list of the database sources. Exemple : Sql and textfile
        /// </summary>
        public static List<IDataConnections> Connections { get; private set; } = new List<IDataConnections>();
        /// <summary>
        /// Chooses wich classes to put in the connections list (database or textfiles).
        /// Called in the very beginning of the app.
        /// </summary>
        /// <param name="database">
        /// Check if there is a database as a data source.
        /// </param>
        /// <param name="textfiles">
        /// Check if there is a textfiles as a data source.
        /// </param>
        public static void InitializeConnections (bool database, bool textfiles)
        {
            if (database)
            {
                //TODO Set up the sql connector properly
                SqlConnector sql = new SqlConnector();
                Connections.Add(sql);
            }
            if (textfiles)
            {
                //TODO Create textfiles connection
                TextConnection text = new TextConnection();
                Connections.Add(text);
            }
        }
    }
}
