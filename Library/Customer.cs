using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library
{
    class Customer
    {
        private string firstName;
        private string lastName;
        private string address;
        private string postcode;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
