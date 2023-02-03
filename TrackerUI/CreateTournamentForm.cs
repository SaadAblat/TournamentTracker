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
using TrackerLibrary.Models;
using TrackLibrary;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {

        // it doesn't matter if connection is sql or text because both are implementing the called method
        // how the compiler knows, well because they are using the interface that force them to have all of its methods
        private List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        private List<TeamModel> selectedTeams = new List<TeamModel>();
        private List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            wireUpLists();
        }

        private void wireUpLists()
        {
            selectTeamDropDown.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            teamsPlayersListBox.DataSource = null;
            teamsPlayersListBox.DataSource = selectedTeams;
            teamsPlayersListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PrizeName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            // the reason why we write (TeamModel) is because C# can't check if the variable is of the type TeamModel.
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;
            if (t != null)
            {
                selectedTeams.Add(t);
                availableTeams.Remove(t);

                wireUpLists();
            }
        }

        private void deleteSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)teamsPlayersListBox.SelectedItem;
            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);

                wireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //Call the Create Prize form
            // It is only possible if this form is IPrizeRequester interface fulfiller 
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }
        public void PrizeComplete(PrizeModel model)
        {
            //Get back from create prize form with a prize model
            // take the prize model and put it into our prize list
            selectedPrizes.Add(model);
            wireUpLists();
        }

        private void createNewLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Call the Create Team form
            createTeamForm frm = new createTeamForm(this);
            frm.Show();

            
        }

        public void TeamComplete(TeamModel model)
        {
            //Get back from create team with a team model
            // Take the Team model and add it to selectedTeams list box

            selectedTeams.Add(model);
            wireUpLists();
        }

        private void deleteSelectedPrizesButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = new PrizeModel();
            if (p != null)
            {
                selectedPrizes.Remove(p);
                wireUpLists();
            }

        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate data
            decimal fee = 0;
            bool feelIsvalid = decimal.TryParse(enterFeeValue.Text, out fee);
            if (!feelIsvalid)
            {
                MessageBox.Show(" You need to enter a valid fee value",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            // Create tournament model
            TournamentModel tm = new TournamentModel();
            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;
            tm.Prizes = selectedPrizes;
            tm.EntredTeams = selectedTeams;

            // Wire up Matchups
            TournamentLogic.CreateRounds(tm);

            // Create Tournament Entry inside the database
            // Create all of the Prizes entries inside the database
            // Create all of the Teams entries inside the database
            GlobalConfig.Connection.CreateTournament(tm);
            tm.AlertUsersToNewRound();
            TournamentViewerForm frm = new TournamentViewerForm(tm);
            frm.Show();
            this.Close();
           


        }
    }
}
