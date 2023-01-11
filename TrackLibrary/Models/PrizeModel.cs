using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
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

        public PrizeModel()
        {

        }
        
        // Look the video at 05:22:00.
        public PrizeModel(string prizeName, string prizeNumber, string prizeAmount, string prizePercentage)
        {
            PrizeName = prizeName;

            int prizeNumberValue = 0;
            int.TryParse(prizeNumber, out prizeNumberValue);
            PrizeNumber = prizeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }

    }
}
