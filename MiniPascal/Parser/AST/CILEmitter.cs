using System;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
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

        public void PushInt32(int Value)
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

        public void Add()
        {
            generator.Emit(OpCodes.Add);
        }

        public void Multiply()
        {
            generator.Emit(OpCodes.Mul);
        }

        public void Negate()
        {
            generator.Emit(OpCodes.Not);
        }

        public void Or()
        {
            generator.Emit(OpCodes.Or);
        }

        public void Divide()
        {
            generator.Emit(OpCodes.Div);
        }

        public void Substract()
        {
            generator.Emit(OpCodes.Sub);
        }

        public void Modulo()
        {
            generator.Emit(OpCodes.Rem);
        }

        public void Equals()
        {
            generator.Emit(OpCodes.Ceq);
        }

        public void LessThan()
        {
            generator.Emit(OpCodes.Clt);
        }

        public void ToInt32()
        {
            generator.Emit(OpCodes.Conv_I4);
        }

        public void CallPrint(MiniPascalType Type)
        {
            generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), Type.CLRTypeArray));
        }

        public void CallEquatable(MiniPascalType Type)
        {
            generator.EmitCall(OpCodes.Call, Type.CLRType.GetMethod(nameof(int.Equals)), Type.CLRTypeArray);
        }

        public void CallComparable(MiniPascalType Type)
        {
            generator.Emit(OpCodes.Pop);
            generator.Emit(OpCodes.Pop);
            LocalBuilder variable = generator.DeclareLocal(Type.CLRType);
            generator.Emit(OpCodes.Ldloca, variable);
            generator.Emit(OpCodes.Ldc_I4, 1);
            generator.EmitCall(OpCodes.Call, Type.CLRType.GetMethod(nameof(int.CompareTo), Type.CLRTypeArray), null);
            //generator.Emit(OpCodes.Ldftn, Type.CLRType.GetMethod(nameof(int.CompareTo)));
            //generator.EmitCalli(OpCodes.Calli, System.Reflection.CallingConventions.HasThis, Type.CLRType, Type.CLRTypeArray, null);
            //generator.EmitCalli(OpCodes.Call, Type.CLRType.GetMethod(nameof(int.CompareTo)), Type.CLRTypeArray);
            // Type.CLRType.GetMethod(nameof(int.CompareTo))
        }
    }
}
