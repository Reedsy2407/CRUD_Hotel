using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace waProyectoEF.Models
{
    public class Habitacion
    {
        [DisplayName("CÓDIGO")]
        public int ide_hab { get; set; }

        [DisplayName("NÚMERO HABITACIÓN")]
        public String num_hab { get; set; }

        [DisplayName("TIPO HABITACIÓN")]
        public String nom_tha { get; set; }

        [DisplayName("CAPACIDAD")]
        public int cap_hab { get; set; }

        [DisplayName("PRECIO")]
        public double pre_hab { get; set; }

        [DisplayName("ESTADO")]
        public String nom_est { get; set; }
    }
}