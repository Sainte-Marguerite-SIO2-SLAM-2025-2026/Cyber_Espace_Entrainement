using CommunityToolkit.Mvvm.ComponentModel;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class PhishingViewModel : ObservableObject
    {
        private readonly PhishingService _phishingService;
        private List<Models.Phishing> _mails;
        private int _index = 0;

        [ObservableProperty]
        private Models.Phishing mailActuel;

        [ObservableProperty]
        private string messageResultat;

        [ObservableProperty]
        private int score;

        public IRelayCommand LegitCommand { get; set; }
        public IRelayCommand FraudCommand { get; set; }
        public int ScoreMin { get; set; }
        public int TotalQuestions { get; set; }

        public PhishingViewModel()
        {
            TotalQuestions = 20;
            ScoreMin = 15;

            _phishingService = new PhishingService();

            _mails = _phishingService.GetAllPhishing();

            if (_mails.Count > 0)
                mailActuel = _mails[0];

            LegitCommand = new RelayCommand(() => Verifier("Legitime"));
            FraudCommand = new RelayCommand(() => Verifier("Frauduleux"));
        }

        
        private void Verifier(string choix)
        {
            if (MailActuel == null) return;

            if (MailActuel.Type == choix)
            {
                MessageResultat = "✅ Bonne réponse !";
                Score++;
            }
            else
            {
                MessageResultat = "❌ Mauvaise réponse !";
            }

            _index++;

            if (_index < _mails.Count)
                MailActuel = _mails[_index];
            else
                MessageResultat += "\nJeu terminé !";
        }
    }
}
    

