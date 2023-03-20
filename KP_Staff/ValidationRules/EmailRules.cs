using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace KP.ValidationRules
{
    public class EmailRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = string.Empty;
            if (value != null)
            {
                email = value.ToString();
            }
            else
            {
                return new ValidationResult(false, " Адрес электронной почты не задан! ");
            }

            if (email.Contains("@") && email.Contains("."))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Адрес электронной почты должен содержать символы @ и(.) точки \n Шаблон адреса: adres@mymail.com");
            }

            throw new NotImplementedException();
        }
    }

    public class TelephoneRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phone = string.Empty;
            if (value != null)
            {
                phone = value.ToString();
            }
            else
            {
                return new ValidationResult(false, "Телефон не задан! ");
            }

            if (phone.StartsWith("+7") && phone.Length == 12 && (phone.Contains("1") || phone.Contains("2") || phone.Contains("3") || 
                phone.Contains("4") || phone.Contains("5") || phone.Contains("6") || phone.Contains("7") || phone.Contains("8") || 
                phone.Contains("9") || phone.Contains("0") || phone.Contains("+")))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Номер телефон должен содержать 12 цифр и/или символ + и начинаться на +7\n Шаблон номера телефона: +79876543210");
            }

            throw new NotImplementedException();
        }
    }


}
