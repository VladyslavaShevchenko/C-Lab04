using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Lab04
{
    internal static class AgeCalcAdapter
    {
        public static List<Person> Users { get; }

        static AgeCalcAdapter()
        {
            var filepath = Path.Combine(GetAndCreateDataPath(), Person.filename);
            if (File.Exists(filepath))
            {
                try
                {
                    Users = SerializeHelper.Deserialize<List<Person>>(filepath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to get user list from file.{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }
            else
            {
                Users = new List<Person>();
                Random rnd = new Random();
                for (int i = 0; i < 50; ++i)
                    Users.Add(new Person($"{(char)(rnd.Next(65, 90))}", $"{(char)(rnd.Next(65, 90))}", $"user{i}@gmail.com", new DateTime(rnd.Next(DateTime.Today.Year - 110, DateTime.Today.Year - 10), rnd.Next(1, 13), rnd.Next(1, 25))));
            }
        }

        internal static Person CreateUser(string firstName, string lastName, string email, DateTime date)
        {
            Person newPerson = new Person(firstName, lastName, email, date);
            Users.Add(newPerson);
            return newPerson;
        }

        internal static void SaveData()
        {
            SerializeHelper.Serialize(Users, Path.Combine(GetAndCreateDataPath(), Person.filename));
        }

        private static string GetAndCreateDataPath()
        {
            string dir = StationManager.WorkingDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

    }
}
