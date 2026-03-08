using Cyber_Espace_Entrainement.ViewModels.Admin;
using System;
using System.Windows;

namespace Cyber_Espace_Entrainement.Views.Admin
{
    public partial class AdminContenu : Window
    {
        private AdminContenuViewModel ViewModel => (AdminContenuViewModel)DataContext;

        public AdminContenu(Models.Admin admin)
        {
            InitializeComponent();

            var vm = new AdminContenuViewModel(admin);
            DataContext = vm;

            vm.FormCleared += OnFormCleared;
        }

        private void OnFormCleared()
        {
            // Prévu pour accueillir le nettoyage de composants non-bindables (ex: PasswordBox)
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is AdminContenuViewModel vm)
                vm.FormCleared -= OnFormCleared;

            base.OnClosed(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.IsEditMode == true)
            {
                var result = MessageBox.Show(
                    "Vous êtes en train de modifier un enregistrement.\n\nVoulez-vous vraiment quitter sans enregistrer ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No) return;
            }

            this.Close();
        }
    }
}