/*
    Based on Vitaliy Kleban tiny-rpc https://bitbucket.org/sources/tiny-rpc

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
using Parser.Declarations;


namespace ComRpc
{
    class Program
    {
        static bool CheckReturnType(string type)
        {
            string[] types = { "void", "int8_t", "int16_t", "int32_t", "uint8_t", "uint16_t", "uint32_t", "double", "float", "char*", "char" };
            foreach (string s in types)
            {
                if (s == type) return true;
            }
            return false;
        }

        static bool CheckParameterType(string type)
        {
            string[] types = { "void", "int8_t", "int16_t", "int32_t","uint8_t", "uint16_t", "uint32_t", "double", "float", "char", "char*" };
            foreach (string s in types)
            {
                if (s == type) return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: DIYRpc.exe -[server|cs_client] input_file_name");
                return;
            }

            DeclarationList decls = Parser.Declarations.Extractor.Extract(args[1]);
            foreach (Declaration declaration in decls)
            {
                if (!CheckReturnType(declaration.ProcType))
                {
                    Console.Error.WriteLine(String.Format("Unsupported return type {0} for function {1}", declaration.ProcType, declaration.ProcName));
                    return;
                }
                foreach (var p in declaration.Parameters)
                {
                    if (!CheckParameterType(p.Type))
                    {
                        Console.Error.WriteLine(String.Format("Unsupported parameter type {0} in function {1}", p.Type, declaration.ProcName));
                        return;
                    }
                }
            }

            if (args[0] == "-server" )
            {
                var c = new cpp_generator();
                string res = c.Generate(decls);
                Console.WriteLine(c.Generate(decls));
            }
           else if (args[0] == "-cs_client")
            {
                var cs = new cs_generator();
                string res = cs.Generate(decls);
                Console.WriteLine(cs.Generate(decls));
            }
//            Console.ReadKey();
        }
    }
}
