using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Order_Aggregate
{
	public class Address
	{
        public Address()
        {
            
        }
        public Address(string firstName, string secondName, string street, string city, string country)
		{
			FirstName = firstName;
			SecondName = secondName;
			Street = street;
			City = city;
			Country = country;
		}

		public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
