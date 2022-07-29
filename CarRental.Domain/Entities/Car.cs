using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Entities
{
    [Table("Cars")]
    public class Car
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Podaj markę samochodu")]
        [Display(Name = "Marka")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Podaj model samochodu")]
        [Display(Name = "Model")]
        public string Model { get; set; }
        public List<LicesnePlate> LicensePlates { get; set; }
        [Required(ErrorMessage = "Podaj ilość miejsc samochodu")]
        [Display(Name = "Ilość miejsc")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Wybierz typ samochodu")]
        [Display(Name = "Typ samochodu")]
        public string Type { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
