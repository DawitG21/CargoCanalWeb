using System.Security.Cryptography;
using System.Text;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class Encrypter
    {
        public string ComputeMD5Hash(string data)
        {
            MD5 hash = MD5.Create();
            byte[] hashData = hash.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                sb.Append(hashData[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
