using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        /// <summary>
        /// This Variable stores whatever form calling the CreatePrizeForm to use it outside of the scope of the CreatePrizeForm method
        /// </summary>
        IPrizeRequester callingform;
        /// <summary>
        /// The parameter means that this method is gonna be called by other form that uses the IPrizeRequester interface.
        /// </summary>
        /// <param name="caller">
        /// the form that is calling this method and implementing the interface
        /// </param>
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();

            //storing the caller to use it outside this method, particularly in the event of createPrizeButton_Click
            callingform = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    prizeNameValue.Text, 
                    prizeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);


                GlobalConfig.Connection.CreatePrize(model);

                // Executing the PrizeComplete method used by the calling form 
                // We know that the calling form will have this PrizeComplete method because it uses the interface IPrizeRequseter
                // wich means it is implementing the PrizeComplet method
                // in this way we're not tying this form to any particular form instead we are tying it to whaterver form use the interface IPrizeRequester
                callingform.PrizeComplete(model);

                //TODO Question : What if we need to open CreatePrizeForm from the begining without calling it from another form ?

                this.Close();

                //prizeNameValue.Text = "";
                //prizeNumberValue.Text = "";
                //prizeAmountValue.Text = "0";
                //prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form is invalid");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;


            int PrizeNumber = 0;
            bool isValidNumber = int.TryParse(prizeNumberValue.Text, out PrizeNumber);

            if (!isValidNumber)
            {
                output = false;
            }
            if (PrizeNumber < 1)
            {
                output = false;
            }
            if (prizeNameValue.Text.Length == 0)
            {
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (!prizeAmountValid || !prizePercentageValid)
            {
                output = false;
            }
            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
            }
            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }



            return output;
        }
    }
}
