using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    public class RemovalContactFromGroupTests : AuthTestBase
    {
        [Test]
        public void TestRemovalContactFromGroup()
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

            GroupData targetGroup = null;
            ContactData targetContact = null;
            
            foreach (GroupData g in groups)
            {
                List<ContactData> groupContacts = g.GetContacts();
                if (groupContacts.Count > 0)
                {
                    targetGroup = g;
                    targetContact = groupContacts[0];
                    break;
                }
            }
            
            if (targetContact == null)
            {
                foreach (GroupData g in groups)
                {
                    List<ContactData> groupContacts = g.GetContacts();
                    List<ContactData> allContacts = ContactData.GetAll();
                    ContactData c = allContacts.Except(groupContacts).FirstOrDefault();
                    if (c != null)
                    {
                        app.Contacts.AddContactToGroup(c, g);
                        targetGroup = g;
                        targetContact = c;
                        break;
                    }
                }
            }
            
            if (targetContact == null)
            {
                ContactData newContact = new ContactData("Firstname", "Lastname");
                app.Contacts.Creation(newContact);
                targetContact = ContactData.GetAll().FirstOrDefault(c => c.Firstname == "Firstname");
                targetGroup = groups[0];
                app.Contacts.AddContactToGroup(targetContact, targetGroup);
            }
            
            List<ContactData> oldList = targetGroup.GetContacts();
            
            app.Contacts.RemovalContactFromGroup(targetContact, targetGroup);
            
            List<ContactData> newList = targetGroup.GetContacts();
            oldList.RemoveAll(c => c.Id == targetContact.Id);
            oldList.Sort();
            newList.Sort();

            Assert.AreEqual(oldList, newList);
        }
    }
}