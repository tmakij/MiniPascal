﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class CILEmitter
    {
        //public CILEmitter Previous { get; }

        private static readonly Type[] stringTypes = new[] { typeof(string), typeof(string) };
        private readonly Dictionary<Identifier, LocalBuilder> localValueVariables = new Dictionary<Identifier, LocalBuilder>();
        private readonly Dictionary<Identifier, MethodBuilder> procedures = new Dictionary<Identifier, MethodBuilder>();
        private readonly ILGenerator generator;
        private readonly TypeBuilder mainType;
        private readonly Parameters parameters;
        private readonly Scope scope;
        private MethodBuilder currentMethod;

        public CILEmitter(ILGenerator Generator, TypeBuilder MainType, MethodBuilder Current, Scope Scope, Parameters Parameters)
        {
            generator = Generator;
            mainType = MainType;
            currentMethod = Current;
            scope = Scope;
            parameters = Parameters;
        }

        public void CreateVariable(Identifier Identifier, MiniPascalType Type)
        {
            LocalBuilder variable = generator.DeclareLocal(Type.CLRType);
            localValueVariables.Add(Identifier, variable);
        }

        public void SaveReferenceVariable(Variable Variable)
        {
            ushort loc = parameters.Index(Variable.Identifier);
            MiniPascalType varType = Variable.Type;
            if (varType.Equals(MiniPascalType.Integer) || varType.Equals(MiniPascalType.Boolean))
            {
                generator.Emit(OpCodes.Stind_I4);
            }
            else if (varType.Equals(MiniPascalType.Real))
            {
                generator.Emit(OpCodes.Stind_R4);
            }
            else if (varType.Equals(MiniPascalType.String))
            {
                generator.Emit(OpCodes.Stind_Ref);
            }
#if DEBUG
            else
            {
                throw new InvalidOperationException();
            }
#endif
        }

        public void SaveVariable(Identifier Variable)
        {
            if (localValueVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Stloc, localValueVariables[Variable]);
            }
            else
#if DEBUG
            if (parameters.HasParameter(Variable))
#endif
            {
                ushort loc = parameters.Index(Variable);
                generator.Emit(OpCodes.Starg, loc);
            }
#if DEBUG
            else
            {
                throw new InvalidOperationException();
            }
#endif
        }

        public void LoadReferenceVariable(Variable Variable)
        {
            ushort loc = parameters.Index(Variable.Identifier);
            generator.Emit(OpCodes.Ldarg, loc);
            MiniPascalType varType = Variable.Type;
            if (varType.Equals(MiniPascalType.Integer) || varType.Equals(MiniPascalType.Boolean))
            {
                generator.Emit(OpCodes.Ldind_I4);
            }
            else if (varType.Equals(MiniPascalType.Real))
            {
                generator.Emit(OpCodes.Ldind_R4);
            }
            else if (varType.Equals(MiniPascalType.String))
            {
                generator.Emit(OpCodes.Ldind_Ref);
            }
        }

        public void LoadVariable(Identifier Variable)
        {
            Console.WriteLine(Variable);
            Console.WriteLine(parameters);
            if (localValueVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Ldloc, localValueVariables[Variable]);
            }
            else if (parameters.HasParameter(Variable))
            {
                generator.Emit(OpCodes.Ldarg, parameters.Index(Variable));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void LoadVariableAddress(Identifier Variable)
        {
            generator.Emit(OpCodes.Ldloca, localValueVariables[Variable]);
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

        public void If(Action Result)
        {
            Label end = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse_S, end);
            Result();
            generator.MarkLabel(end);
        }

        public void IfElse(Action True, Action False)
        {
            Label falseBranch = generator.DefineLabel();
            Label end = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse_S, falseBranch);
            True();
            generator.Emit(OpCodes.Br_S, end);
            generator.MarkLabel(falseBranch);
            False();
            generator.MarkLabel(end);
        }

        public CILEmitter StartBlock(Scope Scope)
        {
            Console.WriteLine("Start block");
            CILEmitter next = new CILEmitter(generator, mainType, currentMethod, scope, parameters);
            return next;
        }

        public CILEmitter StartProcedure(Identifier Identifier, Parameters Parameters)
        {
            List<Type> types = new List<Type>();
            foreach (Variable variable in Parameters.All)
            {
                bool byRef = variable.IsReference;
                types.Add(byRef ? variable.Type.CLRType.MakeByRefType() : variable.Type.CLRType);
            }
            /*
            for (int i = 0; i < Parameters.DeclaredCount; i++)
            {
                Type curr = Parameters.Types[i];
                bool byRef = Parameters.At(i).IsReference;
                types.Add(byRef ? curr.MakeByRefType() : curr);
            }
            */
            MethodBuilder mb = mainType.DefineMethod(Identifier.ToString(), MethodAttributes.Private | MethodAttributes.Static, null, types.ToArray());
            procedures.Add(Identifier, mb);
            return new CILEmitter(mb.GetILGenerator(), mainType, mb, scope, Parameters);
        }

        public void EndProcedure()
        {
            Console.WriteLine("End Proc " + currentMethod.Name);
            generator.Emit(OpCodes.Ret);
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
