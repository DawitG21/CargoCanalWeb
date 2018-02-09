using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace com.AppliedLine.CargoCanal.Models
{
    class Encrypter
    {
        #region constructors
        public Encrypter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region public methods
        public string ComputeMD5Hash(String data)
        {
            MD5 hash = MD5.Create();
            byte[] hashData = hash.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                sBuilder.Append(hashData[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion
    }
}
