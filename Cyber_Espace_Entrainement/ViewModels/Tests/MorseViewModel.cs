using System;
using System.Collections.Generic;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Media;


namespace Cyber_Espace_Entrainement.ViewModels.Tests
{
  /// <summary>
        /// ViewModel pour l'apprentissage du code Morse
        /// </summary>
        public partial class MorseViewModel : ObservableObject
        {
            private readonly Random _random = new();
            private MediaPlayer _mediaPlayer = new();

            // 
            // PROPRIÉTÉS - Données
            // 

            [ObservableProperty]
            private ObservableCollection<MorseCode> listeMorse = new();

            [ObservableProperty]
            private MorseCode? morseActuel;

            [ObservableProperty]
            private string reponseUtilisateur = string.Empty;

            [ObservableProperty]
            private string messageResultat = string.Empty;

            [ObservableProperty]
            private string couleurResultat = "#2196F3";

            [ObservableProperty]
            private bool modeTest = false;

            [ObservableProperty]
            private int score = 0;

            [ObservableProperty]
            private int totalQuestions = 0;

            [ObservableProperty]
            private string questionPosee = "Cliquez sur 'Tester' pour commencer";

            // 
            // CONSTRUCTEUR
            // 

            public MorseViewModel()
            {
                InitialiserCodesMorse();
                MessageResultat = "Prêt à apprendre le code Morse !";
            }

            // 
            // INITIALISATION DES CODES MORSE
            // 

            private void InitialiserCodesMorse()
            {
                // Alphabet complet en Morse
                var codes = new Dictionary<string, string>
            {
                { "A", ".-" }, { "B", "-..." }, { "C", "-.-." }, { "D", "-.." },
                { "E", "." }, { "F", "..-." }, { "G", "--." }, { "H", "...." },
                { "I", ".." }, { "J", ".---" }, { "K", "-.-" }, { "L", ".-.." },
                { "M", "--" }, { "N", "-." }, { "O", "---" }, { "P", ".--." },
                { "Q", "--.-" }, { "R", ".-." }, { "S", "..." }, { "T", "-" },
                { "U", "..-" }, { "V", "...-" }, { "W", ".--" }, { "X", "-..-" },
                { "Y", "-.--" }, { "Z", "--.." },
                { "0", "-----" }, { "1", ".----" }, { "2", "..---" }, { "3", "...--" },
                { "4", "....-" }, { "5", "....." }, { "6", "-...." }, { "7", "--..." },
                { "8", "---.." }, { "9", "----." }
            };

                foreach (var code in codes)
                {
                    ListeMorse.Add(new MorseCode
                    {
                        Lettre = code.Key,
                        Code = code.Value,
                        CheminAudio = $"Resources/morse_{code.Key}.wav" // À adapter selon les fichiers (à revoir) 
                    });
                }
            }

            // 
            // COMMANDES
            // 

            /// <summary>
            /// Afficher le tableau complet
            /// </summary>
            [RelayCommand]
            private void AfficherTableau()
            {
                ModeTest = false;
                MessageResultat = "Tableau du code Morse - Consultez les correspondances";
                CouleurResultat = "#2196F3";
                QuestionPosee = "Consultez le tableau des codes Morse";
                ReponseUtilisateur = string.Empty;
            }

            /// <summary>
            /// Commencer le mode test
            /// </summary>
            [RelayCommand]
            private void CommencerTest()
            {
                ModeTest = true;
                Score = 0;
                TotalQuestions = 0;
                GenererNouvelleQuestion();
                MessageResultat = "Mode test activé - Écoutez et trouvez la lettre !";
                CouleurResultat = "#4CAF50";
            }

            /// <summary>
            /// Générer une nouvelle question aléatoire
            /// </summary>
            private void GenererNouvelleQuestion()
            {
                if (ListeMorse.Count == 0) return;

                int index = _random.Next(ListeMorse.Count);
                MorseActuel = ListeMorse[index];
                ReponseUtilisateur = string.Empty;
                QuestionPosee = $"Quelle lettre correspond à ce son ? ({Score}/{TotalQuestions})";
            }

            /// <summary>
            /// Jouer le son Morse
            /// </summary>
            [RelayCommand]
            private void JouerSon()
            {
                if (MorseActuel == null || !ModeTest)
                {
                    MessageResultat = "Lancez d'abord le mode test !";
                    CouleurResultat = "#FF9800";
                    return;
                }

                try
                {
                    // Simuler le son avec beep pour l'exemple
                    // Dans votre version, chargez le vrai fichier audio
                    MessageResultat = $"Son joué : {MorseActuel.CodeVisuel}";
                    CouleurResultat = "#2196F3";

                    // Atester sur machine : 
                    // _mediaPlayer.Open(new Uri(MorseActuel.CheminAudio, UriKind.Relative));
                    // _mediaPlayer.Play();

                var playerSons = new SoundPlayer(Application.GetResourceStream(
                    new Uri(MorseActuel.CheminAudio, UriKind.Relative)).Stream);
                playerSons.Play();

                // Pour l'instant, afficher le code en console
                System.Diagnostics.Debug.WriteLine($"Son Morse : {MorseActuel.Code} ({MorseActuel.Lettre})");
                }
                catch (Exception ex)
                {
                    MessageResultat = $"Erreur audio : {ex.Message}";
                    CouleurResultat = "#f44336";
                }
            }

            /// <summary>
            /// Valider la réponse de l'utilisateur
            /// </summary>
            [RelayCommand(CanExecute = nameof(CanValider))]
            private void ValiderReponse()
            {
                if (MorseActuel == null) return;

                TotalQuestions++;
                string reponseNormalisee = ReponseUtilisateur.Trim().ToUpper();

                if (reponseNormalisee == MorseActuel.Lettre)
                {
                    Score++;
                    MessageResultat = $"✓ CORRECT ! La réponse était bien '{MorseActuel.Lettre}' ({MorseActuel.Code})";
                    CouleurResultat = "#4CAF50";
                }
                else
                {
                    MessageResultat = $"✗ FAUX ! C'était '{MorseActuel.Lettre}' ({MorseActuel.Code}) et non '{reponseNormalisee}'";
                    CouleurResultat = "#f44336";
                }

                // Passer à la question suivante après 2 secondes
                // gestion d'un timer
                Task.Delay(2000).ContinueWith(_ =>
                {
                    GenererNouvelleQuestion();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }

            private bool CanValider()
            {
                return ModeTest && !string.IsNullOrWhiteSpace(ReponseUtilisateur);
            }

            /// <summary>
            /// Rechercher un code Morse par lettre
            /// </summary>
            [RelayCommand]
            private void RechercherCode(string lettre)
            {
                if (string.IsNullOrWhiteSpace(lettre)) return;

                var morse = ListeMorse.FirstOrDefault(m =>
                    m.Lettre.Equals(lettre.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase));

                if (morse != null)
                {
                    MessageResultat = $" {morse.Lettre} = {morse.CodeVisuel} ({morse.Code})";
                    CouleurResultat = "#2196F3";
                }
                else
                {
                    MessageResultat = $" Lettre '{lettre}' non trouvée";
                    CouleurResultat = "#FF9800";
                }
            }

            /// <summary>
            /// Réinitialiser le test
            /// </summary>
            [RelayCommand]
            private void Reinitialiser()
            {
                ModeTest = false;
                Score = 0;
                TotalQuestions = 0;
                ReponseUtilisateur = string.Empty;
                MorseActuel = null;
                MessageResultat = "Test réinitialisé";
                CouleurResultat = "#2196F3";
                QuestionPosee = "Cliquez sur 'Tester' pour recommencer";
            }
        }
    }