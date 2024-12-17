using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CamionesDTO
    {
        //DTO => Data Transfer Object 
        //Decoradores de Código 
        //sirven para dar caracterizticas y definiciones específicas a cada campos y/o elemento de  una clase 
        [Key]//Data Annotation
        public int ID_Camion { get; set; }

        [Required]//Data Annotation
        [Display (Name = "Matricula")]//DataHelper
        public string Matricula { get; set; }
        [Required]//Data Annotation
        [Display(Name = "Tipo_Camion")]//DataHelper
        public string Tipo_Camion { get; set; }
        [Required]//Data Annotation
        [Display(Name = "Marca")]//DataHelper
        public string Marca { get; set; }

        [Required]//Data Annotation
        [Display(Name = "Modelo")]//DataHelper

        public string Modelo { get; set; }
         [Required]//Data Annotation
        [Display(Name = "Capacidad")]//DataHelper
        public int Capacidad { get; set; }
        [Required]//Data Annotation
        [Display(Name = "Kilometraje")]//DataHelper
        public double Kilometraje { get; set; }
     
        [DataType(DataType.ImageUrl)]//DataHelper
        public string UrlFoto { get; set; }

        [Required]//Data Annotation
        [Display(Name = "Disponibilidad")]//DataHelper
        public bool Disponibilidad { get; set; }
    
}
}
