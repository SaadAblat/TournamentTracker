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

        private void WireUpRoundsList()
        {
            roundDropDown.DataSource = null; 
            roundDropDown.DataSource = rounds;

        }
        private void WireUpMatchupsList()
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
            WireUpRoundsList();

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
                    selectedMatchups.Clear();
                    foreach (MatchupModel matchup in matchups)
                    {
                        if (matchup.Winner == null || !unplayedOnlyCheckBox.Checked)
                        {
                            selectedMatchups.Add(matchup);

                        }

                    }
                }
            }
            DisplayMatchupInfo();
            WireUpMatchupsList();
            
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = (selectedMatchups.Count > 0);
            teamOneLabel.Visible = isVisible;
            teamTwoLabel.Visible = isVisible;

            teamOneScoreLabel.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;

            teamOneScoreValue.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            scoreButton.Visible = isVisible;

            versusLabel.Visible = isVisible;
        }

        private void LoadMatchup()
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneLabel.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entries[0].Score.ToString();
                            teamTwoLabel.Text = "<bye>";
                            teamTwoScoreValue.Text = "0";
                        }
                        else
                        {
                            teamOneLabel.Text = "Not yet Set";
                            teamOneScoreValue.Text = "";

                        }
                    }
                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoLabel.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoLabel.Text = "Not yet Set";
                            teamTwoScoreValue.Text = "";
                        }
                    }
                }
            }
            else
            {
                teamOneLabel.Text = "";
                teamTwoLabel.Text = "";
                teamOneScoreValue.Text = "";
                teamTwoScoreValue.Text = "";


            }

        }


        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup();

        }


        private void TournamentViewerForm_Load(object sender, EventArgs e)
        {

        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups();
            LoadMatchup();
        }
        private string ValidateFormat()
        {
            string output = "";

            bool teamOneValid = double.TryParse(teamOneScoreValue.Text, out double teamOneScore);
            bool teamTwoValid = double.TryParse(teamTwoScoreValue.Text, out double teamTwoScore);


            if (!teamOneValid && !teamTwoValid)
            {
                output = " Both teams formats are not valid";
            }
            else if (!teamOneValid)
            {
                output = " Team one format is not valid";
            }
            else if (!teamTwoValid)
            {
                output = " Team two format is not valid";
            }


            else if (teamOneScore ==0 && teamTwoScore == 0)
            {
                output = " You did not enter a score for either teams";
            }
            else if (teamOneScore == teamTwoScore )
            {
                output = " This application doesn't allow ties";
            }

            return output;
        }
        private void scoreButton_Click(object sender, EventArgs e)
        {
            string errorMessage = ValidateFormat();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage);
                return;
            }


            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            double teamOneScore = 0;
            double teamTwoScore = 0;

            // Capturing each team's score
            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
                            if (scoreValid)
                            {
                                m.Entries[0].Score = teamOneScore;
                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid score value for team 1");
                                return;
                            }
                        }
                    }
                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
                            if (scoreValid)
                            {
                                m.Entries[1].Score = teamTwoScore;
                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid score value for team 2");
                                return;
                            }
                            
                        }
                    }
                }
                try
                {
                    TournamentLogic.UpdateTournamentsResults(tournament);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"The application have the following error{ex.Message}");
                    return;
                }
                LoadMatchups();
            }
        }
    }
}
