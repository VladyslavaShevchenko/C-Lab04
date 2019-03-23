using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lab04
{
    class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _users;
        private readonly Action _showInputViewAction;
        private readonly DataGrid _dataGrid;

        private RelayCommand _showInputViewCommand;
        private RelayCommand _deleteRowCommand;

        private bool _isBirthdayFilter;
        private bool _isAdultFilter;
        private int _minAgeFilter;
        private int _maxAgeFilter;
        private ObservableCollection<string> _sunSignFilter;
        private ObservableCollection<string> _chineseSignFilter;

        public List<string> AvailableSunSigns { get; }
        public List<string> AvailableChineseSigns { get; }

        public bool IsBirthdayFilter
        {
            get { return _isBirthdayFilter; }
            set
            {
                _isBirthdayFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("Users");
            }
        }
        public bool IsAdultFilter
        {
            get { return _isAdultFilter; }
            set
            {
                _isAdultFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("Users");
            }
        }
        public int MinAgeFilter
        {
            get { return _minAgeFilter; }
            set
            {
                _minAgeFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("Users");
            }
        }
        public int MaxAgeFilter
        {
            get { return _maxAgeFilter; }
            set
            {
                _maxAgeFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("Users");
            }
        }

        public ObservableCollection<string> SunSignFilter
        {
            get { return _sunSignFilter; }
            set
            {
                _sunSignFilter = value;
                _sunSignFilter.CollectionChanged += (sender, e) =>
                {
                    OnPropertyChanged("Users");
                };
                OnPropertyChanged("Users");
            }
        }

        public ObservableCollection<string> ChineseSignFilter
        {
            get { return _chineseSignFilter; }
            set
            {
                _chineseSignFilter = value;
                _chineseSignFilter.CollectionChanged += (sender, e) =>
                {
                    OnPropertyChanged("Users");
                };
                OnPropertyChanged("Users");
            }
        }

        public ObservableCollection<Person> Users
        {
            get
            {
                IEnumerable<Person> query = AgeCalcAdapter.Users.Where(x => x.Age <= MaxAgeFilter && x.Age >= MinAgeFilter);
                if (IsBirthdayFilter)
                    query = query.Where(x => x.IsBirthday);
                if (IsAdultFilter)
                    query = query.Where(x => x.IsAdult);
                if (SunSignFilter != null && SunSignFilter.Count > 0)
                    query = query.Where(x => SunSignFilter.Contains(x.SunSign));
                if (ChineseSignFilter != null && ChineseSignFilter.Count > 0)
                    query = query.Where(x => ChineseSignFilter.Contains(x.ChineseSign));
                _users = new ObservableCollection<Person>(query);
                return _users;
            }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        internal UsersViewModel(DataGrid dataGrid, Action showInputViewAction)
        {
            MaxAgeFilter = 110;
            _users = new ObservableCollection<Person>(AgeCalcAdapter.Users);
            _dataGrid = dataGrid;
            _dataGrid.CellEditEnding += myDG_CellEditEnding;
            _showInputViewAction = showInputViewAction;
            AvailableSunSigns = new List<string>(new string[12] { "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" });
            AvailableChineseSigns = new List<string>(new string[12] { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Sheep" });
            SunSignFilter = new ObservableCollection<string>();
            ChineseSignFilter = new ObservableCollection<string>();
        }

        internal void UpdateUsers()
        {
            if (!_users.Contains(AgeCalcAdapter.Users[AgeCalcAdapter.Users.Count - 1]))
                _users.Add(StationManager.CurrentPerson);
        }


        public RelayCommand ShowInputViewCommand
        {
            get { return _showInputViewCommand ?? (_showInputViewCommand = new RelayCommand(ShowInputViewImpl)); }
        }

        public RelayCommand DeleteRowCommand
        {
            get { return _deleteRowCommand ?? (_deleteRowCommand = new RelayCommand(DeleteRowImpl)); }
        }

        private void ShowInputViewImpl(object o)
        {
            MinAgeFilter = 0;
            MaxAgeFilter = 110;
            IsBirthdayFilter = IsAdultFilter = false;
            SunSignFilter.Clear();
            ChineseSignFilter.Clear();
            _showInputViewAction.Invoke();
        }

        private void DeleteRowImpl(object o)
        {
            Person item = (Person)_dataGrid.SelectedItems[0];
            if (item != null)
            {
                AgeCalcAdapter.Users.Remove(item);
                _users.Remove(item);
            }
        }

        void myDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column is DataGridBoundColumn column)
                {
                    var bindingPath = (column.Binding as Binding)?.Path.Path;
                    int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    try
                    {
                        if (el != null)
                        {
                            string value = el.Text;
                            Person person = (Person)_dataGrid.Items.GetItemAt(rowIndex);
                            switch (bindingPath)
                            {
                                case "FirstName":
                                    person.FirstName = value;
                                    break;
                                case "LastName":
                                    person.LastName = value;
                                    break;
                                case "Email":
                                    person.Email = value;
                                    break;
                                case "BirthDateString":
                                    person.BirthDateString = value;
                                    break;
                                default:
                                    throw new Exception("No such property exists");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($"Error!\n{exception.Message}");
                        e.Cancel = true;
                    }
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
