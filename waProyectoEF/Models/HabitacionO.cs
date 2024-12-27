using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace waProyectoEF.Models
{
    public class HabitacionO
    {
        [DisplayName("CÓDIGO")]
        public int ide_hab { get; set; }

        [DisplayName("NÚMERO HABITACIÓN")]
        [Required(ErrorMessage = "Ingrese número de la habitación")]
        [RegularExpression(@"^(10[1-9]|11[0]|20[1-9]|21[0]|30[1-9]|31[0])$", ErrorMessage = "El campo debe ser un número válido entre 101-110, 201-210, o 301-310.")]
        public string num_hab { get; set; }

        [DisplayName("TIPO HABITACIÓN")]
        [Required(ErrorMessage = "Seleccione tipo de la habitación")]
        public int ide_tha { get; set; }

        [DisplayName("CAPACIDAD")]
        [Required(ErrorMessage = "Ingrese capacidad de la habitación")]
        [Range(1, 4, ErrorMessage = "La capacidad debe estar entre 1 y 4")]
        public int cap_hab { get; set; }

        [DisplayName("PRECIO")]
        [Required(ErrorMessage = "Ingrese precio de la habitación")]
        [Range(11.0, 20.0, ErrorMessage = "El precio mínimo es 11 y el máximo es 20")]
        public double pre_hab { get; set; }

        [DisplayName("ESTADO")]
        [Required(ErrorMessage = "Seleccione estado de la habitación")]
        public int ide_est { get; set; }
    }
}
