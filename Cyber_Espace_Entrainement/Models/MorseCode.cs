using System;
using System.Collections.Generic;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
        /// <summary>
        /// Modèle représentant un code Morse
        /// </summary>
        public class MorseCode
        {
            public string Lettre { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public string CheminAudio { get; set; } = string.Empty;

            // Propriété pour l'affichage visuel du code Morse
            public string CodeVisuel => Code
                .Replace(".", "●")
                .Replace("-", "▬");
        }
    }
