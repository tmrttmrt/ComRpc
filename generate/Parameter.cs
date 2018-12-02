using System;

namespace Parser.Declarations
{
    [Serializable]
    public struct Parameter
    {
        public string Type;
        public string Name;

        public Parameter(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
