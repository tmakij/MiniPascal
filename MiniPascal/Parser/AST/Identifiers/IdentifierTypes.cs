using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class IdentifierTypes
    {
        private readonly Dictionary<Identifier, SimpleType> types = new Dictionary<Identifier, SimpleType>();
        private readonly Dictionary<Identifier, Procedure> procedureTypes = new Dictionary<Identifier, Procedure>();

        public void SetIdentifierType(Identifier Identifier, SimpleType Type)
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

        public SimpleType GetIdentifierType(Identifier Identifier)
        {
            return types[Identifier];
        }

        public Procedure Procedure(Identifier Indentifier)
        {
            return procedureTypes[Indentifier];
        }
    }
}
