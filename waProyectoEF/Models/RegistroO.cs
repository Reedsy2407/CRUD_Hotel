using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace waProyectoEF.Models
{
    public class RegistroO
    {
        [DisplayName("CÓDIGO")]
        public int ide_alq { get; set; }

        [DisplayName("CLIENTE")]
        [Required(ErrorMessage = "Seleccione cliente")]
        public int ide_cli { get; set; }

        [DisplayName("HABITACIÓN")]
        [Required(ErrorMessage = "Seleccione habitación")]
        public int ide_hab { get; set; }

        [DisplayName("FECHA ENTRADA")]
        [Required(ErrorMessage = "Seleccione fecha de entrada")]
        public DateTime fen_alq { get; set; }

        [DisplayName("FECHA SALIDA")]
        [Required(ErrorMessage = "Seleccione fecha de salida ")]
        public DateTime fsa_alq { get; set; }
    }
}