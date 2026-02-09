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
        private Cours _coursActuel;

        public CoursContenuViewModel()
        {
            // Constructeur vide pour le design-time
            QuitterCommand = new RelayCommand(Quitter);
            DownloadPdfCommand = new RelayCommand(DownloadPdf);
        }

        public CoursContenuViewModel(Cours cours)
        {
            CoursActuel = cours;
            QuitterCommand = new RelayCommand(Quitter);
            DownloadPdfCommand = new RelayCommand(DownloadPdf);
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
                            page.Size(PageSizes.A4);
                            page.Margin(40);

                            // Header avec espacement après le titre
                            page.Header().Column(header =>
                            {
                                header.Item().Text(CoursActuel.Titre)
                                    .FontSize(20)
                                    .SemiBold()
                                    .FontColor("#1565C0");

                                header.Item().PaddingBottom(15);
                            });

                            page.Content().Column(col =>
                            {
                                col.Spacing(12);

                                // Définition
                                col.Item().Text("Définition").FontSize(14).SemiBold().FontColor("#1565C0");
                                col.Item().Text(CoursActuel.Definition).FontSize(11);

                                // Explication
                                col.Item().PaddingTop(8).Text("Explication").FontSize(14).SemiBold().FontColor("#1565C0");
                                col.Item().Text(CoursActuel.Explication).FontSize(11);

                                // Exemple
                                if (!string.IsNullOrEmpty(CoursActuel.Exemple))
                                {
                                    col.Item().PaddingTop(8).Background("#F5F5F5").Padding(10).Column(c => {
                                        c.Item().Text("Exemple pratique :").FontSize(11).Italic().SemiBold();
                                        c.Item().PaddingTop(4).Text(CoursActuel.Exemple).FontSize(10);
                                    });
                                }

                                // Gestion des images (Image 1, 2 et 3)
                                string[] paths = { CoursActuel.Image1, CoursActuel.Image2, CoursActuel.Image3 };
                                foreach (var path in paths)
                                {
                                    if (!string.IsNullOrEmpty(path))
                                    {
                                        try
                                        {
                                            byte[] imageData = ChargerImageDepuisRessource(path);

                                            if (imageData != null && imageData.Length > 0)
                                            {
                                                col.Item().PaddingVertical(8).MaxHeight(200).Image(imageData);
                                            }
                                        }
                                        catch (Exception imgEx)
                                        {
                                            col.Item().Text($"[Image non chargée: {Path.GetFileName(path)}]")
                                                .FontSize(9)
                                                .Italic()
                                                .FontColor("#999999");
                                        }
                                    }
                                }
                            });

                            page.Footer().AlignCenter().Text(x => {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                                x.Span(" / ");
                                x.TotalPages();
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

        /// <summary>
        /// Charge une image depuis les ressources WPF (embarquées ou fichier)
        /// </summary>
        private byte[] ChargerImageDepuisRessource(string resourcePath)
        {
            try
            {
                // Méthode 1 : Essayer de charger comme ressource embarquée
                var packUri = new Uri($"pack://application:,,,{resourcePath}", UriKind.Absolute);
                var resourceInfo = Application.GetResourceStream(packUri);

                if (resourceInfo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        resourceInfo.Stream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch
            {
                // Si ça échoue, essayer comme fichier physique
            }

            try
            {
                // Méthode 2 : Essayer comme chemin de fichier
                string path = resourcePath.TrimStart('/');
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(baseDir, path);

                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }

                // Essayer dans le dossier projet
                string projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent.FullName;
                fullPath = Path.Combine(projectDir, path);

                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }
            }
            catch
            {
                // Ignorer
            }

            return null;
        }



        #endregion

        #region Utilitaires PDF

        /// <summary>
        /// Convertit un chemin de ressource WPF en chemin absolu sur le disque
        /// </summary>
        private string ConvertirCheminRessource(string resourcePath)
        {
            // Supprimer le "/" initial si présent
            if (resourcePath.StartsWith("/"))
            {
                resourcePath = resourcePath.Substring(1);
                resourcePath = resourcePath.Replace("/", "\\");
            }

            // Obtenir le répertoire de base de l'application
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construire le chemin absolu
            string absolutePath = Path.Combine(baseDirectory, resourcePath);

            // Si le fichier n'existe pas dans bin, chercher dans le dossier du projet
            if (!File.Exists(absolutePath))
            {
                // Remonter de bin\Debug\net8.0-windows vers la racine du projet
                string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
                absolutePath = Path.Combine(projectDirectory, resourcePath);
            }
            if (File.Exists(absolutePath))
            { string e = ""; }
            return absolutePath;
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