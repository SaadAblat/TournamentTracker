using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerUI
{
    /// <summary>
    /// whoever use this interface need to implement the PrizeComplete methode
    /// </summary>
    public interface IPrizeRequester
    {
        /// <summary>
        /// Takes a prize model in and returns nothing.
        /// </summary>
        /// <param name="model"></param>
        void PrizeComplete(PrizeModel model);
    }
}
