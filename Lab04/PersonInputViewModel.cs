using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab04
{
    internal class PersonInputViewModel : INotifyPropertyChanged
    {
        private string _firstName = "";
        private string _lastName = "";
        private string _email = "";
        private DateTime _date = DateTime.Today;
        private bool _canExecute;

        private RelayCommand _calculateAgeCommand;

        private RelayCommand _returnToViewCommand;

        private readonly Action _closeAction;

        private readonly Action<bool> _showLoaderAction;

        internal PersonInputViewModel(Action closeAction, Action<bool> showLoader)
        {
            _closeAction = closeAction;
            _showLoaderAction = showLoader;
            CanExecute = false;
        }

        #region Properties
        public bool CanExecute
        {
            get { return _canExecute; }
            private set
            {
                _canExecute = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                CanExecute = CheckIfFilled();
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                CanExecute = CheckIfFilled();
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                CanExecute = CheckIfFilled();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                CanExecute = CheckIfFilled();
                OnPropertyChanged();
            }
        }

        #endregion

        public RelayCommand CalculateAgeCommand
        {
            get { return _calculateAgeCommand ?? (_calculateAgeCommand = new RelayCommand(AgeCalcImpl)); }
        }

        public RelayCommand ReturnToViewCommand
        {
            get { return _returnToViewCommand ?? (_returnToViewCommand = new RelayCommand(RetToViewImpl)); }
        }

        private async void AgeCalcImpl(object o)
        {
            _showLoaderAction.Invoke(true);
            CanExecute = false;
            try
            {
                await Task.Run(() =>
                {
                    StationManager.CurrentPerson = AgeCalcAdapter.CreateUser(_firstName, _lastName, _email, _date);
                    Thread.Sleep(500);
                });
                if (DateTime.Today.Day == _date.Day && DateTime.Today.Month == _date.Month)
                    MessageBox.Show($"Happy Birthday, {FirstName}!");
                _closeAction.Invoke();
                CanExecute = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ClearInputValues();
            _showLoaderAction.Invoke(false);
        }

        private async void RetToViewImpl(object o)
        {
            await Task.Run(() =>
            {
                ClearInputValues();
                CanExecute = false;
            });
            _closeAction.Invoke();
        }

        private void ClearInputValues()
        {
            Date = DateTime.Today;
            FirstName = "";
            LastName = "";
            Email = "";
        }

        private bool CheckIfFilled()
        {
            return _firstName != "" && _lastName != "" && _email != "";
        }

        #region Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
