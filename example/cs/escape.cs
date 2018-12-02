using System;
using System.Text;
using System.Collections.Generic;
namespace ComRpc
{
    public sealed class EscUtil
    {
        public static byte [] EscBytes(byte [] bain){

            int len = bain.Length;
            List<byte> ret = new List<byte>((int)(bain.Length * 1.1));
            int i = 0;

            while (i<len)
            {
                switch (bain[i])
                {
			        case (byte)'\\':
                    case (byte)'\r':
                    case (byte)'\n':
                    case (byte)',':
                    case (byte)':':
                    case (byte)' ':
                        ret.Add((byte)'\\');
                        ret.AddRange(ASCIIEncoding.ASCII.GetBytes(bain[i++].ToString("X2")));
                        break;
                    default:
                        ret.Add(bain[i++]);
                        break;
                }
            }
            return ret.ToArray(); ;
        }
            
        public static byte[] UnEscBytes(byte[] bain)
        {
            int len = bain.Length;
            List<byte> ret = new List<byte>((int)(bain.Length));
            int i = 0;

            while (i < len)
            {
                switch (bain[i])
                {
                    case (byte)'\\':
                        i++;
                        string str=ASCIIEncoding.ASCII.GetString(bain,i,2);
                        ret.Add(Convert.ToByte(str, 16));
                        i += 2;
                        break;
                    default:
                        ret.Add(bain[i++]);
                        break;
                }
            }
            return ret.ToArray();
        }
 
    }
}