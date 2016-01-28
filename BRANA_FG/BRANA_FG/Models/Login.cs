using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Logins")]
    public class Login
    {
        public int id { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public int idUser { get; set; }
    }
}