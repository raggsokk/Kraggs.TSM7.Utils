﻿using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Kraggs.TSM7.Utils.Tests
{
    public class TestUtils
    {
        protected static string Username { get; set; }
        protected static string Password { get; set; }
        protected static string Server { get; set; }
        protected static int? Port { get; set; }

        static TestUtils()
        {
            var credfile = @"..\..\creds.ignore";

            var content = File.ReadAllLines(credfile);

            Username = content[0];
            Password = content[1];
            Server = content[2];
            Port = int.Parse(content[3]);
        }

        public static clsDsmAdmc CreateDsmAdmc()
        {
            return new clsDsmAdmc(Username, Password, Server, Port);
        }
        public static clsDsmAdmc CreateDsmAdmc(string OptFile)
        {
            return new clsDsmAdmc(Username, Password, OptFile);
        }
    }
}