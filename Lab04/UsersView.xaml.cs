using System;
using System.Windows.Controls;

namespace Lab04
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView(Action showInputViewAction)
        {
            InitializeComponent();
            AgeSlider.LowerValue = AgeSlider.Minimum = 0;
            AgeSlider.UpperValue = AgeSlider.Maximum = 110;
            DataContext = new UsersViewModel(UsersDataGrid, showInputViewAction);
        }
    }
}