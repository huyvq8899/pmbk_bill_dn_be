using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Helper
{
    public class DataValidator
    {
        //Class này để validate email, số điện thoại, mã số thuế

        //Method kiểm tra mã số thuế hợp lệ
        public static bool CheckValidMaSoThue(string inputMaSoThue)
        {
            var mstPattern1 = @"^[0-9\-]{10}$";
            var mstPattern2 = @"^[0-9\-]{14}$";

            if (string.IsNullOrWhiteSpace(inputMaSoThue) == false)
            {
                bool isMatch1 = Regex.IsMatch(inputMaSoThue, mstPattern1,
                                    RegexOptions.IgnoreCase);

                bool isMatch2 = Regex.IsMatch(inputMaSoThue, mstPattern2,
                                    RegexOptions.IgnoreCase);

                if (isMatch1 == false && isMatch2 == false)
                {
                    return false;
                }
            }

            return true;
        }

        //Method kiểm tra số điện thoại hợp lệ
        public static bool CheckValidPhone(string inputPhones)
        {
            if (string.IsNullOrWhiteSpace(inputPhones))
            {
                return true;
            }

            var phonePattern = "^[0-9]{10}$";
            var phones = inputPhones.Split(';');
            var checkRepeat = GetRepeatLetterSpecific(";", inputPhones.Split(';'));
            if (string.IsNullOrWhiteSpace(checkRepeat) == false)
            {
                return false;
            }

            foreach (var phone in phones)
            {
                if (string.IsNullOrWhiteSpace(phone) == false)
                {
                    bool isMatch = Regex.IsMatch(phone, phonePattern,
                                     RegexOptions.IgnoreCase);

                    if (isMatch == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Method kiểm tra email hợp lệ
        public static bool CheckValidEmail(string inputEmails)
        {
            if (string.IsNullOrWhiteSpace(inputEmails))
            {
                return true;
            }

            var emailPattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
            var emails = inputEmails.Split(';');
            var checkRepeat = GetRepeatLetterSpecific(";", inputEmails.Split(';'));
            if (string.IsNullOrWhiteSpace(checkRepeat) == false)
            {
                return false;
            }

            foreach (var email in emails)
            {
                if (string.IsNullOrWhiteSpace(email) == false)
                {
                    bool isMatch = Regex.IsMatch(email, emailPattern,
                                     RegexOptions.IgnoreCase);

                    if (isMatch == false)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }


        private static string GetRepeatLetterSpecific(string separator, string[] array)
        {
            var firstIndex = -1;
            var result = "";
            for (var i = 0; i < array.Length; i++)
            {
                var element = array[i];
                if (element == separator)
                {
                    if (firstIndex == -1)
                    {
                        firstIndex = i;
                    }
                    else
                    {
                        if (i - 1 == firstIndex)
                        {
                            firstIndex = i;
                            result += element;
                        }
                    }
                }
            }

            return result;
        }
    }
}
