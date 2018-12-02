using System;
using System.Collections.Generic;
using System.IO;

namespace Parser.Declarations
{
    /// <summary>
    /// This class extract function declarations from C source code
    /// </summary>
    public class Extractor
    {
        private Symbol sym;
        private Lexer lex = null;

        Declaration decl = new Declaration();
        DeclarationList decls = new DeclarationList();

        public static DeclarationList Extract(string filename)
        {
            string line = "";
            List<string> lines = new List<string>();
            StreamReader file = new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();

            Extractor p = new Extractor();

            return p.Parse(lines.ToArray());
        }

        /// <summary>
        /// Parse source code and return list of declarations
        /// </summary>
        /// <param name="source">Source code</param>
        /// <returns>List of declarations (based on List)</returns>
        public DeclarationList Parse(string[] source)
        {
            foreach (string s in source)
            {
                try
                {
                    decl = new Declaration();
                    Parse(s);
                    decls.Add(decl.DeepClone());
                }
                catch (Exception)
                {
                    //Console.Error.WriteLine(e.Message);
                    continue;
                }
            }

            

            return decls;
        }

        private void Parse(string s)
        {
            lex = new Lexer(s);
            GetSym();
            Def();
        }

        private void Def()
        {
            decl.ProcType = Typ();
            if (sym.Type == Symbols.ident)
            {
                decl.ProcName = sym.Id;
                GetSym();
                if (sym.Type == Symbols.lbracket)
                {
                    GetSym();
                    if (sym.Type != Symbols.rbracket)
                    {
                        string t = Typ();
                        string n = Pname();
                        decl.Parameters.Add(new Parameter(t, n));
                        while (sym.Type == Symbols.comma)
                        {
                            GetSym();
                            t = Typ();
                            n = Pname();
                            decl.Parameters.Add(new Parameter(t, n));
                        }
                    }

                    if (sym.Type == Symbols.rbracket)
                    {
                        GetSym();
                        if (sym.Type == Symbols.semicolon)
                        {
                            //Console.WriteLine("End.");
                        }
                        else
                        {
                            throw new Exception(String.Format("; expected {0} found", sym.Id));
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format(") expected {0} found", sym.Id));
                    }
                }
                else
                {
                    throw new Exception(String.Format("( expected {0} found", sym.Id));
                }
            }
            else
            {
                throw new Exception(String.Format("function name expected {0} found", sym.Id));
            }
        }

        private string Typ()
        {
            string t = "";
            if (sym.Type == Symbols.ident)
            {
                t = sym.Id;
                GetSym();
                if (sym.Type == Symbols.star)
                {
                    t += sym.Id;
                    GetSym();
                }
            }
            else
            {
                throw new Exception(String.Format("type expected {0} found", sym.Id));
            }
            return t;
        }

        private string Pname()
        {
            string n = "";
            if (sym.Type == Symbols.ident)
            {
                n = sym.Id;
                GetSym();

                if (sym.Type == Symbols.lbrak)
                {
                    GetSym();
                    if (sym.Type == Symbols.rbrak)
                    {
                        n += "[]";
                        GetSym();
                    }
                    else
                    {
                        throw new Exception(String.Format("] expected {0} found", sym.Id));
                    }
                }

            }
            else
            {
                throw new Exception(String.Format("variable name expected {0} found", sym.Id));
            }
            return n;
        }

        private void GetSym()
        {
            sym = lex.GetSym();
        }
    }
}
