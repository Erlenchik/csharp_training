using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using WebAddressbookTests;

namespace WebAddressbookTests
{
    public class AddingContactToGroupTests : AuthTestBase
    {
        [Test]
        public void TestAddingContactToGroup()
        {
            if (GroupData.GetAll().Count == 0)
            {
                app.Groups.Create(new GroupData("TestGroup"));
            }

            if (ContactData.GetAll().Count == 0)
            {
                app.Contacts.Creation(new ContactData("FirstName", "LastName"));
            }

            List<GroupData> groups = GroupData.GetAll();
            List<ContactData> contacts = ContactData.GetAll();

            GroupData selectedGroup = null;
            ContactData selectedContact = null;
            
            foreach (GroupData group in groups)
            {
                List<ContactData> groupContacts = group.GetContacts();
                ContactData contact = contacts.FirstOrDefault(c => !groupContacts.Any(gc => gc.Id == c.Id));

                if (contact != null)
                {
                    selectedGroup = group;
                    selectedContact = contact;
                    break;
                }
            }
            
            if (selectedContact == null || selectedGroup == null)
            {
                GroupData newGroup = new GroupData("NewTestGroup_" + DateTime.Now.Ticks);
                app.Groups.Create(newGroup);
                selectedGroup = GroupData.GetAll().FirstOrDefault(g => g.Name == newGroup.Name);
                selectedContact = ContactData.GetAll().First();
            }
            
            List<ContactData> oldList = selectedGroup.GetContacts();
            
            app.Contacts.AddContactToGroup(selectedContact, selectedGroup);
            
            List<ContactData> newList = selectedGroup.GetContacts();
            oldList.Add(selectedContact);
            oldList.Sort();
            newList.Sort();

            Assert.AreEqual(oldList, newList);
        }
    }
}

