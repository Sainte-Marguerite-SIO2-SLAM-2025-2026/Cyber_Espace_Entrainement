using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    /// <summary>
    /// Model Captcha correspondant à la table Captcha de la BDD
    /// </summary>
    [Table("Captcha")]
    public class Captchas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CaptchaID")]
        public int CaptchaId { get; set; }

        [Column("ActiviteID")]
        public int ActiviteId { get; set; }

        [Column("CoursID")]
        public int CourdId { get; set; }

        [Column("Explication")]
        public string? Explication { get; set; }

        [Column("Zone")]
        public string? Zone { get; set; }

        [Column("Image")]
        public string? Image { get; set; }

        [Column("Valide")]
        public bool Valide { get; set; }
    }
}
