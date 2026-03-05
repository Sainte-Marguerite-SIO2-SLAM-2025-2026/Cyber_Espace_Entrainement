using Cyber_Espace_Entrainement.Models.UserEnumeration;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.ViewModels.Activite;
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
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Activite
{
    /// <summary>
    /// Fenêtre de l'activité "User Enumeration" - niveau facile.
    /// gère le drag & drop des items, la validation et les interactions UI liées au bouton "Quitter".
    /// Le DataContext attendu est `UserEnumerationFacileViewModel`.
    /// </summary>
    public partial class UserEnumerationFacileView : Window
    {
        // Couleurs utilisées pour l'état normal et survol du bouton Quitter.
        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;

        /// <summary>
        /// Initialise la classe UserEnumerationFacileView et les couleurs du bouton Quitter
        /// </summary>
        public UserEnumerationFacileView()
        {
            InitializeComponent();
            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");
        }

        /// <summary>
        /// Fermeture de la fenêtre (retour au menu).
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Déclenche un drag quand l'utilisateur maintient le bouton gauche sur un message.
        /// Vérifie que le ViewModel est présent et que l'activité n'est pas déjà validée.
        /// </summary>
        private void Message_MouseMove(object sender, MouseEventArgs e)
        {
            var vm = DataContext as UserEnumerationFacileViewModel;
            if (vm == null || vm.EstValide)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // On attend que l'élément visuel soit un Border contenant le DataContext UserEnumeration.
                Border border = sender as Border;
                if (border?.DataContext is UserEnumeration item)
                {
                    // Lancer l'opération de drag-drop. L'item est passé comme donnée pour le Drop.
                    DragDrop.DoDragDrop(border, item, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Drop handler pour la colonne "UserEnumeration" (catégorie correcte).
        /// Exécute la commande du ViewModel pour traiter l'élément déplacé.
        /// </summary>
        private void UserEnum_Drop(object sender, DragEventArgs e)
        {
            var vm = DataContext as UserEnumerationFacileViewModel;

            if (vm == null)
                return;

            if (vm.EstValide)
                return;

            // Vérifier que la donnée correspond bien au type attendu pour éviter les cast invalides.
            if (e.Data.GetDataPresent(typeof(UserEnumeration)))
            {
                var item = (UserEnumeration)e.Data.GetData(typeof(UserEnumeration));
                vm.DropInUserEnumerationCommand.Execute(item);
            }
        }

        /// <summary>
        /// Drop handler pour la colonne "PasUserEnumeration" (catégorie incorrecte).
        /// </summary>
        private void PasUserEnum_Drop(object sender, DragEventArgs e)
        {
            var vm = DataContext as UserEnumerationFacileViewModel;

            if (vm == null)
                return;

            if (vm.EstValide)
                return;

            if (e.Data.GetDataPresent(typeof(UserEnumeration)))
            {
                var item = (UserEnumeration)e.Data.GetData(typeof(UserEnumeration));
                vm.DropInPasUserEnumerationCommand.Execute(item);
            }
        }

        /// <summary>
        /// Double-clic sur un message : remet l'élément dans sa liste d'origine via la commande ResetItemCommand.
        /// Protection contre l'état validé et contre DataContext manquant.
        /// </summary>
        private void Message_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as UserEnumerationFacileViewModel;
            if (vm == null)
                return;

            if (vm.EstValide)
                return;
            if (e.ClickCount == 2)
            {
                if ((sender as FrameworkElement)?.DataContext is UserEnumeration item)
                {
                    vm.ResetItemCommand.Execute(item);
                }
            }
        }

        /// <summary>
        /// Clic sur le bouton "Quitter" : confirme puis ferme l'application si l'utilisateur confirme.
        /// </summary>
        private void ButtonQuitter_Click(object sender, RoutedEventArgs e)
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
        /// MouseEnter du bouton Quitter : changer le background
        /// </summary>
        private void ButtonQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _hoverQuitBackground;
        }

        /// <summary>
        /// MouseLeave du bouton Quitter : restauration du background.
        /// </summary>
        private void ButtonQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _defaultQuitBackground;
        }

        /// <summary>
        /// affiche une MessageBox avec les règles du jeu lorsque l'utilisateur clique sur le bouton "Règles".
        /// </summary>
        private void Regles_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxService.ShowInformation(
                
                "1) Glissez chaque message dans la bonne catégorie.\n" +
                "2) Double-cliquez pour remettre un message.\n" +
                "3) Cliquez sur Valider quand tout est classé.\n" +
                "4) Les bonnes réponses s'afficheront en vert et les mauvaises en rouge.\n\n" +
                "Bonne chance !",
                "RÈGLES DU JEU :"
                );
        }
    }
}
