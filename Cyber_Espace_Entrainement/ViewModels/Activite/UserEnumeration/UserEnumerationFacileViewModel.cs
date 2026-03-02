using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using Cyber_Espace_Entrainement.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class UserEnumerationFacileViewModel : ObservableObject
    {
        private readonly UserEnumerationService _service;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> messages;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> userEnumeration;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> pasUserEnumeration;

        public UserEnumerationFacileViewModel()
        {
            _service = new UserEnumerationService();

            Messages = new ObservableCollection<UserEnumeration>();
            UserEnumeration = new ObservableCollection<UserEnumeration>();
            PasUserEnumeration = new ObservableCollection<UserEnumeration>();

            LoadMessages();
        }

        private void LoadMessages()
        {
            var data = _service.GetAllUserEnumeration();

            foreach (var item in data)
            {
                Messages.Add(item);
            }
        }

        [RelayCommand]
        private void DropInUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);

            item.Reponse = true; // Mise à jour du modèle
            UserEnumeration.Add(item);       
        }

        [RelayCommand]
        private void DropInPasUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);

            item.Reponse = false; // Mise à jour du modèle
            PasUserEnumeration.Add(item);
        }

        private void RemoveFromAllCollections(UserEnumeration item)
        {
            Messages.Remove(item);
            UserEnumeration.Remove(item);
            PasUserEnumeration.Remove(item);
        }

        [RelayCommand]
        private void ResetItem(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);
            Messages.Add(item);
        }
    }
}
