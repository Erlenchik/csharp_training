﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mantis_tests
{
    public class AccountData
    {
        private string name;
        private string password;

        public AccountData(string name, string password)
        {
            this.name = name;
            this.password = password;
        }

        public string Id { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Email { get; set; }

        public static AccountData GenerateUser()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string username = $"testuser_{timestamp}";
            string password = "password";
            string email = $"{username}@localhost.localdomain";

            return new AccountData(username, password)
            {
                Email = email
            };
        }
    }
}