using System.Collections.Generic;
using System.Linq;

namespace Services.Helper
{
    public static class SoChungTuHelper
    {
        public static string IncreaseSoChungTu(this string soChungTu)
        {
            if (soChungTu.Any(char.IsDigit))
            {
                int length = soChungTu.Length;
                bool hasNumber = false;
                int lastIndexOfNumber = -1;

                var stack = new Stack<char>();
                for (int i = length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(soChungTu[i]))
                    {
                        if (lastIndexOfNumber == -1)
                        {
                            lastIndexOfNumber = i;
                        }
                        stack.Push(soChungTu[i]);
                        hasNumber = true;
                    }
                    else
                    {
                        if (hasNumber)
                        {
                            break;
                        }
                    }
                }

                string sNumber = new string(stack.ToArray());
                int lengthSNumber = sNumber.Length;
                System.Numerics.BigInteger number = System.Numerics.BigInteger.Parse(sNumber) + 1;
                if (System.Numerics.BigInteger.Parse(sNumber).ToString().Count() == sNumber.Count())
                {
                    sNumber = number.ToString();
                }
                else
                {
                    sNumber = number.ToString().PadLeft(sNumber.Length, '0');
                }

                string result = soChungTu.Remove(lastIndexOfNumber - (lengthSNumber - 1), lengthSNumber)
                    .Insert(lastIndexOfNumber - (lengthSNumber - 1), sNumber);
                return result;
            }

            return soChungTu + 1;
        }
    }
}
