using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.ViewModels;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Profil;

namespace Cyber_Espace_Entrainement.Views.Cours
{
    /// <summary>
    /// Logique d'interaction pour CoursContenue.xaml
    /// </summary>
    public partial class CoursContenu : Window
    {
        private CoursContenuViewModel _viewModel;
        private CoursService _coursService;

        // Couleur originale du bouton Profile (utilisée pour restaurer la couleur après animation)
        private Color _profileOriginalColor = (Color)ColorConverter.ConvertFromString("#1565C0");

        // Brushes pour le bouton Quitter
        private SolidColorBrush _defaultQuitBackground;
        private SolidColorBrush _hoverQuitBackground;

        // Brushes pour le bouton Retour (couleur complémentaire orange/amber)
        private SolidColorBrush _defaultRetourBackground;
        private SolidColorBrush _hoverRetourBackground;

        // Brushes pour le bouton Déconnexion
        private SolidColorBrush _defaultDecoBackground;
        private SolidColorBrush _hoverDecoBackground;

        // Brushes pour les boutons de téléchargement
        private SolidColorBrush _defaultDownloadBackground;
        private SolidColorBrush _hoverDownloadBackground;

        public CoursContenu()
        {
            InitializeComponent();
            _coursService = new CoursService();

            // Initialiser le ViewModel avec le cours
            _viewModel = new CoursContenuViewModel(_coursService.GetCoursById(1));

            // Charger les données depuis le ViewModel
            ChargerDepuisViewModel();

            // Initialiser les brushes pour les boutons
            InitialiserBrushes();

            // Attacher les événements des boutons
            AttacherEvenements();
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

            // Initialiser les brushes pour les boutons
            InitialiserBrushes();

            // Attacher les événements des boutons
            AttacherEvenements();

            // Après que la fenêtre soit chargée, configuration additionnelle (ex : mise en place du bouton Profile)
            Loaded += CoursContenu_Loaded;
        }

        /// <summary>
        /// Initialise les brushes pour les boutons depuis les ressources
        /// </summary>
        private void InitialiserBrushes()
        {
            // Bouton Quitter
            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");

            // Bouton Retour (couleur complémentaire orange/amber)
            _defaultRetourBackground = (SolidColorBrush)Application.Current.FindResource("RetourBackBlueBrush");
            _hoverRetourBackground = (SolidColorBrush)Application.Current.FindResource("RetourBackBlueLightBrush");

            // Bouton Déconnexion
            _defaultDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateBrush");
            _hoverDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateDarkBrush");

            // Boutons de téléchargement
            _defaultDownloadBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32"));
            _hoverDownloadBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
        }

        /// <summary>
        /// Attache tous les événements des boutons
        /// </summary>
        private void AttacherEvenements()
        {
            // Bouton Retour
            BackButton.Click += BackButton_Click;
            BackButton.MouseEnter += BackButton_MouseEnter;
            BackButton.MouseLeave += BackButton_MouseLeave;

            // Bouton Quitter
            btnQuitter.Click += BtnQuitter_Click;
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
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

            // Charger les images
            ChargerImages();

            // Après que la fenêtre soit chargée, configuration additionnelle (ex : mise en place du bouton Profile)
            Loaded += CoursContenu_Loaded;
        }

        /// <summary>
        /// Handler appelé lorsque la fenêtre est entièrement chargée.
        /// Utilisé pour des initialisations qui nécessitent que l'arbre visuel soit prêt.
        /// </summary>
        private void CoursContenu_Loaded(object sender, RoutedEventArgs e)
        {
            // Rendre le bouton Profile rond en lui appliquant un template personnalisé
            MakeProfileButtonRound();

            // Attacher les événements de survol et de clic au bouton Profile
            // Ces événements remplacent des triggers XAML et permettent des animations programmatiques.
            Profile.MouseEnter += ProfileButton_MouseEnter;
            Profile.MouseLeave += ProfileButton_MouseLeave;
            Profile.MouseDown += ProfileButton_MouseDown;
            Profile.MouseUp += ProfileButton_MouseUp;
            Profile.Click += ProfileButton_Click;

            // Attacher les événements de survol au bouton Déconnexion (visuels)
            BtnDeco.MouseEnter += BtnDeco_MouseEnter;
            BtnDeco.MouseLeave += BtnDeco_MouseLeave;
        }

        /// <summary>
        /// Charge les images disponibles et en sélectionne 2 aléatoirement si nécessaire
        /// </summary>
        private void ChargerImages()
        {
            // Liste des images disponibles
            var imagesDisponibles = new List<string>();

            if (!string.IsNullOrEmpty(_viewModel.Image1Path))
                imagesDisponibles.Add(_viewModel.Image1Path);

            if (!string.IsNullOrEmpty(_viewModel.Image2Path))
                imagesDisponibles.Add(_viewModel.Image2Path);

            if (!string.IsNullOrEmpty(_viewModel.Image3Path))
                imagesDisponibles.Add(_viewModel.Image3Path);

            // Si aucune image disponible
            if (imagesDisponibles.Count == 0)
            {
                BorderImage1.Visibility = Visibility.Collapsed;
                BorderImage2.Visibility = Visibility.Collapsed;
                BorderAucuneImage.Visibility = Visibility.Visible;
                return;
            }

            // Cacher le message "aucune image"
            BorderAucuneImage.Visibility = Visibility.Collapsed;

            // Si 3 images disponibles, en sélectionner 2 aléatoirement
            if (imagesDisponibles.Count == 3)
            {
                var random = new Random();
                var imagesSelectionnees = imagesDisponibles
                    .OrderBy(x => random.Next())
                    .Take(2)
                    .ToList();
                imagesDisponibles = imagesSelectionnees;
            }

            // Afficher la première image
            if (imagesDisponibles.Count >= 1)
            {
                try
                {
                    ImgCours1.Source = new BitmapImage(new Uri(imagesDisponibles[0], UriKind.RelativeOrAbsolute));
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

            // Afficher la deuxième image
            if (imagesDisponibles.Count >= 2)
            {
                try
                {
                    ImgCours2.Source = new BitmapImage(new Uri(imagesDisponibles[1], UriKind.RelativeOrAbsolute));
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
        }

        #region Bouton Retour

        /// <summary>
        /// Gestionnaire du clic sur le bouton Retour
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Événement MouseEnter pour le bouton Retour (effet hover)
        /// </summary>
        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Background = _hoverRetourBackground;
        }

        /// <summary>
        /// Événement MouseLeave pour le bouton Retour (retour à l'état normal)
        /// </summary>
        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Background = _defaultRetourBackground;
        }

        #endregion

        #region Bouton Quitter

        /// <summary>
        /// Gestionnaire du clic sur le bouton Quitter
        /// Affiche une confirmation et ferme l'application si confirmé
        /// </summary>
        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Événement MouseEnter pour le bouton Quitter (effet hover)
        /// </summary>
        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _hoverQuitBackground;
        }

        /// <summary>
        /// Événement MouseLeave pour le bouton Quitter (retour à l'état normal)
        /// </summary>
        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _defaultQuitBackground;
        }

        #endregion

        #region Bouton Déconnexion

        /// <summary>
        /// Click du bouton Déconnexion : confirme, appelle le service de session pour logout,
        /// ferme la fenêtre courante et ouvre la fenêtre de connexion.
        /// </summary>
        private void BtnDeco_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment vous déconnecter ?",
                "Confirmation de déconnexion"
            );

            if (result == MessageBoxResult.Yes)
            {
                // Appeler la méthode de déconnexion du service de session (singleton)
                SessionService.Instance.Logout();

                // Ouvrir la fenêtre de connexion (MainWindow)
                var connexionWindow = new MainWindow();

                // Fermer la fenêtre actuelle puis afficher la connexion
                this.Close();
                connexionWindow.Show();
            }
        }

        /// <summary>
        /// MouseEnter du bouton Déconnexion : appliquer la couleur hover définie.
        /// </summary>
        private void BtnDeco_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnDeco.Background = _hoverDecoBackground;
        }

        /// <summary>
        /// MouseLeave du bouton Déconnexion : restauration du background.
        /// </summary>
        private void BtnDeco_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnDeco.Background = _defaultDecoBackground;
        }
        #endregion

        #region Événements Bouton Téléchargement

        private void BtnDownload_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = _hoverDownloadBackground;
            }
        }

        private void BtnDownload_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = _defaultDownloadBackground;
            }
        }

        #endregion

        #region Bouton Profile

        /// <summary>
        /// Applique un template personnalisé au bouton Profile pour le rendre rond (ellipse).
        /// Construit un ControlTemplate programmatiquement avec un Grid contenant une Ellipse stylisée.
        /// </summary>
        private void MakeProfileButtonRound()
        {
            var template = new ControlTemplate(typeof(Button));

            // Grid contenant l'ellipse et le contenu
            var grid = new FrameworkElementFactory(typeof(Grid));

            // Ellipse pour donner une forme arrondie au bouton
            var ellipse = new FrameworkElementFactory(typeof(System.Windows.Shapes.Ellipse));
            ellipse.SetValue(System.Windows.Shapes.Shape.FillProperty, Brushes.Transparent);

            // Bordure colorée de l'ellipse (utilise la BorderBrush du bouton)
            ellipse.SetBinding(
                System.Windows.Shapes.Shape.StrokeProperty,
                new System.Windows.Data.Binding("BorderBrush")
                {
                    RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.TemplatedParent)
                }
            );

            // Épaisseur de la bordure de l'ellipse
            ellipse.SetValue(System.Windows.Shapes.Shape.StrokeThicknessProperty, 3.0);

            // Ombre portée pour donner de la profondeur
            var dropShadow = new DropShadowEffect
            {
                BlurRadius = 15,
                ShadowDepth = 4,
                Opacity = 0.3,
                Color = Colors.Black
            };
            ellipse.SetValue(System.Windows.Shapes.Shape.EffectProperty, dropShadow);

            // ContentPresenter centré pour afficher l'image/icone du profil
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Assemblage des éléments dans le template
            grid.AppendChild(ellipse);
            grid.AppendChild(contentPresenter);
            template.VisualTree = grid;

            // Application du template au bouton Profile défini dans le XAML
            Profile.Template = template;
        }

        /// <summary>
        /// MouseEnter : effet visuel combiné (zoom + éclaircissement de la bordure).
        /// Cette logique remplace des triggers XAML et réalise des animations programmatiques.
        /// </summary>
        private void ProfileButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Mise en place d'une ScaleTransform pour l'effet de zoom
                var scaleTransform = new ScaleTransform(1, 1);
                button.RenderTransform = scaleTransform;
                button.RenderTransformOrigin = new Point(0.5, 0.5);

                var scaleAnimation = new DoubleAnimation
                {
                    To = 1.1,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase()
                };

                // Animation X et Y simultanées
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

                // Animation de couleur de la bordure (si la bordure est un SolidColorBrush)
                if (button.BorderBrush is SolidColorBrush borderBrush)
                {
                    var lighterColor = LightenColor(_profileOriginalColor, 0.3f);
                    var colorAnimation = new ColorAnimation
                    {
                        To = lighterColor,
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }
        }

        /// <summary>
        /// MouseLeave : restaure l'apparence originale (taille + couleur de bordure).
        /// </summary>
        private void ProfileButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Retour à l'échelle normale
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        EasingFunction = new QuadraticEase()
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }

                // Restauration de la couleur de bordure d'origine
                if (button.BorderBrush is SolidColorBrush borderBrush)
                {
                    var colorAnimation = new ColorAnimation
                    {
                        To = _profileOriginalColor,
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }
        }

        /// <summary>
        /// MouseDown : petit effet d'enfoncement pour donner du feedback tactile.
        /// </summary>
        private void ProfileButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 0.95,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }
        }

        /// <summary>
        /// MouseUp : ramène l'échelle au niveau de survol (si applicable).
        /// </summary>
        private void ProfileButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.1,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }
        }

        /// <summary>
        /// Click : ouvre la fenêtre de profil (PersonalView).
        /// Utilise ShowDialog pour modalité ; adapter selon besoin (Show si modeless).
        /// </summary>
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de profil
            var profileWindow = new PersonalView();
            profileWindow.ShowDialog();
        }

        #endregion

        #region Utilitaires de couleurs

        /// <summary>
        /// Éclaircit une couleur en appliquant un pourcentage.
        /// Utilisé par les animations pour obtenir une couleur "plus claire".
        /// </summary>
        private Color LightenColor(Color color, float amount)
        {
            return Color.FromArgb(
                color.A,
                (byte)Math.Min(255, color.R + (255 - color.R) * amount),
                (byte)Math.Min(255, color.G + (255 - color.G) * amount),
                (byte)Math.Min(255, color.B + (255 - color.B) * amount)
            );
        }

        /// <summary>
        /// Assombrit une couleur en appliquant un pourcentage.
        /// </summary>
        private Color DarkenColor(Color color, float amount)
        {
            return Color.FromArgb(
                color.A,
                (byte)(color.R * (1 - amount)),
                (byte)(color.G * (1 - amount)),
                (byte)(color.B * (1 - amount))
            );
        }

        #endregion
    }
}