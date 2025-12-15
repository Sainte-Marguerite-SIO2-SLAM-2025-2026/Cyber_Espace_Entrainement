using Cyber_Espace_Entrainement.Views;
using Cyber_Espace_Entrainement.Views.Tests;
using Cyber_Espace_Entrainement.Views.Users;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement
{
        /// <summary>
        /// Page d'accueil avec menu de navigation c
        /// </summary>
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
            }

            /// <summary>
            /// Ouvrir l'espace Admin (gestion des utilisateurs)
            /// </summary>
            private void AdminCard_Click(object sender, MouseButtonEventArgs e)
            {
                var adminWindow = new UserGestion();
                adminWindow.ShowDialog(); // Ouvre en modal

                // OU pour ouvrir en nouvelle fenêtre indépendante :
                // adminWindow.Show();
            }

            /// <summary>
            /// Ouvrir l'espace Tests (à créer)
            /// </summary>
            private void TestsCard_Click(object sender, MouseButtonEventArgs e)
            {
            //MessageBox.Show(
            //    "ESPACE TESTS\n\n" +
            //    "Cette section contiendra :\n" +
            //    "• Quiz de cybersécurité\n" +
            //    "• Exercices pratiques\n" +
            //    "• Évaluations\n" +
            //    "• Scores et classements\n\n" +
            //    "À développer...à vous de trouver",
            //    "Espace Tests",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information
            //);
            var morseWindow = new MorseTestView();
            morseWindow.ShowDialog();
        }

            /// <summary>
            /// Ouvrir l'espace Personnel (à créer)
            /// </summary>
            private void PersonalCard_Click(object sender, MouseButtonEventArgs e)
            {
                MessageBox.Show(
                    "ESPACE PERSONNEL\n\n" +
                    "Cette section contiendra :\n" +
                    "• Profil utilisateur\n" +
                    "• Statistiques personnelles\n" +
                    "• Historique des tests\n" +
                    "• Badges et achievements\n\n" +
                    "À développer...",
                    "Espace Personnel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // TODO : Créer PersonalView.xaml par exemple et décommenter la suite !
                // var personalWindow = new PersonalView();
                // personalWindow.ShowDialog();
            }

            /// <summary>
            /// Ouvrir l'espace Cours (à créer)
            /// </summary>
            private void CoursCard_Click(object sender, MouseButtonEventArgs e)
            {
                MessageBox.Show(
                    "ESPACE COURS\n\n" +
                    "Cette section contiendra :\n" +
                    "• Cours de cybersécurité\n" +
                    "• Tutoriels .... vidéo\n" +
                    "• Documentation\n" +
                    "• Ressources pédagogiques\n\n" +
                    "À développer...",
                    "Espace Cours",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // TODO : Créer CoursView.xaml et décommenter :
                // var coursWindow = new CoursView();
                // coursWindow.ShowDialog();
            }

            // 
            // Effetes visuel : à garder ... oupas ! 
            //

            /// <summary>
            /// Effet de zoom au survol de la carte
            /// </summary>
            private void Card_MouseEnter(object sender, MouseEventArgs e)
            {
                if (sender is Border card)
                {
                    // Animation de zoom léger
                    card.RenderTransform = new ScaleTransform(1.05, 1.05);
                    card.RenderTransformOrigin = new Point(0.5, 0.5);

                    // Bordure plus visible
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                    card.BorderThickness = new Thickness(2);

                    // Ombre plus marquée
                    if (card.Effect is DropShadowEffect shadow)
                    {
                        shadow.BlurRadius = 20;
                        shadow.ShadowDepth = 5;
                    }
                }
            }

            /// <summary>
            /// Retour à la normale quand la souris quitte
            /// </summary>
            private void Card_MouseLeave(object sender, MouseEventArgs e)
            {
                if (sender is Border card)
                {
                    // Retour à la taille normale
                    card.RenderTransform = new ScaleTransform(1.0, 1.0);

                    // Bordure normale
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                    card.BorderThickness = new Thickness(1);

                    // Ombre normale
                    if (card.Effect is DropShadowEffect shadow)
                    {
                        shadow.BlurRadius = 10;
                        shadow.ShadowDepth = 2;
                    }
                }
            }

            //
            // BOUTON QUITTER
            //

            private void QuitButton_Click(object sender, RoutedEventArgs e)
            {
                var result = MessageBox.Show(
                    "Voulez-vous vraiment quitter l'application ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }