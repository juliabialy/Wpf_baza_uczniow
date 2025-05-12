using System;
using System.Linq;

namespace WpfApp1
{
    public static class DataValidator
    {
        public static bool IsValidPesel(string pesel, DateTime? birthDate)
        {
            if (string.IsNullOrEmpty(pesel) || pesel.Length != 11 || !pesel.All(char.IsDigit))
                return false;

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (pesel[i] - '0') * weights[i];

            int controlDigit = (10 - sum % 10) % 10;
            if (controlDigit != (pesel[10] - '0'))
                return false;

            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));

            if (month >= 80) { year += 1800; month -= 80; }
            else if (month >= 60) { year += 2200; month -= 60; }
            else if (month >= 40) { year += 2100; month -= 40; }
            else if (month >= 20) { year += 2000; month -= 20; }
            else { year += 1900; }

            DateTime parsedDate;
            try
            {
                parsedDate = new DateTime(year, month, day);
            }
            catch
            {
                return false;
            }

            if (birthDate == null)
                return false;

            return parsedDate.Date == birthDate.Value.Date;
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            string cleanedNumber = new string(phoneNumber
                .Where(char.IsDigit)
                .ToArray());

            if (cleanedNumber.StartsWith("48") && cleanedNumber.Length == 11)
                return "+48" + cleanedNumber.Substring(2);
            if (cleanedNumber.Length == 9)
                return "+48" + cleanedNumber;
            if (cleanedNumber.StartsWith("00") && cleanedNumber.Length > 4)
                return "+" + cleanedNumber.Substring(2);

            return "+" + cleanedNumber;
        }

        public static string FormatText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Trim().ToLower();
            var separators = new[] { ' ', '-' };
            var parts = input.Split(separators);

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i][1..];
            }

            var result = input;

            foreach (var sep1 in separators)
            {
                if (input.Contains(sep1))
                    result = string.Join(sep1.ToString(), input.Split(sep1).Select(FormatText));
            }

            if (!input.Contains('-'))
                result = string.Join(" ", parts);

            return result;
        }
    }
}
