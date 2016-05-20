using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Read : IStatement
    {
        private readonly List<AssigmentStatement> assigments = new List<AssigmentStatement>();

        public void Add(AssigmentStatement Assigment)
        {
            assigments.Add(Assigment);
        }

        public void CheckIdentifiers(Scope Current)
        {
            foreach (AssigmentStatement assigment in assigments)
            {
                assigment.CheckIdentifiers(Current);
            }
        }

        public void CheckType(Scope Current)
        {
            foreach (AssigmentStatement assigment in assigments)
            {
                assigment.CheckType(Current);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            foreach (AssigmentStatement assigment in assigments)
            {
                assigment.EmitIR(Emitter);
            }
            /*
            foreach (Variable variable in arguments.All)
            {
                if (variable.IsReference)
                {
                    Emitter.LoadVariable(variable.Identifier);
                }
                else
                {
                    Emitter.LoadVariableAddress(variable.Identifier);
                }
                if (variable.Type.IsArray)
                {
                    variable.Type.Size.EmitIR(Emitter, false);
                    Emitter.LoadArrayIndexAddress();
                }
                //Emitter.CallRead(expr.Type);
                //Emitter.SaveReferenceVariable();
            }
            /*foreach (Variable prevVar in ToCall.Parameters.PreviousVariables)
            {
                if (current.Variable(prevVar.Identifier).IsReference)
                {
                    Emitter.LoadVariable(prevVar.Identifier);
                }
                else
                {
                    Emitter.LoadVariableAddress(prevVar.Identifier);
                }
            }*/
        }
    }
}
