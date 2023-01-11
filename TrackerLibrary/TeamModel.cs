using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class TeamModel
    {
        /// <summary>
        /// Represents the list of the persons who are members of this team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>() ;

        /// <summary>
        /// Represent the name of this particular team.
        /// </summary>
        public string TeamName { get; set; }
    }
}
