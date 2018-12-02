using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.Declarations;

namespace ComRpc
{
	class cs_generator
	{
		private string AdjType(String type)
		{
			string adj_type;
			switch (type)
			{
				case "char*":
					adj_type = "byte []";
					break;
				case "int8_t":
					adj_type = "sbyte";
					break;
				case "int16_t":
					adj_type = "Int16";
					break;
				case "int32_t":
					adj_type = "Int32";
					break;
				case "uint8_t":
					adj_type = "byte";
					break;
				case "uint16_t":
					adj_type = "UInt16";
					break;
				case "uint32_t":
					adj_type = "UInt32";
					break;
				default:
					adj_type = type;
					break;

			}
			return adj_type;
		}

		public string Generate(DeclarationList decls)
		{
			StringBuilder code = new StringBuilder();
			

			code.Append(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Text;
using System.IO;

namespace ComRpc
{
	public class RemoteObject
	{
        private Stream stream;

        public RemoteObject(Stream stream)
        {
            this.stream = stream;
        }

        private byte[] ReadLineBytes()
        {
            List<byte> buff = new List<byte>();

            byte b = (byte)stream.ReadByte();
            while ((b == 13 || b == 10))
            {
                b = (byte)stream.ReadByte();
            }
            while ((b != 13 && b != 10 ))
            {
                buff.Add(b);
                b = (byte)stream.ReadByte();
            }
            return buff.ToArray();
        }
"
            );

			// Create function  code
			for (int i = 0; i < decls.Count; i++)
			{
				StringBuilder pars = new StringBuilder();
				Declaration d = decls[i];

				code.AppendFormat("\n\t\tpublic {0} {1}( ", AdjType(d.ProcType), d.ProcName);

				for (int j = 0; j < d.Parameters.Count; j++)
				{
					Parameter p = d.Parameters[j];

					pars.AppendFormat(String.Format(" {0} {1},", AdjType(p.Type), p.Name));

				}
				code.Append(pars.ToString().TrimEnd(new char [] {',',' '}));
				code.Append(
@" ) {
            List<byte> buff = new List<byte>();
"
                );

				code.AppendFormat(
@"
            buff.AddRange(ASCIIEncoding.ASCII.GetBytes("":{0},""));
"
                , i);

				for (int j = 0; j < d.Parameters.Count; j++)
				{
					Parameter p = d.Parameters[j];
                    if ("char*" == p.Type)
					{
                        code.AppendFormat(
@"            buff.AddRange(EscUtil.EscBytes({0}));
"
                        , p.Name);
					}
					else
					{
						code.AppendFormat(
@"                buff.AddRange(ASCIIEncoding.ASCII.GetBytes({0}.ToString()));
"
                        , p.Name);
					}
                    if(d.Parameters.Count-j>2){
                        code.Append(
@"                buff.AddRange(ASCIIEncoding.ASCII.GetBytes("",""));
"
                        );
                    }
				}

				code.AppendFormat(
@"			byte [] ret;
			try 
			{{
                buff.Add((byte)'\n');
                byte [] ob=buff.ToArray();
				stream.Write(ob,0,ob.Length);
				String resp=Encoding.Default.GetString( ReadLineBytes());//Just echo of the sent command
				ret=ReadLineBytes();
			}} 
			catch (Exception ex)
			{{
				throw new Exception(""DIYRpc: communication error in method '{0}'."");
			}} 
			if (ret[0] != (byte)':')
			{{
				throw new Exception(""DIYRpc: invalid RPC response in method '{0}'."");
			}}
			if(ret[1]!=(byte)'0'){{
				throw new Exception(""DIYRpc: RPC call of method '{0}' returned an error."");"
                , d.ProcName);              
				if (d.ProcType == "void")
				{
					code.Append("\n\t\t\t}\n");
				}
				else 
				{
					code.Append(
@"
			} else {
"
);
					if (d.ProcType == "char*"){
                        code.AppendFormat(
@"              
                ret=ret.SkipWhile(b => b != ',').Skip(1).ToArray();
                return EscUtil.UnEscBytes(ret);", d.ProcType);
					}
					else
					{
                        code.AppendFormat(
@"
                string rets=ASCIIEncoding.ASCII.GetString(ret.SkipWhile(b => b != ',').Skip(1).ToArray());
                return {0}.Parse(rets);", AdjType(d.ProcType));
					}
				code.Append(
@"
			}
"
					);
				}

				code.Append("\t\t}\n");

			}
			code.Append("\t}\n}\n");
			return code.ToString();
		}
	}
}
