/*
	Copyright (C) 2018 T. Mertelj
	
	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComRpc;
using System.IO.Ports;


namespace ComRpc_example
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] s=new byte[254];

            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: DIYRpc-example.exe com[number]");
                return;
            }


            for (int i = 0; i < 254; i++)
            {
                s[i] = Convert.ToByte(i+1);
            }


            SerialPort sp = new SerialPort(args[0], 115200, Parity.None, 8, StopBits.One);
            sp.Handshake = Handshake.None;
            try
            {
                sp.Open();
            }
            catch (Exception ex )
            {
                Console.WriteLine(String.Format("Cannot open serial port '{0}'.", args[0]));
                Console.WriteLine(ex.ToString());
                return;
            }

            RemoteObject ro = new RemoteObject(sp.BaseStream);

            string rs = System.Text.Encoding.ASCII.GetString(ro.returnCString());
            Console.WriteLine("In: void: ");
            Console.WriteLine("Out: byte[]: '" + rs + "'");
            Console.WriteLine();
            
            Console.WriteLine("In: byte[]: '" + System.Text.Encoding.Default.GetString(s) + "'");
            byte[] so=ro.testCString(s);
            Console.WriteLine("Out: byte[]: '" + System.Text.Encoding.Default.GetString(so) + "'");
            Console.WriteLine();

            double d = 1.23456789123456789;
            Console.WriteLine("double: '" + d.ToString() + " -> " + ro.testDouble(d).ToString() + "'");
            float f = 1.23456789123456789f;
            Console.WriteLine("float: '" + f.ToString() + " -> " + ro.testFloat(f).ToString() + "'");
            Random rnd = new Random();
            sbyte sb = (sbyte)rnd.Next(-sbyte.MaxValue, 0);
            Console.WriteLine("sbyte: '" + sb.ToString() + " -> " + ro.testChar(sb).ToString() + "'");
            byte b = (byte)rnd.Next(byte.MaxValue);
            Console.WriteLine("byte: '" + b.ToString() + " -> " + ro.testUChar(b).ToString() + "'");
            Int16 i16 = (Int16)rnd.Next(-Int16.MaxValue,-byte.MaxValue);
            Console.WriteLine("Int16: '" + i16.ToString() + " -> " + ro.testInt16(i16).ToString() + "'");
            UInt16 ui16 = (UInt16)rnd.Next(byte.MaxValue, UInt16.MaxValue);
            Console.WriteLine("UInt16: '" + ui16.ToString() + " -> " + ro.testUInt16(ui16).ToString() + "'");
            Int32 i32 = (Int32)rnd.Next(-Int32.MaxValue, -Int16.MaxValue);
            Console.WriteLine("Int32: '" + i32.ToString() + " -> " + ro.testInt32(i32).ToString() + "'");
            UInt32 ui32 = (UInt32)(UInt16.MaxValue * rnd.Next(0, UInt16.MaxValue));
            Console.WriteLine("UInt32: '" + ui32.ToString() + " -> " + ro.testUInt32(ui32).ToString() + "'");
            Console.Write("Press a key to exit.");
            Console.ReadKey();
        }
    }
}
