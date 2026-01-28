using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    /// <summary>
    /// Modèle Activite correspondant à la table Activite de la BDD
    /// </summary>
    [Table("Activite")]
    [PrimaryKey(nameof(Id), nameof(CoursId))]
    public class Activites
    {
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("CoursID")]
        public int CoursId { get; set; }

        [MaxLength(255)]
        [Column("Libelle")]
        public string? Libelle { get; set; }

        [MaxLength(50)]
        [Column("Image")]
        public string? Image { get; set; }

        [MaxLength(255)]
        [Column("Contenu")]
        public string? Contenu { get; set; }

        [Column("NbPoints")]
        public int NbPoints { get; set; }

        [MaxLength(255)]
        [Column("Explication")]
        public string? Explication { get; set; }

        [MaxLength(50)]
        [Column("Type")]
        public string? Type { get; set; }

        [MaxLength(10)]
        [Column("Niveau")]
        public string? Niveau { get; set; }
    }
}
