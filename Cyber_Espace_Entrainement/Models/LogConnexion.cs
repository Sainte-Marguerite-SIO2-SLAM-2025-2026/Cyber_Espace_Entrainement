using System;
using System.Collections.Generic;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    public class LogConnexion
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public Utilisateurs User { get; set; }

        public DateTime derniereConnexion { get; set; }
    }

}
