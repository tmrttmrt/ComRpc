using System.Text.RegularExpressions;

namespace Parser
{
    internal enum Symbols
    {
        unknown = -1, empty, typedef, unsigned, structure, ident, integer, lbrace, rbrace,
        lbrak, rbrak, star, comma, semicolon, rbracket, lbracket
    }

    internal struct Symbol
    {
        public Symbols Type;
        public string Id;

        public Symbol(Symbols type, string id)
        {
            this.Type = type;
            this.Id = id;
        }
    }

    internal class Lexer
    {
        private string input = "";
        private int pos = 0;
        private char ch = '\0';

        public Lexer(string input)
        {
            this.input = input;
        }

        /// <summary>
        /// Get next symbol from input string
        /// </summary>
        /// <returns>Object of type Symbol</returns>
        public Symbol GetSym()
        {
            string buf = "";
            Symbol sym = new Symbol(Symbols.empty, "");

            while (pos < input.Length && sym.Type == Symbols.empty)
            {
                ch = input[pos];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        sym = Analyze(buf);
                        buf = "";
                        pos += 1;
                        continue;
                    case ')':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.rbracket;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case '(':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.lbracket;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case '}':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.rbrace;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case '{':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.lbrace;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case '[':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.lbrak;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case ']':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.rbrak;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case ',':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.comma;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case ';':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.semicolon;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    case '*':
                        if (buf.Length == 0)
                        {
                            sym.Type = Symbols.star;
                            break;
                        }
                        sym = Analyze(buf);
                        buf = "";
                        continue;
                    default:
                        buf += ch;
                        pos += 1;
                        continue;
                }
                pos += 1;
                sym.Id = ch.ToString();

            }
            return sym;
        }
        
        private Symbol Analyze(string s)
        {
            Symbol sym = new Symbol(Symbols.empty, "");
            if (s == "") return sym;

            if (s == "typedef")
                sym.Type = Symbols.typedef;
            else if (s == "struct")
                sym.Type = Symbols.structure;
            else if (s == "unsigned")
                sym.Type = Symbols.unsigned;
            else if (Regex.IsMatch(s, @"\A[0-9]+\Z"))
                sym.Type = Symbols.integer;
            else if (Regex.IsMatch(s, @"\A[a-zA-Z_]+[0-9a-zA-Z_]*\Z"))
                sym.Type = Symbols.ident;
            else
                sym.Type = Symbols.unknown;

            sym.Id = s;
            return sym;
        }
    }
}
