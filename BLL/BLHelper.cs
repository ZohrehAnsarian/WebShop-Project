using System;

namespace BLL
{
    public static class BLHelper
    {
        public static int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string GeneratePassword(int passwordLength)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyz";
            string allowedUpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string allowedNoneDigitChars = "!@#$%&*_-+=";
            string allowedDigitChars = "0123456789";
            char[] chars = new char[passwordLength];

            Random random = new Random(Environment.TickCount);

            int i = 0;

            for (i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            int[] randomIndexs = new int[3];

            randomIndexs = GenerateUniqueRandomNumbers(3, 0, 5);

            chars[randomIndexs[0]] = allowedUpperChars[random.Next(0, allowedUpperChars.Length)];
            chars[randomIndexs[1]] = allowedNoneDigitChars[random.Next(0, allowedNoneDigitChars.Length)];
            chars[randomIndexs[2]] = allowedDigitChars[random.Next(0, allowedDigitChars.Length)];

            return new string(chars);
        }
        public static int[] GenerateUniqueRandomNumbers(int randomCount, int min, int max)
        {

            Random random = new Random(Environment.TickCount);

            int[] randomArray = new int[randomCount];

            int tempMin = 0;
            int tempMax = 0;
            int numbCount = max - min + 1;
            int minMaxIndex = 0;
            int partCount = numbCount / randomCount;

            double divResult = (double)numbCount / randomCount;

            if (divResult - Math.Truncate(divResult) > 0)
            {
                partCount++;
            }

            for (int i = 0; i < numbCount; i += partCount)
            {
                tempMin = min;
                tempMax = min + partCount - 1;
                min = tempMax + 1;
                minMaxIndex++;

                if (minMaxIndex == randomCount)
                {
                    tempMax = max;
                }

                randomArray[minMaxIndex - 1] = random.Next(tempMin, tempMax);
            }

            return randomArray;
        }

        public static int SendSMS(string receptor, string message)
        {
            //var sender = "100065995";

            //var api = new Kavenegar.KavenegarApi("4738656B564552554A57726B6C61523537476B7733556430375736765256415A");

            //var sender = "100065995";
            //var api = new Kavenegar.KavenegarApi("666234496E7478745430446539574F6B776E446F5A694D7A7A70316679443832");

            var sender = "100065995";
            //var api = new Kavenegar.KavenegarApi("4C69472B665677527241736C6B2F65382B444B4733667655553471384D483343");
            var api = "";

            //return api.Send(sender, receptor, message).Status;
            return 1;
        }
        public static string ReversDateString(string DateString)
        {
            string[] TempDateString = DateString.Split('/');
            return TempDateString[2] + "/" + TempDateString[1] + "/" + TempDateString[0];
        }

    }
}
