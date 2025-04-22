using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using LinqToDB.Mapping;
using WebAddressbookTests;
using Microsoft.Office.Interop.Excel;

namespace WebAddressbookTests
{
    [Table(Name = "addressbook")]
    public class ContactData : IEquatable<ContactData>, IComparable<ContactData>
    {
        private const string Pattern = "[ ()-]";
        private string allPhones;
        private string allEmails;
        private string allDetails;
        private string name;

        public ContactData()
        {
        }

        public ContactData(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public bool Equals(ContactData other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(other, this)) return true;
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
            if (Object.ReferenceEquals(other, null)) return 1;
            int result = Lastname.CompareTo(other.Lastname);
            return result == 0 ? Firstname.CompareTo(other.Firstname) : result;
        }

        [Column(Name = "firstname")]
        public string Firstname { get; set; }

        [Column(Name = "lastname")]
        public string Lastname { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "home")]
        public string HomePhone { get; set; }

        [Column(Name = "mobile")]
        public string MobilePhone { get; set; }

        [Column(Name = "work")]
        public string WorkPhone { get; set; }

        [Column(Name = "email")]
        public string Email { get; set; }

        [Column(Name = "email2")]
        public string Email2 { get; set; }

        [Column(Name = "email3")]
        public string Email3 { get; set; }

        [Column(Name = "id"), PrimaryKey, Identity]
        public string Id { get; set; }

        [Column(Name = "deprecated")]
        public string Deprecated { get; set; }

        public string AllPhones
        {
            get
            {
                if (allPhones != null) return allPhones;
                return (FormatPhone("H: ", HomePhone) + FormatPhone("M: ", MobilePhone) + FormatPhone("W: ", WorkPhone)).Trim();
            }
            set => allPhones = value;
        }

        public string AllEmails
        {
            get
            {
                if (allEmails != null) return allEmails;
                return (CleanUp(Email) + CleanUp(Email2) + CleanUp(Email3)).Trim();
            }
            set => allEmails = value;
        }

        public string AllDetails
        {
            get
            {
                if (allDetails != null)
                { 
                    return allDetails; 
                }
                return (Name + "\r\n" + CleanUpEmpty(Address) + CleanUpEmpty(AllPhones) + CleanUpEmpty(AllEmails)).Trim();
            }
            set 
            { 
                allDetails = value; 
            }
        }

        public string Name
        {
            get
            {
                if (name != null) return name;
                return (CleanUpEmptyName(Firstname) + " " + CleanUpEmptyName(Lastname)).Trim();
            }
            set => name = value;
        }

        public static List<ContactData> GetAll()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return (from c in db.Contacts select c).ToList();
            }
        }

        public static ContactData GetLastContact()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return db.Contacts
                    .OrderByDescending(c => c.Id)
                    .Select(c => new ContactData
                    {
                        Id = c.Id,
                        Firstname = c.Firstname,
                        Lastname = c.Lastname
                    })
                    .First();
            }
        }
        
        private string CleanUp(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return Regex.Replace(input, Pattern, "") + "\r\n";
        }

        private string CleanUpEmpty(string input)
        {
            return string.IsNullOrEmpty(input) ? "" : input + "\r\n";
        }

        private string CleanUpEmptyName(string name)
        {
            return string.IsNullOrEmpty(name) ? "" : name;
        }

        private string FormatPhone(string label, string number)
        {
            return string.IsNullOrEmpty(number) ? "" : label + CleanUp(number);
        }
    }
}