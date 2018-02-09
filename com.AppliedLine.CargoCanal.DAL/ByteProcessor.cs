using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class ByteProcessor
    {
        
        public byte[] TransformVarbinary(string sqlVarbinaryString)
        {
            List<byte> byteList = new List<byte>();

            string hexPart = sqlVarbinaryString.Substring(2);
            for (int i = 0; i < hexPart.Length / 2; i++)
            {
                string hexNumber = hexPart.Substring(i * 2, 2);
                byteList.Add((byte)Convert.ToInt32(hexNumber, 16));
            }

            return byteList.ToArray();
        }
    }
}
