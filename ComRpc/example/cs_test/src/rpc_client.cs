using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Text;

namespace ComRpc
{
	public class RemoteObject
	{
		private SerialPort sport;
		public RemoteObject(SerialPort sport)
		{
			this.sport = sport;
		}

        private byte[] ReadLineBytes()
        {
            List<byte> buff = new List<byte>();
            byte b=(byte) sport.ReadByte();
            while (b != 13 && b != 10)
            {
                buff.Add(b);
                b = (byte)sport.ReadByte();
            }
            return buff.ToArray();
        }

		public byte [] returnCString(  ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":0,"));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'returnCString'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'returnCString'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'returnCString' returned an error.");
			} else {
              
                ret=ret.SkipWhile(b => b != ',').Skip(1).ToArray();
                return EscUtil.UnEscBytes(ret);
			}
		}

		public byte [] testCString(  byte [] cstring ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":1,"));
            buff.AddRange(EscUtil.EscBytes(cstring));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testCString'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testCString'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testCString' returned an error.");
			} else {
              
                ret=ret.SkipWhile(b => b != ',').Skip(1).ToArray();
                return EscUtil.UnEscBytes(ret);
			}
		}

		public double testDouble(  double par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":2,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testDouble'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testDouble'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testDouble' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return double.Parse(rets);
			}
		}

		public float testFloat(  float par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":3,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testFloat'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testFloat'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testFloat' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return float.Parse(rets);
			}
		}

		public sbyte testChar(  sbyte cin ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":4,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(cin.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testChar'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testChar'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testChar' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return sbyte.Parse(rets);
			}
		}

		public Int16 testInt16(  Int16 par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":5,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testInt16'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testInt16'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testInt16' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return Int16.Parse(rets);
			}
		}

		public Int32 testInt32(  Int32 par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":6,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testInt32'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testInt32'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testInt32' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return Int32.Parse(rets);
			}
		}

		public byte testUChar(  byte cin ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":7,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(cin.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testUChar'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testUChar'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testUChar' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return byte.Parse(rets);
			}
		}

		public UInt16 testUInt16(  UInt16 par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":8,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testUInt16'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testUInt16'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testUInt16' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return UInt16.Parse(rets);
			}
		}

		public UInt32 testUInt32(  UInt32 par ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":9,"));
                buff.AddRange(ASCIIEncoding.ASCII.GetBytes(par.ToString()));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testUInt32'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testUInt32'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testUInt32' returned an error.");
			} else {

                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return UInt32.Parse(rets);
			}
		}

		public void testVoid(  ) {
            List<byte> buff = new List<byte>();

            buff.AddRange(ASCIIEncoding.ASCII.GetBytes(":10,"));
			byte [] ret;
			try 
			{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
                sport.DiscardInBuffer();
				sport.Write(ob,0,ob.Length);
				String resp=sport.ReadLine();//Just echo of the sent command
				ret=ReadLineBytes();
			} 
			catch (Exception ex)
			{
				throw new Exception("DIYRpc: communication error in method 'testVoid'.");
			} 
			if (ret[0] != (byte)':')
			{
				throw new Exception("DIYRpc: invalid RPC response in method 'testVoid'.");
			}
			if(ret[1]!=(byte)'0'){
				throw new Exception("DIYRpc: RPC call of method 'testVoid' returned an error.");
			}
		}
	}
}

