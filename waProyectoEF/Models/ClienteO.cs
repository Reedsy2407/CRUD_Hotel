using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace waProyectoEF.Models
{
    public class ClienteO
    {
        [DisplayName("CÓDIGO")]
        public int ide_cli { get; set; }

        [DisplayName("NOMBRE CLIENTE")]
        [Required(ErrorMessage = "Ingrese nombre del cliente")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El nombre del cliente debe tener entre 1 y 20 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]*$", ErrorMessage = "El nombre del cliente solo puede contener letras")]
        public string nom_cli { get; set; }

        [DisplayName("APELLIDO CLIENTE")]
        [Required(ErrorMessage = "Ingrese apellido del cliente")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El apellido del cliente debe tener entre 1 y 20 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]*$", ErrorMessage = "El nombre del cliente solo puede contener letras")]
        public string ape_cli { get; set; }

        [DisplayName("EMAIL")]
        [Required(ErrorMessage = "Ingrese email del cliente")]
        [EmailAddress]
        public String email { get; set; }

        [DisplayName("MÉTODO DE PAGO")]
        [Required(ErrorMessage = "Seleccione método de pago")]
        public int ide_met { get; set; }

        [DisplayName("DNI CLIENTE")]
        [Required(ErrorMessage = "Ingrese dni del cliente")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI del cliente solo puede tener 8 dígitos")]
        public string dni_cli { get; set; }
    }
}