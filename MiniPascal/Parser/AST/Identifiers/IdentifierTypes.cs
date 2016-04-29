using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class IdentifierTypes
    {
        private readonly Dictionary<Identifier, MiniPascalType> types = new Dictionary<Identifier, MiniPascalType>();

        public void SetIdentifierType(Identifier Identifier, MiniPascalType Type)
        {
            types.Add(Identifier, Type);
        }

        public MiniPascalType GetIdentifierType(Identifier Identifier)
        {
            return types[Identifier];
        }
    }
}
