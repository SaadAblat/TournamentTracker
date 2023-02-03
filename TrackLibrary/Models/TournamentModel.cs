using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {

        public event EventHandler<string> OnTournamentComplete;
        /// <summary>
        /// Unique identifier for the tournament
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of this particular tournament
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// The entry fee required to participate in this tournament.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Represents the list of the teams participating in this tournament.
        /// </summary>
        public List<TeamModel> EntredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// Represents the list of prizes available to the winners of this tournament.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// Represents a list of all the rounds of this tournaments. Note that every round is in itself a list
        /// of matchups
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>() ;

        public void CompleteTournament()
        {
            OnTournamentComplete?.Invoke(this, "I don't know what this is used for");
        }

    }
}
