using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Shared.Utilities
{
    public class CodeGenerator
    {
        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateUniqueCode(int exactLength)
        {
            if (exactLength != 6)
                throw new ArgumentException("The code length must be exactly 6.", nameof(exactLength));

            var random = new Random();
            var randomPart = new char[exactLength];

            for (int i = 0; i < exactLength; i++)
            {
                randomPart[i] = AllowedChars[random.Next(AllowedChars.Length)];
            }

            return new string(randomPart);
        }

        public static int GenerateOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[2];
                int randomNumber;
                string otpString;
                do
                {
                    rng.GetBytes(randomBytes);
                    randomNumber = BitConverter.ToUInt16(randomBytes, 0) % 10000;
                    otpString = randomNumber.ToString("D4");
                } while (otpString == "0000");

                int otp;
                if (!int.TryParse(otpString, out otp))
                {
                    throw new Exception("Failed to parse OTP to integer");
                }
                return otp;
            }
        }
    }
}