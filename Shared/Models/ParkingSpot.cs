using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ParkingSpot
    {
		private int spotId;
		private string location;
		private string section;
		private string level;
		private string spotStatus;

		public int SpotID
        {
			get { return spotId; }
			set { spotId = value; }
		}


		public string SpotStatus
		{
			get { return spotStatus; }
			set { spotStatus = value; }
		}

		public string Level
		{
			get { return level; }
			set { level = value; }
		}

		public string Section
		{
			get { return section; }
			set { section = value; }
		}

		public string Location 
		{
			get { return location; }
			set { location = value; }
		}

	}
}
