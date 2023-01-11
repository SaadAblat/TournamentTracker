using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Initialize the database connections
            // We are using an enum 'DatabaseType' instead of a string to avoid writing manually the value of it.
            //TODO Understand more what a global class is.
            // in a global class you can acces directly the methods and variable inside
            // exemple global class called global config
            // globalconfig.something();
            // not global class called Class
            // Class instanceOftheClasee = new Class(); 
            TrackerLibrary.GlobalConfig.InitializeConnections(TrackLibrary.DatabaseType.Sql);
            
            
            Application.Run(new DashboardForm());
        }
    }
}
