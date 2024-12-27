using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace waProyectoEF.Models
{
    public class Cliente
    {
        [DisplayName("CÓDIGO")]
        public int ide_cli { get; set; }

        [DisplayName("NOMBRE CLIENTE")]
        public String nom_cli { get; set; }

        [DisplayName("EMAIL")]
        public String email { get; set; }

        [DisplayName("MÉTODO DE PAGO")]
        public String nom_met { get; set; }

        [DisplayName("DNI CLIENTE")]
        public String dni_cli { get; set; }
    }
}