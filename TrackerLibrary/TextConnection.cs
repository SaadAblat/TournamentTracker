using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class TextConnection : IDataConnections
    {
        //TODO Make the CreatePrize actually works for the tex files

        /// <summary>
        /// Saves a new prize to the text files.
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}
