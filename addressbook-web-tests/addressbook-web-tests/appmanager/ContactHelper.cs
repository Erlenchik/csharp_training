using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;
using WebAddressbookTests;

namespace WebAddressbookTests
{
    public class ContactHelper : HelperBase
    {
        public ContactHelper(ApplicationManager manager) : base(manager) { }

        public ContactHelper Creation(ContactData contact)
        {
            manager.Navigator.OpenHomePage();
            InitContactCreation();
            FillContactForm(contact);
            NewContactAdd();
            ReturnToHomepage();
            return this;
        }

        public ContactHelper InitContactCreation()
        {
            driver.FindElement(By.LinkText("add new")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper Modify(ContactData newData)
        {
            manager.Navigator.OpenHomePage();
            SelectContact();
            InitContactModification(0);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToHomepage();
            return this;

        }

        public ContactHelper Modify(ContactData contact, ContactData newData)
        {
            manager.Navigator.OpenHomePage();
            SelectContact(contact.Id);
            InitContactModification(contact.Id);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToHomepage();
            return this;
        }

        public ContactHelper Remove(int v)
        {
            manager.Navigator.OpenHomePage();
            SelectContact();
            RemoveContact();
            return this;
        }

        public ContactHelper Remove(ContactData contact)
        {
            manager.Navigator.OpenHomePage();
            SelectContact(contact.Id);
            RemoveContact();
            return this;
        }

        public ContactHelper FillContactForm(ContactData contact)
        {
            Type(By.Name("firstname"), contact.Firstname);
            Type(By.Name("lastname"), contact.Lastname);
            return this;
        }

        public ContactHelper NewContactAdd()
        {
            driver.FindElement(By.XPath("//div[@id='content']/form/input[20]")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper IsContactPresent()
        {
            manager.Navigator.OpenHomePage();
            if (!IsElementPresent(By.Name("selected[]")))
            {
                ContactData ContactPresent = new ContactData("missing", "missing");
                Creation(ContactPresent);
                driver.FindElement(By.Name("selected[]")).Click();
            }
            return this;
        }

        public ContactHelper SelectContact()
        {
            driver.FindElement(By.Name("selected[]")).Click();
            return this;
        }

        public ContactHelper SelectContact(String id)
        {
            driver.FindElement(By.XPath("(//input[@name='selected[]' and @value='" + id + "'])")).Click();
            return this;
        }

        public void InitContactModification(int index)
        {
            driver.FindElements(By.Name("entry"))[index]
                .FindElements(By.TagName("td"))[7]
                .FindElement(By.TagName("a")).Click();
        }

        public void InitContactModification(String id)
        {
            driver.FindElement(By.XPath("//a[@href='edit.php?id=" + id + "']")).Click();
        }

        public ContactHelper SubmitContactModification()
        {
            driver.FindElement(By.Name("update")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper RemoveContact()
        {
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper ReturnToHomepage()
        {
            driver.FindElement(By.LinkText("home")).Click();
            return this;
        }

        private List<ContactData> contactCache = null;

        public List<ContactData> GetContactList()
        {
            if (contactCache == null)
            {
                contactCache = new List<ContactData>();
                ICollection<IWebElement> elements = driver.FindElements(By.XPath("//*[@id=\"maintable\"]/tbody/tr[@name=\"entry\"]"));
                foreach (IWebElement element in elements)
                {
                    String contactFirstname = element.FindElement(By.XPath("td[3]")).Text;
                    String contactLastname = element.FindElement(By.XPath("td[2]")).Text;

                    contactCache.Add(new ContactData(contactFirstname, contactLastname));
                }
            }
            return new List<ContactData>(contactCache);
        }

        public int GetContactCount()
        {
            return driver.FindElements(By.XPath("//*[@id=\"maintable\"]/tbody/tr[@name=\"entry\"]")).Count;
        }

        public ContactData GetContactInformationFromTable(int index)
        {
            manager.Navigator.OpenHomePage();

            IList<IWebElement> cells = driver.FindElements(By.Name("entry"))[index]
                .FindElements(By.TagName("td"));

            string lastName = cells[1].Text;
            string firstName = cells[2].Text;
            string address = cells[3].Text;
            string allPhones = cells[5].Text;
            string allEmails = cells[4].Text;

            return new ContactData(firstName, lastName)
            {
                Address = address,
                AllPhones = allPhones,
                AllEmails = allEmails
            };
        }

        public ContactData GetContactInformationFromEditForm(int index)
        {
            manager.Navigator.OpenHomePage();
            InitContactModification(0);

            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string lastName = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");

            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");
            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email3 = driver.FindElement(By.Name("email3")).GetAttribute("value");

            return new ContactData(firstName, lastName)
            {
                Address = address,
                HomePhone = homePhone,
                MobilePhone = mobilePhone,
                WorkPhone = workPhone,
                Email = email,
                Email2 = email2,
                Email3 = email3
            };
        }

        public string GetContactInfoFromEditFormToString(int index)
        {
            manager.Navigator.OpenHomePage();
            InitContactModification(0);

            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string lastName = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");

            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");
            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email3 = driver.FindElement(By.Name("email3")).GetAttribute("value");

            string contactFromEditFormToDetail = firstName + " " + lastName + "\r\n" + address + "\r\n" + "\r\n" + homePhone + "\r\n" + mobilePhone + "\r\n" + workPhone + "\r\n" + "\r\n" + email + "\r\n" + email2 + "\r\n" + email3;

            return contactFromEditFormToDetail.Trim();
        }

        public int GetNumberOfSearchResults()
        {
            manager.Navigator.OpenHomePage();
            string text = driver.FindElement(By.TagName("label")).Text;
            Match m = new Regex(@"\d+").Match(text);
            return Int32.Parse(m.Value);
        }
        public void InitContactDetails(int index)
        {
            driver.FindElements(By.Name("entry"))[index]
                .FindElements(By.TagName("td"))[6]
                .FindElement(By.TagName("a")).Click();

        }

        public string ContactFromTableToDetail(int index)
        {
            string firstName = GetContactInformationFromTable(index).Firstname;
            string lastName = GetContactInformationFromTable(index).Lastname;
            string address = GetContactInformationFromTable(index).Address;
            string allPhones = GetContactInformationFromTable(index).AllPhones;
            string allEmails = GetContactInformationFromTable(index).AllEmails;

            string contactFromTableToDetail = firstName + " " + lastName + "\r\n" + address + "\r\n" + "\r\n" + allPhones + "\r\n" + "\r\n" + allEmails;

            return contactFromTableToDetail.Trim();
        }

        public string GetContactInformationFromDetails(int index)
        {
            manager.Navigator.OpenHomePage();
            InitContactDetails(0);

            string details = driver.FindElement(By.CssSelector("div#content")).Text;
            if (details == null || details == "")
            {
                return "";
            }
            else
            {
                return details.Replace("H: ", "").Replace("M: ", "").Replace("W: ", "");
            }
        }
    }
}