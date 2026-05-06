using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    [Table("Phishing")]
    public class Phishing
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("ActiviteID")]
        public int ActiviteId { get; set; }

        [Column("CoursID")]
        public int CoursId { get; set; }

        [Column("Type")]
        public string Type { get; set; } = string.Empty;

        [Column("Image")]
        public string Image { get; set; } = string.Empty;

        [Column("Expediteur")]
        public string Expediteur { get; set; } = string.Empty;

        [Column("Objet")]
        public string Objet { get; set; } = string.Empty;

        [Column("Contenu")]
        public string Contenu { get; set; } = string.Empty;

    }
}
