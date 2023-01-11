using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class PrizeModel
    {
        /// <summary>
        /// the unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the number of the prize.
        /// </summary>
        public int PrizeNumber { get; set; }

        /// <summary>
        /// The name of this prize
        /// </summary>
        public string PrizeName { get; set; }

        /// <summary>
        /// The amount attributed to this prize
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// the percentage of the amount attributed to this prize
        /// </summary>
        public double PrizePercentage { get; set; }

    }
}
