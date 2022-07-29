using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class FilterModel
    {

        public IEnumerable<CheckBoxField> CarTypes { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IEnumerable<CheckBoxField> CarBrands { get; set; }
        [UIHint("Enum")]
        public SortOprions SortOprion { get; set; }

    }
    public enum SortOprions
    {
        popularity,
        priceUp,
        priceDown
    }
    public class CheckBoxField
    {
        public string Name { get; set; }
        public bool CheckBoxValue { get; set; }

    }
}
