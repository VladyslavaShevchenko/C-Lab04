using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Lab04
{
    [Serializable]
    internal class Person : INotifyPropertyChanged
    {
        internal const string filename = "Users.dat";

        private DateTime _birthDate;
        private string _firstName, _lastName;
        private string _email;
        private int _age;
        private bool _isAdult;
        private bool _isBirthday;
        private string _chineseSign;
        private string _sunSign;

        public string BirthDateString
        {
            get { return _birthDate.ToShortDateString(); }
            set
            {
                string[] dmy = value.Split('/');
                BirthDate = new DateTime(Int32.Parse(dmy[2]), Int32.Parse(dmy[1]), Int32.Parse(dmy[0]));
            }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                int d = (DateTime.Today - value).Days;
                if (d < 0)
                    throw new FutureDateException(value);
                int y = d / 365;
                if (y > 110)
                    throw new PastDateException(value);
                _birthDate = value;
                Age = CalculateAge();
                ChineseSign = CalculateChineseSign();
                SunSign = CalculateSunSign();
                IsAdult = (Age = CalculateAge()) > 18;
                IsBirthday = BirthDate.DayOfYear == DateTime.Today.DayOfYear;
                ChineseSign = CalculateChineseSign();
                SunSign = CalculateSunSign();
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z'-]+$"))
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
                else
                    throw new InvalidNameException($"{value} {_lastName}");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z'-]+$"))
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
                else
                    throw new InvalidNameException($"{_firstName} {value}");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (new EmailAddressAttribute().IsValid(value))
                {
                    _email = value;
                    OnPropertyChanged();
                }
                else
                    throw new InvalidEmailException(value);
            }
        }

        public bool IsAdult
        {
            get { return _isAdult; }
            private set
            {
                _isAdult = value;
                OnPropertyChanged();
            }
        }
        public bool IsBirthday
        {
            get { return _isBirthday; }
            private set
            {
                _isBirthday = value;
                OnPropertyChanged();
            }
        }
        public int Age
        {
            get { return _age; }
            private set
            {
                _age = value;
                OnPropertyChanged();
            }
        }
        public string ChineseSign
        {
            get { return _chineseSign; }
            private set
            {
                _chineseSign = value;
                OnPropertyChanged();
            }
        }
        public string SunSign
        {
            get { return _sunSign; }
            private set
            {
                _sunSign = value;
                OnPropertyChanged();
            }
        }

        private Person(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        private Person(string firstName, string lastName, DateTime birthDate)
        {
            BirthDate = birthDate;
            FirstName = firstName;
            LastName = lastName;
        }

        internal Person(string firstName, string lastName, string email, DateTime birthDate)
            : this(firstName, lastName, birthDate)
        {
            Email = email;
        }

        private int CalculateAge()
        {
            DateTime today = DateTime.Today;
            return today.Year - BirthDate.Year - (today.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
        }

        private string CalculateChineseSign()
        {
            switch (BirthDate.Year % 12)
            {
                case 0:
                    return "Monkey";
                case 1:
                    return "Rooster";
                case 2:
                    return "Dog";
                case 3:
                    return "Pig";
                case 4:
                    return "Rat";
                case 5:
                    return "Ox";
                case 6:
                    return "Tiger";
                case 7:
                    return "Rabbit";
                case 8:
                    return "Dragon";
                case 9:
                    return "Snake";
                case 10:
                    return "Horse";
                case 11:
                    return "Sheep";
                default:
                    throw new ArgumentException();
            }
        }

        private string CalculateSunSign()
        {
            switch (BirthDate.Month)
            {
                case 1:
                    return BirthDate.Day <= 19 ? "Capricorn" : "Aquarius";
                case 2:
                    return BirthDate.Day <= 17 ? "Aquarius" : "Pisces";
                case 3:
                    return BirthDate.Day <= 19 ? "Pisces" : "Aries";
                case 4:
                    return BirthDate.Day <= 19 ? "Aries" : "Taurus";
                case 5:
                    return BirthDate.Day <= 20 ? "Taurus" : "Gemini";
                case 6:
                    return BirthDate.Day <= 20 ? "Gemini" : "Cancer";
                case 7:
                    return BirthDate.Day <= 22 ? "Cancer" : "Leo";
                case 8:
                    return BirthDate.Day <= 22 ? "Leo" : "Virgo";
                case 9:
                    return BirthDate.Day <= 22 ? "Virgo" : "Libra";
                case 10:
                    return BirthDate.Day <= 22 ? "Libra" : "Scorpio";
                case 11:
                    return BirthDate.Day <= 21 ? "Scorpio" : "Sagittarius";
                case 12:
                    return BirthDate.Day <= 21 ? "Sagittarius" : "Capricorn";
                default:
                    throw new ArgumentException();
            }
        }

        #region Implementation

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

    internal class FutureDateException : Exception
    {
        internal FutureDateException(DateTime date) : base($"You don't seem to be born yet: {date.ToShortDateString()}") { }
    }

    internal class PastDateException : Exception
    {
        internal PastDateException(DateTime date) : base($"You seem to be dead already: {date.ToShortDateString()}") { }
    }

    internal class InvalidEmailException : Exception
    {
        internal InvalidEmailException(string email) : base($"Email is invalid: {email}") { }
    }

    internal class InvalidNameException : Exception
    {
        internal InvalidNameException(string name) : base($"Incorrect name: {name}") { }
    }
}
