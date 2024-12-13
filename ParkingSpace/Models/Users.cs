using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpace.Models
{
    internal class Users
    {
		private int id;
		private string email;
		private string name;
		private string password;
		private string phone;
		private DateTime registrationDate;

		public DateTime RegistrationDate
		{
			get { return registrationDate; }
			set { registrationDate = value; }
		}

		public string Phone
		{
			get { return phone; }
			set { phone = value; }
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		public int ID
		{
			get { return id; }
			set { id = value; }
		}

	}
}
