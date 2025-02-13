using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactModificationTests : AuthTestBase
    {
        [Test]
        public void ContactModificationTest()
        {
            app.Contacts.IsContactPresent();

            ContactData newData = new ContactData("I.");
            newData.Lastname = "Z.";

            app.Contacts.Modify(newData);
        }
    }
}
