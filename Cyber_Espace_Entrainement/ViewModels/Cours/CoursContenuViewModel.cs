using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cyber_Espace_Entrainement.Models;

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
        }

        public CoursContenuViewModel(Cours cours)
        {
            CoursActuel = cours;
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}