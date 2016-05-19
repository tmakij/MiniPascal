using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class ScopedProgram : IStatement
    {
        public Scope Scope { get; private set; }
        private readonly List<IStatement> statements = new List<IStatement>();

        public void Add(IStatement Statement)
        {
            statements.Add(Statement);
        }

        public void AddParameters(Parameters Parameters)
        {
            statements.Insert(0, Parameters);
        }

        public void CheckIdentifiers(Scope Current)
        {
            Scope = new Scope(Current);
            foreach (IStatement item in statements)
            {
                item.CheckIdentifiers(Scope);
            }
        }

        public void CheckType(Scope Current)
        {
            foreach (IStatement item in statements)
            {
                item.CheckType(Scope);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.StartBlock(Scope,
                (emitter) =>
                {
                    foreach (IStatement item in statements)
                    {
                        item.EmitIR(emitter);
                    }
                }
            );
        }
    }
}
