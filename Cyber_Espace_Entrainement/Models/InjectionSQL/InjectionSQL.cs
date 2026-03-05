using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models.InjectionSQL
{
    public class InjectionSQL
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("ActiviteId")]
        public int ActiviteId { get; set; }

        [ForeignKey("CoursId")]
        public int CoursId { get; set; }

        [Column("Login")]
        public string Login { get; set; } = string.Empty;

        [Column("Password")]
        public string Password { get; set; } = string.Empty;

        [Column("SoldeCompte")]
        public int SoldeCompte { get; set; } = 0;

        [Column("Nom")]
        public string Nom { get; set; } = string.Empty;
        
        [Column("Prenom")]
        public string Prenom { get; set; } = string.Empty;
    }
}
