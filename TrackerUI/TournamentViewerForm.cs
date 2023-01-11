using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {

        private TournamentModel tournament;
        List<int> rounds = new List<int>();
        List<MatchupModel> selectedMatchups = new List<MatchupModel>();

        
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            tournament = tournamentModel;
            LoadFormData();
            LoadRounds();
            //LoadMatchups();
        }

        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

        private void WireUpRoundsLists()
        {
            roundDropDown.DataSource = null; 
            roundDropDown.DataSource = rounds;

        }
        private void WireUpMatchupLists()
        {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadRounds()
        {
            rounds = new List<int>();
            rounds.Add(1);
            int currRound = 1;
            // looping into each round
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                // each round contains a list of matchup models each matchup model has his own round number
                // so every time we loop into a new list of match... I'm confused
                if(matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                }

            }
            WireUpRoundsLists();

        }

        private void LoadMatchups()
        {
            // which round selected
            int round = (int)roundDropDown.SelectedItem;

            // loop into each round in the tournament and find the round that have the same value as the selected int from the roundDropDown
            foreach(List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    selectedMatchups = matchups;
                }
            }
            WireUpMatchupLists();

        }

        private void LoadMatchup()
        {

        }


        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadMatchups();
        }


        private void TournamentViewerForm_Load(object sender, EventArgs e)
        {

        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }


    }
}
