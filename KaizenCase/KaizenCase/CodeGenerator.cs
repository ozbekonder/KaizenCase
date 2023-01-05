using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaizenCase
{
    public class CodeGenerator
    {
        string chars = "ACDEFGHKLMNPRTXYZ234579";
        Random random = new Random();

        //Üretilecek kodun 1,3,5,7,... karakterleri random belirlenir. Belirlenen bu random karakterleriden kod üretilir.
        public string generate_codes()
        {
            string code = "";
            string[] randomChars = new string[4];
            for (int i = 0; i < 4; i++)
            {
                randomChars[i] = chars.Substring(random.Next(chars.Length - 1), 1);
            }
            code = CodeCreateAlgorithm(randomChars);
            if (check_code(code))
                return code;
            else
                return null;
        }

        //Kontrol edilecek kodun 1,3,5,7,... karakterleri tespit edilir. Bu karakterlerden yeni bir kod üretilir üretilen kod ile gelen kod aynı ise kod doğrudur.
        public bool check_code(string code)
        {
            bool IsValid = false;
            string[] randomChars = new string[4];
            for (int i = 0; i < 4; i++)
            {
                randomChars[i] = code.Substring((2 * i + 1) - 1, 1);
            }
            if (code == CodeCreateAlgorithm(randomChars))
                IsValid = true;
            return IsValid;
        }

        // Kod üretme algoritması, üretilecek kodun 1,3,5,7,.. karakterleri random seçilir.
        // Koda ait 2,4,6,8,... karakterlerinide -1 ve +1 sırasındaki karakterlerin karaktersetindeki sırasının toplamına denk gelen karakter seçilerek yapılır.
        // Örn Kodun 2. karakter 1. ve 3. karakterlerin karakter setindeki sırasının toplamına denk gelen karakter seçilir.
        // 1. karakter A, 3. karakter C olursa 2. karakter D olur. Kod ADC olur.
        private string CodeCreateAlgorithm(string[] randomChars)
        {
            string code = null;
            int newCharIndex = 0;
            if (randomChars.Length > 1)
            {
                code = randomChars[0];
                for (int i = 1; i < randomChars.Length; i++)
                {
                    newCharIndex = (chars.IndexOf(randomChars[i - 1]) + 1 + chars.IndexOf(randomChars[i]) + 1) % (chars.Length);
                    code = code + chars.Substring(newCharIndex - 1, 1) + randomChars[i];
                }
                if (randomChars.Length % 2 == 0)
                {
                    newCharIndex = (chars.IndexOf(randomChars[randomChars.Length - 1]) + 1 + chars.IndexOf(randomChars[0]) + 1) % (chars.Length);
                    code = code + chars.Substring(newCharIndex - 1, 1);
                }
            }
            return code;
        }
    }
}
