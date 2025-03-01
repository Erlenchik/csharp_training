using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace WebAddressbookTests
{
    public class ContactData : IEquatable<ContactData>, IComparable<ContactData>
    {
        private string allPhones;
        private string allEmails;

        public ContactData(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public bool Equals(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(other, this))
            {
                return true;
            }
            return Firstname == other.Firstname && Lastname == other.Lastname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Firstname, Lastname);
        }

        public override string ToString()
        {
            return Firstname + " " + Lastname + " ";
        }

        public int CompareTo(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return 1;
            }
            return Firstname.CompareTo(other.Firstname);
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }

        public string AllPhones 
        { 
            get 
            {
                if (allPhones != null)
                {
                    return allPhones;
                }
                else
                { 
                    return (CleanUp(HomePhone) + CleanUp(MobilePhone) + CleanUp(WorkPhone)).Trim();
                }
            }

          set 
            { 
                allPhones = value;
            } 
        }

        private string CleanUp(string phone)
        {
            if (phone == null || phone == "")
            {
                return "";
            }
            return phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "") + "\r\n";
        }

        public string AllEmails
        {
            get
            {
                if (allEmails != null)
                {
                    return allEmails;
                }
                else
                {
                    return (CleanUp(Email) + CleanUp(Email2) + CleanUp(Email3)).Trim();
                }
            }

            set
            {
                allEmails = value;
            }
        }
    }
}
