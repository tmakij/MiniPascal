using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class UsedIdentifiers
    {
        private readonly HashSet<Identifier> usedIdentifiers = new HashSet<Identifier>();
        private readonly HashSet<Identifier> immutableIdentifiers = new HashSet<Identifier>();

        public void DeclareVariable(Identifier Identifier)
        {
            usedIdentifiers.Add(Identifier);
        }

        public bool IsUsed(Identifier Identifier)
        {
            return usedIdentifiers.Contains(Identifier);
        }

        public bool IsMutable(Identifier Identifier)
        {
            return !immutableIdentifiers.Contains(Identifier);
        }

        public void SetMutable(Identifier Identifier)
        {
            immutableIdentifiers.Remove(Identifier);
        }

        public void SetImmutable(Identifier Identifier)
        {
            immutableIdentifiers.Add(Identifier);
        }
    }
}
