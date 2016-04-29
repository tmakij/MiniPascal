using System;

namespace MiniPL.Parser.AST
{
    public sealed class Identifier : IEquatable<Identifier>
    {
        private readonly string name;

        public Identifier(string Name)
        {
            name = Name;
        }

        public override string ToString()
        {
            return name;
        }

        public bool Equals(Identifier Other)
        {
            return Other.name == name;
        }

        public override bool Equals(object Other)
        {
            return Equals((Identifier)Other);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
