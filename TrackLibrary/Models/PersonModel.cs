using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {
        /// <summary>
        /// the unique identifier for the person.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the first name of this person.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Represents the last name of this person.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Represents the Email of this person.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Represents the Cellphone of this person
        /// </summary>
        public string Cellphone { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        } 


    }
}
