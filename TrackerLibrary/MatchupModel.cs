using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class MatchupModel
    {
        /// <summary>
        /// Represents the both teams competing against each other in this particular matchup.
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// Represents the team that won at the end of this particular match.
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Represents the current round on wich this matchup exists.
        /// </summary>
        public int MatchupRound { get; set; }

    }
}
