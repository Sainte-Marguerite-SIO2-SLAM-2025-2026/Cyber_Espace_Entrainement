using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    [Table("Activite")]
    public class Phishing
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("ID")]
        public int ActiviteId { get; set; }

        [ForeignKey("ID")]
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
