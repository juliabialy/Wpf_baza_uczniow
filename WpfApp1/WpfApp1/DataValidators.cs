using System;
using System.Linq;
using System.Text;
namespace WpfApp1
{
    public static class DataValidator
    {

        public static bool IsValidPesel(string pesel)
        {
            if (string.IsNullOrEmpty(pesel) || pesel.Length != 11 || !pesel.All(char.IsDigit))
                return false;

            // suma kontrolna
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (pesel[i] - '0') * weights[i];

            if ((10 - sum % 10) % 10 != (pesel[10] - '0'))
                return false;


            // data
            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));

            if (month >= 80) { year += 1800; month -= 80; }
            else if (month >= 60) { year += 2200; month -= 60; }
            else if (month >= 40) { year += 2100; month -= 40; }
            else if (month >= 20) { year += 2000; month -= 20; }
            else { year += 1900; }

            return birthDate.Value.Date == new DateTime(year, month, day);

        }

        //Phone Number
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            string cleanedNumber = phoneNumber.Replace(" ", "")
                                           .Replace("-", "")
                                           .Replace("(", "")
                                           .Replace(")", "");

            if (cleanedNumber.StartsWith("+"))
            {
                return cleanedNumber;
            }

            if (cleanedNumber.StartsWith("48") && cleanedNumber.Length == 11)
            {
                return "+" + cleanedNumber;
            }

            if (cleanedNumber.Length == 9)
            {
                return "+48" + cleanedNumber;
            }

            return phoneNumber;
        }
    }
}