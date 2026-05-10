using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models.UserEnumeration
{
    /// <summary>
    /// Modèle UserEnumeration correspondant à la table UserEnumeration de la BDD
    /// </summary>
    [Table("UserEnumeration")]
    public class UserEnumeration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }          // PK simple autoincrement

        [Column("ActiviteID")]
        public int ActiviteId { get; set; }

        [Column("CoursID")]
        public int CoursId { get; set; }

        [Column("reponse")]
        public bool Reponse { get; set; }

        [Column("Message")]
        public string? Message { get; set; }

        [Column("libelle")]
        public string? Libelle { get; set; }

        [NotMapped]
        public bool ReponseUtilisateur { get; set; }
    }

}
