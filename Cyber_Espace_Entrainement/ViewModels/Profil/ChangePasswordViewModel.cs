using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Profil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Profil
{
    public partial class ChangePasswordViewModel : ObservableObject
    {
        [ObservableProperty] private string oldPassword;
        [ObservableProperty] private string newPassword;
        [ObservableProperty] private string confirmPassword;
        [ObservableProperty] private string message;
        [ObservableProperty] private Action closePageAction;

        private UserService _userService;



        public bool IsError => !string.IsNullOrEmpty(Message) && !Message.Contains("succès");

        [RelayCommand]
        private void ChangePassword()
        {
            Message = string.Empty; // Vérifications simples
            if (string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Message = "Tous les champs doivent être remplis.";
                OnPropertyChanged(nameof(IsError));
                return;
            }
            if (NewPassword.Length < 8)
            {
                Message = "Le nouveau mot de passe doit contenir au moins 8 caractères.";
                OnPropertyChanged(nameof(IsError));
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                Message = "Les mots de passe ne correspondent pas.";
                OnPropertyChanged(nameof(IsError));
                return;
            }
            _userService = new UserService();
            var (success, user, message) = _userService.Authentifier(SessionService.Instance.CurrentLogin, OldPassword);

            if (!success)
            { 
                Message = "L'ancien mot de passe n'est pas bon";
            }
            else
            {
                var result = MessageBox.Show(
                    "Votre mot de passe a bien été changé\n\n" ,
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
                
            OnPropertyChanged(nameof(IsError)); }
        }
    }

