using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {       
        
        public int Id { get; set; }
        /// <summary>
        /// Represent the name of this particular team.
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// Represents the list of the persons who are members of this team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>() ;


    }
}
