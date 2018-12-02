using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.Declarations;

namespace ComRpc
{
	class cpp_generator
	{
		public string Generate(DeclarationList decls)
		{
			StringBuilder code = new StringBuilder();
			StringBuilder externs = new StringBuilder();

			for (int i = 0; i < decls.Count; i++)
			{
				Declaration d = decls[i];

				externs.Append(String.Format("extern {0} {1}(", d.ProcType, d.ProcName));
				if (d.Parameters.Count > 0)
				{
					externs.Append(String.Format("{0} {1}", d.Parameters[0].Type, d.Parameters[0].Name));
					for (int j = 1; j < d.Parameters.Count; j++)
					{
						externs.Append(String.Format(",{0} {1}", d.Parameters[j].Type, d.Parameters[j].Name));
					}
				}
				externs.AppendLine(");");
			}

			code.Append(
@"#include <Arduino.h>
#include ""ComRpc.h""
#if defined(ESP8266)
#include ""dtostrg.h""
#endif 

String response; // Using WString class for a clever static buffer that can (hopefully) grow only.

"
);
			code.Append(externs);
			code.Append(
@"
const char* rpc_proc_line( char *line ){
	
	if(!response.reserve(RET_BUFF_SIZE)){ //To prevent heap fragmentation we should reserve large enough buffer on the first call. 
		response=F("":1,Out of memory!"");
		return response.c_str();
	} 
	char *pc=strtok(line+1,"","");
	if(NULL==pc){
		response=F("":1, func_id missing"");
		return response.c_str();
	}
	int id=atoi(pc);

	switch (id){

"
			);

			// Create function selection code
			for (int i = 0; i < decls.Count; i++)
			{
				Declaration d = decls[i];

				code.AppendFormat("\t\tcase {0}:\t// {1} {2}(...) \n\t\t{{\n", i, d.ProcType, d.ProcName);
				string parameters = "";
				for (int j = 0; j < d.Parameters.Count; j++)
				{
					string comma = (j != 0) ? "," : "";
					Parameter p = d.Parameters[j];
					string convf="";

					parameters = parameters + p.Name + ", ";
					code.AppendFormat("\t\t\t{0} {1};\n",p.Type,p.Name);
					switch (p.Type)
					{
						case "double":
						case "float":
							convf="atof( pc )";
							break;
						case "int8_t":
						case "int16_t":
						case "uint8_t":
						case "uint16_t":
							convf = "atoi( pc )";
							break;
						case "int32_t":
                            convf = "atol( pc )";
                            break;
                        case "uint32_t":
                            convf = "atouint32_t( pc )";
							break;
						case "char*":
                            convf = "unescapeString(pc)";
							break;
					}
					code.AppendFormat(
@"			pc=strtok(NULL,"","");
			if(NULL!=pc)
				{0} = {1};
			else {{
				response=F("":1, param: '{0}' missing!"");
				return response.c_str();
			}}
"
					,  p.Name, convf); 
				}
				char[] charsToTrim ={',',' '};
				parameters=parameters.TrimEnd(charsToTrim);
				if (d.ProcType == "void")
				{
					code.AppendFormat(
@"            {0}({1});
			response = F("":0,"");
"
					, d.ProcName, parameters);
				}
				else
				{
					if ("char*" == d.ProcType)
					{
						code.AppendFormat(
@"
			char* ret_{1} = {1}({2});
			response = F("":0,"");
			if(response.reserve(response.length()+escapeString(ret_{1},NULL)+1)){{
				escapeString(ret_{1}, (char*)response.c_str()+response.length());
			}} else {{
				response = F("":1,Out of memory during call to '{1}'"");
			}}"
                        , d.ProcType, d.ProcName, parameters);
					}
					else if ("double" == d.ProcType || "float" == d.ProcType)
					{
						code.AppendFormat(
@"            response = F("":0,"");
			dtostre({1}( {2} ),(char*)response.c_str()+response.length(),DBL_PREC,0);"
						, d.ProcType, d.ProcName, parameters);
					}
					else
					{
						code.AppendFormat(
@"            response = F("":0,"");
			response += String( {1}( {2} ) );", d.ProcType, d.ProcName, parameters);
					}

				}
				code.Append(
@"
			return response.c_str();
			break;
		}       
"
					);
				
			}
			code.Append(
@"		default:
			response = F("":1, func_id: '"");
			response += String(id);
			response += F(""' invalid!"");
			return response.c_str();
			break;
	}
}
"
			);
			return code.ToString();
		}
	}
}
