using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models.UserEnumeration
{
    /// <summary>
    /// Modèle UserEnumeration correspondant à la table UserEnumeration de la BDD
    /// </summary>
    [Table("UserEnumeration")]
    [PrimaryKey(nameof(Id), nameof(ActiviteId), nameof(CoursId))]
    class UserEnumeration
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("ActiviteID")]
        public string ActiviteId { get; set; }

        [Column("CoursID")] // Retirez [Required] ici, car faire partie de la PK implique l'obligation
        public int CoursId { get; set; }

    }

}
