using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Parameters
    {
        private readonly List<DeclarationStatement> parameters = new List<DeclarationStatement>();

        public void Add(DeclarationStatement Parameter)
        {
            parameters.Add(Parameter);
        }
    }
}
