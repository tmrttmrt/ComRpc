using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Parser.Declarations
{
    [Serializable]
    public class Declaration
    {
        /// <summary>
        /// Function name
        /// </summary>
        public string ProcName;
        /// <summary>
        /// Function return type
        /// </summary>
        public string ProcType;
        /// <summary>
        /// List of function parameters
        /// </summary>
        public List<Parameter> Parameters = new List<Parameter>();
    }

    public class DeclarationList : List<Declaration>
    {
        public Declaration Last
        {
            get
            {
                return this[Count - 1];
            }
        }
    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
