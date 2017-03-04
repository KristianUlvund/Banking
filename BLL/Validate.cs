using System;
using System.Text.RegularExpressions;
using TheBank.Model;

namespace BLL
{
    public static class Validate
    {
        public static bool validatePID(string pID)
        {
            Regex regex = new Regex("^[0-9]{11}$");
            return (pID != null) && regex.IsMatch(pID);
        }
        public static bool validateDate(string date)
        {
            Regex regex = new Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}$");
            return (date != null) && regex.IsMatch(date);
        }
        public static bool validateName(string name)
        {
            Regex regex = new Regex("^[a-zA-ZæøåÆØÅ.- ]{0,30}$");
            return (name != null) && regex.IsMatch(name);
        }
        public static bool validatePassword(string password)
        {
            Regex regex = new Regex("^[0-9a-zA-Z.!?+-]{0,30}$");
            return (password != null) && regex.IsMatch(password);
        }
        public static bool validateAccountNmbr(string accountNmbr)
        {
            Regex regex = new Regex("^[0-9.]{13}$");
            return (accountNmbr != null) && regex.IsMatch(accountNmbr);
        }

        public static bool validateMonthYear(int month, int year)
        {
            var m = month > 0 && month <= 12;
            var y = year > 1900 && year <= DateTime.Now.Year;
            return m && y;
        }

        public static bool validateMessage(string message)
        {
            Regex regex = new Regex("^[0-9a-zA-ZæøåÆØÅ.,/ -]{0,50}$");
            return (message != null) && regex.IsMatch(message);
        }
        public static bool validateTransaction(TransactionPresentation tp)
        {
            var a = tp.ID >= 0;
            var b = validateDate(tp.date);
            var c = validateMessage(tp.message);
            var d = tp.amount > 0;
            var e = validateAccountNmbr(tp.fromAccount);
            var f = validateAccountNmbr(tp.toAccount);

            return a && b && c && d && e && f;
        }

    }
}
