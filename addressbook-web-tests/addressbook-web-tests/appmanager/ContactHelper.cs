using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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

        public ContactHelper Remove(int v)
        {
            manager.Navigator.OpenHomePage();
            SelectContact();
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
        public void InitContactModification(int index)
        {
            driver.FindElements(By.Name("entry"))[index]
                .FindElements(By.TagName("td"))[7]
                .FindElement(By.TagName("a")).Click();
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
            driver.FindElement(By.LinkText("home page")).Click();
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
    }
}
