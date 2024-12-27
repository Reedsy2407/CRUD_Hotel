using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace waProyectoEF.Models
{
    public class Registro
    {
        [DisplayName("CÓDIGO")]
        public int ide_alq { get; set; }

        [DisplayName("NOMBRE CLIENTE")]
        public String nom_cli { get; set; }

        [DisplayName("DNI")]
        public String dni_cli { get; set; }

        [DisplayName("NÚMERO HABITACIÓN")]
        public String num_hab { get; set; }

        [DisplayName("PRECIO")]
        public double pre_hab { get; set; }

        [DisplayName("FECHA ENTRADA")]
        public DateTime fen_alq { get; set; }

        [DisplayName("FECHA SALIDA")]
        public DateTime fsa_alq { get; set; }
    }
}