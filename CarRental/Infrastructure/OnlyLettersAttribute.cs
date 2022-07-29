using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CarRental.Infrastructure
{
    public class OnlyLettersAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var regexItem = new Regex("^[a-zA-ZĄąĆćĘęŁłŃńÓóŚśŹźŻż]*$");
                return regexItem.IsMatch((string)value);
            }
            return false;
        }
    }
}