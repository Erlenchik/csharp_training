﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using WebAddressbookTests;

namespace WebAddressbookTests
{
    public class AddressBookDB : LinqToDB.Data.DataConnection
    {
        public AddressBookDB() : base("AddressBook") { }

        public ITable<GroupData> Groups { get { return this.GetTable<GroupData>(); } }

        public ITable<ContactData> Contacts { get { return this.GetTable<ContactData>(); } }

        public ITable<GroupContactRelation> GCR { get { return this.GetTable<GroupContactRelation>(); } }
    }
}