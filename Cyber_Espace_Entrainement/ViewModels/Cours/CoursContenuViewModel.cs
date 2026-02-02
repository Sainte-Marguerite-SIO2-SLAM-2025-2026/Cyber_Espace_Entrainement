using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Commands;
using Cyber_Espace_Entrainement.Services;

namespace Cyber_Espace_Entrainement.ViewModels
{
    /// <summary>
    /// ViewModel pour la fenêtre CoursContenue
    /// Sert de conteneur de données avec notification de changement
    /// </summary>
    public class CoursContenuViewModel : INotifyPropertyChanged
    {
        private Cours _coursActuel;

        public CoursContenuViewModel()
        {
            // Constructeur vide pour le design-time
            QuitterCommand = new RelayCommand(Quitter);
        }

        public CoursContenuViewModel(Cours cours)
        {
            CoursActuel = cours;
            QuitterCommand = new RelayCommand(Quitter);
        }

        #region Propriétés

        /// <summary>
        /// Le cours actuellement affiché
        /// </summary>
        public Cours CoursActuel
        {
            get => _coursActuel;
            set
            {
                _coursActuel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Titre));
                OnPropertyChanged(nameof(Definition));
                OnPropertyChanged(nameof(Explication));
                OnPropertyChanged(nameof(Exemple));
                OnPropertyChanged(nameof(Image1Path));
                OnPropertyChanged(nameof(Image2Path));
                OnPropertyChanged(nameof(Image3Path));
            }
        }

        public string Titre => CoursActuel?.Titre ?? "Sans titre";
        public string Definition => CoursActuel?.Definition ?? string.Empty;
        public string Explication => CoursActuel?.Explication ?? string.Empty;
        public string Exemple => CoursActuel?.Exemple ?? string.Empty;
        public string Image1Path => CoursActuel?.Image1 ?? string.Empty;
        public string Image2Path => CoursActuel?.Image2 ?? string.Empty;
        public string Image3Path => CoursActuel?.Image3 ?? string.Empty;

        #endregion

        #region Commands

        /// <summary>
        /// Commande liée au bouton "Quitter" qui ferme l'application après confirmation.
        /// </summary>
        public ICommand QuitterCommand { get; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Commande Quitter : affiche une confirmation et ferme l'application si l'utilisateur confirme.
        /// </summary>
        private void Quitter(object parameter)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation"
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}