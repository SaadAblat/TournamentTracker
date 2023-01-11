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
using TrackerLibrary.DataAccess;

namespace TrackerUI
{
    public partial class createTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        ITeamRequester callingform;

        public createTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            //CreateSampleData();
            wireUpLists();

            callingform = caller;
        }

        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "saad", LastName = "Ablat" });
            availableTeamMembers.Add(new PersonModel { FirstName = "said", LastName = "Ablat" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "hamid", LastName = "othmani" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "rabie", LastName = "qomal" });
        }
        private void wireUpLists()
        {
            teamMemberDropDown.DataSource = null;
            teamMemberDropDown.DataSource = availableTeamMembers;
            teamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (validatForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.Email = emailValue.Text;
                p.Cellphone = cellphoneValue.Text;

                GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);
                wireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";


            }
            else
            {
                MessageBox.Show("You need to fill in all of the fields !");
            }
        }
        private bool validatForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }

            if (emailValue.Text.Length == 0)
            {
                return false;
            }

            if (cellphoneValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel) teamMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                wireUpLists(); 
            }
        }

        private void renameSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                wireUpLists(); 
            }
        }

        private bool ValidateTeamInfo()
        {
            if (teamNameValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }
        private void createTeamButton_Click(object sender, EventArgs e)
        {
            
            if (ValidateTeamInfo())
            {
                TeamModel t = new TeamModel();
                t.TeamName = teamNameValue.Text;
                t.TeamMembers = selectedTeamMembers;

                GlobalConfig.Connection.CreateTeam(t);

                callingform.TeamComplete(t);

                this.Close();
            }

            else
            {
                MessageBox.Show("You need to fill in the team name field !");
            }
        }
    }
}
