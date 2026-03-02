using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using System.Collections.ObjectModel;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class UserEnumerationFacileViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<UserEnumeration> messages;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> userEnumeration;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> pasUserEnumeration;

        public UserEnumerationFacileViewModel()
        {
            Messages = new ObservableCollection<UserEnumeration>();
            UserEnumeration = new ObservableCollection<UserEnumeration>();
            PasUserEnumeration = new ObservableCollection<UserEnumeration>();
        }

        [RelayCommand]
        private void DropInUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            item.Reponse = true; // Mise à jour du modèle
            UserEnumeration.Add(item);
            Messages.Remove(item);
        }

        [RelayCommand]
        private void DropInPasUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            item.Reponse = false; // Mise à jour du modèle
            PasUserEnumeration.Add(item);
            Messages.Remove(item);
        }
    }
}
