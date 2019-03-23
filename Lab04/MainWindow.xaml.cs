using System.ComponentModel;
using System.Windows;
using FontAwesome.WPF;

namespace Lab04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageAwesome _loader;
        private PersonInputView _personInputView;
        private UsersView _usersView;

        public MainWindow()
        {
            InitializeComponent();
            ShowUsersView();
        }

        private void ShowInputView()
        {
            MainGrid.Children.Clear();
            if (_personInputView == null)
                _personInputView = new PersonInputView(ShowUsersView, ShowLoader);
            MainGrid.Children.Add(_personInputView);
        }

        private void ShowUsersView()
        {
            MainGrid.Children.Clear();
            if (_usersView == null)
                _usersView = new UsersView(ShowInputView);
            else
                ((UsersViewModel)_usersView.DataContext).UpdateUsers();
            MainGrid.Children.Add(_usersView);
        }

        private void ShowLoader(bool isShow)
        {
            LoaderHelper.OnRequestLoader(MainGrid, ref _loader, isShow);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AgeCalcAdapter.SaveData();
            base.OnClosing(e);
        }
    }
}
