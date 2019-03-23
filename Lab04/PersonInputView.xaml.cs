using System;
using System.Windows.Controls;

namespace Lab04
{
    /// <summary>
    /// Логика взаимодействия для PersonInputView.xaml
    /// </summary>
    internal partial class PersonInputView : UserControl
    {
        public PersonInputView(Action usersViewAction, Action<bool> loaderAction)
        {
            InitializeComponent();
            DataContext = new PersonInputViewModel(usersViewAction, loaderAction);
        }
    }
}
