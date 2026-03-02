using Cyber_Espace_Entrainement.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    [Table("Cours")]
    public class Cours
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("Titre")]
        public string Titre { get; set; } = string.Empty;

        [Column("Definition")]
        public string Definition { get; set; } = string.Empty;

        [Column("Explication")]
        public string Explication { get; set; } = string.Empty;

        [Column("Exemple")]
        public string Exemple { get; set; } = string.Empty;

        [Column("Image1")]
        public string Image1 { get; set; } = string.Empty;

        [Column("Image2")]
        public string Image2 { get; set; } = string.Empty;

        [Column("Image3")]
        public string Image3 { get; set; } = string.Empty;

        [Column("Lien")]
        public string Lien { get; set; } = string.Empty;

        [Column("Theme")]
        public string Theme { get; set; } = string.Empty;

        [Column("ImageBouton")]
        public string ImageBouton { get; set; } = string.Empty;

        //public string Source => $"/Resources/Images/Icons/{ImageBouton}";
    }
}
