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
        // Service dédié à la récupération des données et opérations métier pour l'activité.
        private readonly UserEnumerationService _service;

        // Collections observables bindées à la vue.
        [ObservableProperty]
        private ObservableCollection<UserEnumeration> messages;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> userEnumeration;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> pasUserEnumeration;

        // Statistiques et état de l'activité.
        [ObservableProperty]
        private int score = 0;

        [ObservableProperty]
        private int totalPropositions = 0;

        [ObservableProperty]
        private string libelle = "User énumération";

        [ObservableProperty]
        private bool estValide;

        [ObservableProperty]
        private string texteBouton = "Valider";

        [ObservableProperty]
        private string messageResultat = string.Empty;


        /// <summary>
        /// Constructeur : initialise les collections, le service et lance le chargement initial.
        /// </summary>
        public UserEnumerationFacileViewModel()
        {
            _service = new UserEnumerationService();

            Messages = new ObservableCollection<UserEnumeration>();
            UserEnumeration = new ObservableCollection<UserEnumeration>();
            PasUserEnumeration = new ObservableCollection<UserEnumeration>();

            // Met à jour l'état CanExecute du bouton chaque fois que la collection Messages change.
            Messages.CollectionChanged += (_, __) => ActionBoutonCommand.NotifyCanExecuteChanged();

            LoadMessages();

            // Nombre total d'items attendus pour le calcul du score / affichage.
            TotalPropositions = _service.GetCountUserEnumeration();
        }

        /// <summary>
        /// Charge les messages depuis le service et mélange aléatoirement l'ordre.
        /// </summary>
        private void LoadMessages()
        {
            var data = _service.GetAllUserEnumeration();

            // Mélange aléatoire et ajout aux Messages.
            foreach (var item in data.OrderBy(x => Guid.NewGuid()))
            {
                Messages.Add(item);
            }
        }

        /// <summary>
        /// Déplace un item vers la liste des "UserEnumeration".
        /// Supprime l'item de toutes les collections avant de l'ajouter pour éviter les doublons.
        /// </summary>
        /// <param name="item">Item déplacé.</param>
        [RelayCommand]
        private void DropInUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);

            // Marquer la réponse de l'utilisateur et ajouter dans la collection appropriée.
            item.ReponseUtilisateur = true; // Mise à jour du modèle
            UserEnumeration.Add(item);       
        }

        /// <summary>
        /// Déplace un item vers la liste des "PasUserEnumeration".
        /// </summary>
        /// <param name="item">Item déplacé.</param>
        [RelayCommand]
        private void DropInPasUserEnumeration(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);

            item.ReponseUtilisateur = false; // Mise à jour du modèle
            PasUserEnumeration.Add(item);
        }

        /// <summary>
        /// Retire un élément de toutes les collections gérées par ce ViewModel.
        /// Utilisé avant d'insérer l'élément dans une autre collection pour garantir l'unicité.
        /// </summary>
        /// <param name="item">Item à retirer.</param>
        private void RemoveFromAllCollections(UserEnumeration item)
        {
            Messages.Remove(item);
            UserEnumeration.Remove(item);
            PasUserEnumeration.Remove(item);
        }

        /// <summary>
        /// Remet un item dans la pile de départ (Messages).
        /// Utilisé par le double-clic dans la vue.
        /// </summary>
        /// <param name="item">Item à réinitialiser.</param>
        [RelayCommand]
        private void ResetItem(UserEnumeration item)
        {
            if (item == null)
                return;

            RemoveFromAllCollections(item);
            Messages.Add(item);
        }

        /// <summary>
        /// Détermine si l'action principale (Valider/Recommencer) est autorisée.
        /// - Quand l'activité n'est pas validée : le bouton est actif seulement si tous les éléments ont été distribués (Messages.Count == 0).
        /// - Quand l'activité est validée : le bouton "Recommencer" est toujours actif.
        /// </summary>
        /// <returns>bool indiquant si la commande est exécutable.</returns>
        private bool CanActionBouton()
        {
            if (!EstValide)
                return Messages.Count == 0; // mode Valider

            return true; // mode Recommencer toujours autorisé
        }

        // Méthode auxiliaire utilisée par la commande de validation.
        private bool CanValidate() { return (Messages.Count == 0); }

        /// <summary>
        /// Valide la configuration actuelle : calcule le score, bascule en mode "validé" et prépare l'UI pour l'affichage du résultat.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanValidate))]
        private void Valider()
        {
            EstValide = true;

            // Calcul du score : +1 pour chaque bonne réponse dans les deux colonnes.
            int scoreTemp = 0;
            foreach (var item in UserEnumeration)
            {
                if (item.Reponse == true)
                    scoreTemp++;
            }
            foreach (var item in PasUserEnumeration)
            {
                if (item.Reponse == false)
                    scoreTemp++;
            }
            Score = scoreTemp;

            TexteBouton = "Recommencer";
            MessageResultat = $"Votre score est de {Score}/{TotalPropositions}";
        }

        /// <summary>
        /// Remet l'état de l'activité à zéro pour permettre une nouvelle tentative.
        /// </summary>
        private void Recommencer()
        {
            Messages.Clear();
            UserEnumeration.Clear();
            PasUserEnumeration.Clear();

            LoadMessages();
            Score = 0;
            EstValide = false;
            TexteBouton = "Valider";
            MessageResultat = string.Empty;

            // Met à jour l'état du bouton principal.
            ActionBoutonCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Commande principale qui bascule entre Valider et Recommencer selon l'état.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanActionBouton))]
        private void ActionBouton()
        {
            if (!EstValide)
                Valider();
            else
                Recommencer();
        }
    }
}
