using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class CILEmitter
    {
        private static readonly Type[] stringTypes = new[] { typeof(string), typeof(string) };
        private Parameters parameters;
        private readonly Dictionary<Identifier, LocalBuilder> localVariables = new Dictionary<Identifier, LocalBuilder>();
        private readonly Dictionary<Identifier, MethodBuilder> procedures = new Dictionary<Identifier, MethodBuilder>();
        private ILGenerator generator, mg;
        private readonly TypeBuilder mainType;
        private readonly MethodBuilder main;

        public CILEmitter(ILGenerator Generator, TypeBuilder MainType, MethodBuilder Main)
        {
            generator = Generator;
            mainType = MainType;
            main = Main;
        }

        public void CreateVariable(Identifier Identifier, MiniPascalType Type)
        {
            LocalBuilder variable = generator.DeclareLocal(Type.CLRType);
            localVariables.Add(Identifier, variable);
        }

        public void SaveVariable(Identifier Variable)
        {
            generator.Emit(OpCodes.Stloc, localVariables[Variable]);
        }

        public void LoadVariable(Identifier Variable)
        {
            if (localVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Ldloc, localVariables[Variable]);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg, parameters.Index(Variable));
            }
        }

        public void PushString(string Value)
        {
            generator.Emit(OpCodes.Ldstr, Value);
        }

        public void PushInt32(int Value)
        {
            generator.Emit(OpCodes.Ldc_I4, Value);
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

        public void GreaterThan()
        {
            generator.Emit(OpCodes.Cgt);
        }

        public void ToInt32()
        {
            generator.Emit(OpCodes.Conv_I4);
        }

        public void CreateProcedure(Identifier Identifier, Parameters Parameters)
        {
            MethodBuilder mb = mainType.DefineMethod(Identifier.ToString(), MethodAttributes.Private | MethodAttributes.Static, null, Parameters.Types);
            procedures.Add(Identifier, mb);
            mg = generator;
            generator = mb.GetILGenerator();
            parameters = Parameters;
        }

        public void EndProcedure()
        {
            generator.Emit(OpCodes.Ret);
            generator = mg;
        }

        public void Call(Identifier ProcedureId)
        {
            generator.Emit(OpCodes.Call, procedures[ProcedureId]);
        }

        public void CallPrint(MiniPascalType Type)
        {
            if (Type.Equals(MiniPascalType.Boolean))
            {
                Label trueRes = generator.DefineLabel();
                Label end = generator.DefineLabel();
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Ceq);
                generator.Emit(OpCodes.Brtrue_S, trueRes);
                PushString(bool.FalseString);
                generator.Emit(OpCodes.Br, end);
                generator.MarkLabel(trueRes);
                PushString(bool.TrueString);
                generator.MarkLabel(end);
                generator.Emit(OpCodes.Nop);
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.Write), MiniPascalType.String.CLRTypeArray));
            }
            else
            {
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.Write), Type.CLRTypeArray));
            }
        }

        public void CallStringLessThan()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod(nameof(string.Compare), stringTypes));
            Label trueRes = generator.DefineLabel();
            Label end = generator.DefineLabel();
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Clt);
            generator.Emit(OpCodes.Brtrue_S, trueRes);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Br, end);
            generator.MarkLabel(trueRes);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.MarkLabel(end);
            generator.Emit(OpCodes.Nop);
        }

        public void CallStringEquals()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality"));
            Label trueRes = generator.DefineLabel();
            Label end = generator.DefineLabel();
            generator.Emit(OpCodes.Brtrue_S, trueRes);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Br, end);
            generator.MarkLabel(trueRes);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.MarkLabel(end);
            generator.Emit(OpCodes.Nop);
        }

        public void CallStringConcat()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod(nameof(string.Concat), stringTypes));
        }
    }
}
