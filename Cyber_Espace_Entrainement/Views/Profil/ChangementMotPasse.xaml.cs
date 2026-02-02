using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.ViewModels.Profil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Profil
{
    /// <summary>
    /// Logique d'interaction pour ChangementMotPasse.xaml
    /// </summary>
    public partial class ChangementMotPasse : Window
    {
        public ChangementMotPasse() 
        {
            InitializeComponent(); 
            DataContext = new ChangePasswordViewModel(); 
        }

        private void OldPasswordChanged(object sender, RoutedEventArgs e) 
            => ((ChangePasswordViewModel)DataContext).OldPassword = ((PasswordBox)sender).Password; 
        private void NewPasswordChanged(object sender, RoutedEventArgs e) 
            => ((ChangePasswordViewModel)DataContext).NewPassword = ((PasswordBox)sender).Password; 
        private void ConfirmPasswordChanged(object sender, RoutedEventArgs e) 
            => ((ChangePasswordViewModel)DataContext).ConfirmPassword = ((PasswordBox)sender).Password;

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
