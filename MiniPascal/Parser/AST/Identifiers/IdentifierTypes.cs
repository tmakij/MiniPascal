using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class IdentifierTypes
    {
        private readonly Dictionary<Identifier, MiniPascalType> types = new Dictionary<Identifier, MiniPascalType>();
        private readonly Dictionary<Identifier, Procedure> procedureTypes = new Dictionary<Identifier, Procedure>();

        public void SetIdentifierType(Identifier Identifier, MiniPascalType Type)
        {
            if (!types.ContainsKey(Identifier))
            {
                types.Add(Identifier, null);
            }
            types[Identifier] = Type;
        }

        public void SetProcedureType(Identifier Identifier, Procedure Procedure)
        {
            procedureTypes.Add(Identifier, Procedure);
        }

        public MiniPascalType GetIdentifierType(Identifier Identifier)
        {
            return types[Identifier];
        }

        public Procedure Procedure(Identifier Indentifier)
        {
            return procedureTypes[Indentifier];
        }
    }
}
