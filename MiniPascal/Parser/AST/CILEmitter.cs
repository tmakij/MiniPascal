using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class CILEmitter
    {
        private readonly Dictionary<Identifier, LocalBuilder> localVariables = new Dictionary<Identifier, LocalBuilder>();
        private readonly ILGenerator generator;

        public CILEmitter(ILGenerator Generator)
        {
            generator = Generator;
        }

        public void CreateVariable(Identifier Identifier, MiniPascalType Type)
        {
            LocalBuilder variable = generator.DeclareLocal(Type.CLRType);
            localVariables.Add(Identifier, variable);
        }

        public void PushInt(int Value)
        {
            generator.Emit(OpCodes.Ldc_I4, Value);
        }

        public void SaveVariable(Identifier Variable)
        {
            generator.Emit(OpCodes.Stloc, localVariables[Variable]);
        }

        public void LoadVariable(Identifier Variable)
        {
            generator.Emit(OpCodes.Ldloc, localVariables[Variable]);
        }

        public void PushSingle(float Value)
        {
            generator.Emit(OpCodes.Ldc_R4, Value);
        }

        /*
        public void UnLoad()
        {
            generator.Emit(OpCodes.Pop);
        }
        */

        public void CallPrint(MiniPascalType Type)
        {
            if (Type.Equals(MiniPascalType.Integer))
            {
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(int) }));
            }
            else if (Type.Equals(MiniPascalType.Real))
            {
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(float) }));
            }
        }
    }
}
