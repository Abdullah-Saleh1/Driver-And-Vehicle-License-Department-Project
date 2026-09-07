using System;
using System.Text.RegularExpressions;

namespace DVLD_PresentationLayer.Global_Classes
{
    static public class clsValidation
    {
        static public bool ValidateEmail(string Email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            return regex.IsMatch(Email);
        }
    }
}
