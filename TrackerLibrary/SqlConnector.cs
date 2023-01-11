using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnections
    {
        //TODO Make the createPrize method actually saves the new prize to the database
        /// <summary>
        /// Saves a new prize to the database
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
