using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Admin;
using Cyber_Espace_Entrainement.Views.Cours;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly AdminService _adminService;

        [ObservableProperty]
        private ObservableCollection<Models.Admin> _admin = new();

        public AdminViewModel()
        {
            _adminService = new AdminService();
            Admin = new ObservableCollection<Models.Admin>();

            LoadAdmin();
        }

        private void LoadAdmin()
        {
            try
            {
                var data = _adminService.GetAllAdmin();
                Admin.Clear();
                foreach (var item in data)
                {
                    Admin.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement : {ex.Message}");
            }
        }

        [RelayCommand]
        public void OuvertureAdmin(Models.Admin Admin)
        {
            try
            {
                AdminContenu AdminWindow = new AdminContenu(Admin);
                AdminWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowError($"Erreur d'ouverture : {ex.Message}");
            }


        }
    }
}
