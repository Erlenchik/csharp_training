using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using WebAddressbookTests;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactCreationTests : AuthTestBase
    {
        [Test]
        public void ContactCreationTest()
        {
            ContactData contact = new ContactData("Irina");
            contact.Lastname = "Zaichikova";
            app.Contacts
                .InitContactCreation()
                .FillContactForm(contact)
                .NewContactAdd();
            app.Navigator.ReturnToHomepage();
        }      
    }
}
