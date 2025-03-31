using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Common.Implement
{
    /// <summary>
    /// class ReturnResult
    /// </summary>
    public class EncryptHelper //: IEncryptHelper
    {
        /// <summary>Encrypt value use SHA256</summary>
        /// <param name="value">The value.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string SHAEncrypt(string value, string salt)
        {
            // 使用 SHA256 雜湊運算加密(不可逆)
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(salt + value); //將密碼鹽及要加密的值組合
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder encryptValue = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                encryptValue.Append(hash[i].ToString("x2"));  //轉換成十六進位制，每次都是兩位數
            }
            string result = encryptValue.ToString(); // 雜湊運算後密碼

            return result;
        }
    }
}
