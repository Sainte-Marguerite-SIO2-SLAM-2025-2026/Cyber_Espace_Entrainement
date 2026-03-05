using Cyber_Espace_Entrainement.Models.UserEnumeration;
using Cyber_Espace_Entrainement.ViewModels.Activite;
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

namespace Cyber_Espace_Entrainement.Views.Activite
{
    /// <summary>
    /// Logique d'interaction pour UserEnumeration.xaml
    /// </summary>
    public partial class UserEnumerationFacileView : Window
    {
        public UserEnumerationFacileView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Retour au menu principal
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Message_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Border border = sender as Border;
                if (border?.DataContext is UserEnumeration item)
                {
                    DragDrop.DoDragDrop(border, item, DragDropEffects.Move);
                }
            }
        }

        private void UserEnum_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(UserEnumeration)))
            {
                var item = (UserEnumeration)e.Data.GetData(typeof(UserEnumeration));

                if (DataContext is UserEnumerationFacileViewModel vm)
                {
                    vm.DropInUserEnumerationCommand.Execute(item);
                }
            }
        }

        private void PasUserEnum_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(UserEnumeration)))
            {
                var item = (UserEnumeration)e.Data.GetData(typeof(UserEnumeration));

                if (DataContext is UserEnumerationFacileViewModel vm)
                {
                    vm.DropInPasUserEnumerationCommand.Execute(item);
                }
            }
        }

        private void Message_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ((sender as FrameworkElement)?.DataContext is UserEnumeration item)
                {
                    if (DataContext is UserEnumerationFacileViewModel vm)
                    {
                        vm.ResetItemCommand.Execute(item);
                    }
                }
            }
        }
    }
}
