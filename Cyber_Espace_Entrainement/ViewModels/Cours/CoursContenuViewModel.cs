using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Commands;
using Cyber_Espace_Entrainement.Services;
using Microsoft.Win32; // Pour SaveFileDialog
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace Cyber_Espace_Entrainement.ViewModels
{
    /// <summary>
    /// ViewModel pour la fenêtre CoursContenue
    /// Sert de conteneur de données avec notification de changement
    /// </summary>
    public class CoursContenuViewModel : INotifyPropertyChanged
    {
        private Models.Cours _coursActuel;

        public CoursContenuViewModel()
        {
            // Constructeur vide pour le design-time
            QuitterCommand = new RelayCommand(Quitter);
            DownloadPdfCommand = new RelayCommand(DownloadPdf);
        }

        public CoursContenuViewModel(Models.Cours cours)
        {
            CoursActuel = cours;
            QuitterCommand = new RelayCommand(Quitter);
            DownloadPdfCommand = new RelayCommand(DownloadPdf);
        }

        #region Propriétés

        /// <summary>
        /// Le cours actuellement affiché
        /// </summary>
        public Models.Cours CoursActuel
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

        public ICommand DownloadPdfCommand { get; }

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

        private void DownloadPdf(object parameter)
        {
            if (CoursActuel == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Fichier PDF (*.pdf)|*.pdf",
                FileName = $"Cours_{CoursActuel.Titre.Replace(" ", "_")}",
                Title = "Enregistrer le cours"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Initialisation de la licence (obligatoire)
                    QuestPDF.Settings.License = LicenseType.Community;

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Margin(50);

                            // Header avec espacement après le titre
                            page.Header().Column(header =>
                            {
                                header.Item().Text(CoursActuel.Titre)
                                    .FontSize(24)
                                    .SemiBold()
                                    .FontColor("#1565C0");

                                // Ajouter un espace après le titre
                                header.Item().PaddingBottom(20);
                            });

                            page.Content().Column(col =>
                            {
                                col.Spacing(15);

                                // Définition
                                col.Item().Text("Définition").FontSize(16).SemiBold();
                                col.Item().Text(CoursActuel.Definition);

                                // Gestion des images (Image 1, 2 et 3)
                                string[] paths = { CoursActuel.Image1, CoursActuel.Image2, CoursActuel.Image3 };
                                foreach (var path in paths)
                                {
                                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                                    {
                                        try
                                        {
                                            col.Item().PaddingVertical(10).Image(path).FitWidth();
                                        }
                                        catch (Exception imgEx)
                                        {
                                            // En cas d'erreur sur une image, on continue avec les autres
                                            col.Item().Text($"[Image non chargée: {Path.GetFileName(path)}]")
                                                .FontSize(10)
                                                .Italic()
                                                .FontColor("#999999");
                                        }
                                    }
                                }

                                // Explication
                                col.Item().PaddingTop(10).Text("Explication").FontSize(16).SemiBold();
                                col.Item().Text(CoursActuel.Explication);

                                // Exemple
                                if (!string.IsNullOrEmpty(CoursActuel.Exemple))
                                {
                                    col.Item().PaddingTop(10).Background("#F5F5F5").Padding(10).Column(c => {
                                        c.Item().Text("Exemple pratique :").Italic();
                                        c.Item().PaddingTop(5).Text(CoursActuel.Exemple);
                                    });
                                }
                            });

                            page.Footer().AlignCenter().Text(x => {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                        });
                    }).GeneratePdf(saveFileDialog.FileName);

                    MessageBoxService.ShowInformation("PDF généré avec succès !", "Succès");
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowError($"Erreur : {ex.Message}\n\nDétails: {ex.InnerException?.Message}",
                        "Erreur");
                }
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