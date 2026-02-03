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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.ViewModels;
using Cyber_Espace_Entrainement.Services;

namespace Cyber_Espace_Entrainement.Views.Cours
{
    /// <summary>
    /// Logique d'interaction pour CoursContenue.xaml
    /// </summary>
    public partial class CoursContenu : Window
    {
        private CoursContenuViewModel _viewModel;
        private CoursService _coursService;

        public CoursContenu()
        {
            InitializeComponent();

            _coursService = new CoursService();

            // Initialiser le ViewModel avec le cours
            _viewModel = new CoursContenuViewModel(_coursService.GetCoursById(1));

            // Charger les données depuis le ViewModel
            ChargerDepuisViewModel();
        }

        /// <summary>
        /// Constructeur avec un cours spécifique
        /// </summary>
        /// <param name="cours">Le cours à afficher</param>
        public CoursContenu(Models.Cours cours)
        {
            InitializeComponent();

            // Initialiser le ViewModel avec le cours
            _viewModel = new CoursContenuViewModel(cours);

            // Charger les données depuis le ViewModel
            ChargerDepuisViewModel();
        }

        /// <summary>
        /// Charge les données depuis le ViewModel vers les contrôles
        /// </summary>
        private void ChargerDepuisViewModel()
        {
            if (_viewModel == null || _viewModel.CoursActuel == null) return;

            // Titre de la fenêtre
            this.Title = _viewModel.Titre;
            TxtTitreCours.Text = _viewModel.Titre;

            // Textes
            TxtDefinition.Text = _viewModel.Definition;
            TxtExplication.Text = _viewModel.Explication;
            TxtExemple.Text = _viewModel.Exemple;

            // Image 1
            if (!string.IsNullOrEmpty(_viewModel.Image1Path))
            {
                try
                {
                    ImgCours1.Source = new BitmapImage(new Uri(_viewModel.Image1Path, UriKind.RelativeOrAbsolute));
                    BorderImage1.Visibility = Visibility.Visible;
                }
                catch
                {
                    BorderImage1.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                BorderImage1.Visibility = Visibility.Collapsed;
            }

            // Image 2
            if (!string.IsNullOrEmpty(_viewModel.Image2Path))
            {
                try
                {
                    ImgCours2.Source = new BitmapImage(new Uri(_viewModel.Image2Path, UriKind.RelativeOrAbsolute));
                    BorderImage2.Visibility = Visibility.Visible;
                }
                catch
                {
                    BorderImage2.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                BorderImage2.Visibility = Visibility.Collapsed;
            }

            // Note: Image3 n'est pas affichée car on affiche seulement 2 images maximum
        }

        /// <summary>
        /// Gestionnaire du bouton Retour
        /// </summary>
        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Méthode pour mettre à jour le cours affiché
        /// </summary>
        /// <param name="cours">Le nouveau cours à afficher</param>
        public void UpdateCours(Models.Cours cours)
        {
            if (_viewModel == null)
            {
                _viewModel = new CoursContenuViewModel(cours);
            }
            else
            {
                _viewModel.CoursActuel = cours;
            }

            ChargerDepuisViewModel();
        }

        /// <summary>
        /// Accès au ViewModel (optionnel, si besoin)
        /// </summary>
        public CoursContenuViewModel ViewModel => _viewModel;
    }
}