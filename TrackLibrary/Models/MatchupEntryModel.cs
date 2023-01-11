using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents one team of the matchup.
    /// </summary>
    public class MatchupEntryModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Represents the Id of team of the matchup.
        /// </summary>
        public int TeamCompetingId { get; set;}
        /// <summary>
        /// Represents one team of the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represents the score of this particular team.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represents the Matchup that the team came from as winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
        /// <summary>
        /// Represents the id of the Matchup that the team came from as winner.
        /// </summary>
        public int ParentMatchupId { get; set; }

    }
}
