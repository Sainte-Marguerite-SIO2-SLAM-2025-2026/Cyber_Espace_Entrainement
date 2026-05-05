using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        [Column("AdminID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminID { get; set; }

        [Column("Table")]
        public string Table { get; set; } = string.Empty;

        [Column("Icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
