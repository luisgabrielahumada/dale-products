using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool ToBool(this string value)
        {
            bool ret;
            bool.TryParse(value, out ret);
            return ret;
        }
        
        public static int ToInt(this string value)
        {
            int ret;
            int.TryParse(value, out ret);
            return ret;
        }
        public static int ToInt(this int? value)
        {
            return value.IsNull(0).Value;
        }

        public static bool IsNullOrEmpty(this string item)
        {
            return string.IsNullOrEmpty(item);
        }
        public static string IsNullOrEmpty(this string item, string result)
        {
            return string.IsNullOrEmpty(item) ? result : item;
        }
        public static bool IsNullOrWhiteSpace(this string item)
        {
            return string.IsNullOrWhiteSpace(item);
        }
        public static string IsNullOrWhiteSpace(this string item, string result)
        {
            return string.IsNullOrWhiteSpace(item) ? result : item;
        }

        public static bool IsNotNullOrEmpty(this string item)
        {
            return !string.IsNullOrEmpty(item);
        }
        public static string IsNotNullOrEmpty(this string item, string result)
        {
            return !string.IsNullOrEmpty(item) ? result : item;
        }
        public static bool IsNotNullOrWhiteSpace(this string item)
        {
            return !string.IsNullOrWhiteSpace(item);
        }
        public static string IsNotNullOrWhiteSpace(this string item, string result)
        {
            return !string.IsNullOrWhiteSpace(item) ? result : item;
        }

        public static string EncodeBase64(this string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string DecodeBase64(this string base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveSpecialCharacters(this string str) {
            var sb = new StringBuilder();
            foreach (var c in str) {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string ReplaceFields(this string value, KeyValuePair<string, string>[] fields)
        {
            foreach (var field in fields)
                value = value.Replace("[" + field.Key + "]", field.Value);

            return value;
        }

        public static bool IsOnlyNumbers(this string str)
        {
            var valid = true;
            foreach (var c in str) {
                if (!(c >= '0' && c <= '9'))
                    valid = false;
            }
            return valid;
        }
        
        public static bool IsOnlyLetters(this string str)
        {
            var regex = new Regex(@"^[\p{L} ]+$");
            return regex.IsMatch(str);
        }

        public static bool IsOnlyLettersAndNumbers(this string str)
        {
            var valid = true;
            foreach (var c in str) {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                    valid = false;
            }
            return valid;
        }

        public static bool IsEmail(this string str)
        {
            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            return regex.IsMatch(str);
        }

        public static DateTime DecryptDateTimeExact(this string value, string format)
        {
            return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }
        public static string SetFirstCharUpperCase(this string value)
        {
            return Regex.Replace(value, "^[a-z]", m => m.Value.ToUpper());
        }
    }
}