using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class CILEmitter
    {
        private static readonly Type[] stringTypes = new[] { typeof(string), typeof(string) };

        private readonly Dictionary<Identifier, LocalBuilder> localValueVariables = new Dictionary<Identifier, LocalBuilder>();
        private readonly Dictionary<Identifier, MethodBuilder> procedures = new Dictionary<Identifier, MethodBuilder>();
        private readonly ILGenerator generator;
        private readonly TypeBuilder mainType;
        private readonly Parameters parameters;
        private readonly Scope scope;
        private readonly MethodBuilder currentMethod;
        private readonly CILEmitter previous;

        public CILEmitter(ILGenerator Generator, TypeBuilder MainType, MethodBuilder Current, Scope Scope, Parameters Parameters, CILEmitter Previous)
        {
            generator = Generator;
            mainType = MainType;
            currentMethod = Current;
            scope = Scope;
            parameters = Parameters;
            previous = Previous;
        }

        public void CreateArrayVariable(Identifier Identifier, MiniPascalType Type)
        {
            CreateVariable(Identifier, Type.SimpleType.CLRType.MakeArrayType());
        }

        public void CreateSimpleVariable(Identifier Identifier, MiniPascalType Type)
        {
            CreateVariable(Identifier, Type.SimpleType.CLRType);
        }

        private void CreateVariable(Identifier Identifier, Type Type)
        {
            LocalBuilder variable = generator.DeclareLocal(Type);
            localValueVariables.Add(Identifier, variable);
        }

        public void SaveReferenceVariable(Variable Variable)
        {
            SimpleType varType = Variable.Type.SimpleType;
            if (varType.Equals(SimpleType.Integer) || varType.Equals(SimpleType.Boolean))
            {
                generator.Emit(OpCodes.Stind_I4);
            }
            else if (varType.Equals(SimpleType.Real))
            {
                generator.Emit(OpCodes.Stind_R4);
            }
            else if (varType.Equals(SimpleType.String))
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

        public void SaveArray(Variable Variable)
        {
            SimpleType type = Variable.Type.SimpleType;
            OpCode code;
            if (type.Equals(SimpleType.Integer) || type.Equals(SimpleType.Boolean))
            {
                code = OpCodes.Stelem_I4;
            }
            else if (type.Equals(SimpleType.Real))
            {
                code = OpCodes.Stelem_R4;
            }
            else if (type.Equals(SimpleType.String))
            {
                code = OpCodes.Stelem_Ref;
            }
            else
            {
                throw new InvalidOperationException();
            }
            generator.Emit(code);
        }

        public void SaveVariable(Identifier Variable)
        {
            if (localValueVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Stloc, localValueVariables[Variable]);
            }
            else
            if (parameters != null && parameters.HasParameter(Variable))
            {
                byte loc = parameters.Index(Variable);
                generator.Emit(OpCodes.Starg_S, loc);
            }
            else
            {
                previous.SaveVariable(Variable);
            }
        }

        public void LoadReferenceVariable(Variable Variable)
        {
            byte loc = parameters.Index(Variable.Identifier);
            generator.Emit(OpCodes.Ldarg_S, loc);
            if (Variable.Type.IsArray)
            {
                generator.Emit(OpCodes.Ldind_Ref);
            }
            else
            {
                SimpleType varType = Variable.Type.SimpleType;
                if (varType.Equals(SimpleType.Integer) || varType.Equals(SimpleType.Boolean))
                {
                    generator.Emit(OpCodes.Ldind_I4);
                }
                else if (varType.Equals(SimpleType.Real))
                {
                    generator.Emit(OpCodes.Ldind_R4);
                }
                else if (varType.Equals(SimpleType.String))
                {
                    generator.Emit(OpCodes.Ldind_Ref);
                }
            }
        }

        public void LoadArrayVariable(Variable Variable)
        {
            SimpleType type = Variable.Type.SimpleType;
            OpCode code;
            if (type.Equals(SimpleType.Integer) || type.Equals(SimpleType.Boolean))
            {
                code = OpCodes.Ldelem_I4;
            }
            else if (type.Equals(SimpleType.Real))
            {
                code = OpCodes.Ldelem_R4;
            }
            else if (type.Equals(SimpleType.String))
            {
                code = OpCodes.Ldelem_Ref;
            }
            else
            {
                throw new InvalidOperationException();
            }
            generator.Emit(code);
        }

        public void LoadArrayIndexAddress()
        {
            generator.Emit(OpCodes.Ldelema);
        }

        private static OpCode Ldarg(byte Value)
        {
            switch (Value)
            {
                case 1:
                    return OpCodes.Ldarg_1;
                case 2:
                    return OpCodes.Ldarg_2;
                case 3:
                    return OpCodes.Ldarg_3;
#if DEBUG
                case 0:
#else
                default:
#endif
                    return OpCodes.Ldarg_0;
#if DEBUG
                default:
                    throw new InvalidOperationException();
#endif
            }
        }

        public void LoadVariable(Identifier Variable)
        {
            if (localValueVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Ldloc, localValueVariables[Variable]);
            }
            else if (parameters != null && parameters.HasParameter(Variable))
            {
                byte argIndex = parameters.Index(Variable);
                if (argIndex >= 0 && argIndex <= 3)
                {
                    generator.Emit(Ldarg(argIndex));
                }
                else
                {
                    generator.Emit(OpCodes.Ldarg_S, argIndex);
                }
            }
            else
            {
                previous.LoadVariable(Variable);
            }
        }

        public void LoadVariableAddress(Identifier Variable)
        {
            if (localValueVariables.ContainsKey(Variable))
            {
                generator.Emit(OpCodes.Ldloca_S, localValueVariables[Variable]);
            }
            else if (parameters != null && parameters.HasParameter(Variable))
            {
                byte loc = parameters.Index(Variable);
                generator.Emit(OpCodes.Ldarga_S, loc);
            }
            else
            {
                previous.LoadVariable(Variable);
            }
        }

        public void PushArray(MiniPascalType Type)
        {
            generator.Emit(OpCodes.Newarr, Type.SimpleType.CLRType);
        }

        public void PushString(string Value)
        {
            generator.Emit(OpCodes.Ldstr, Value);
        }

        private static OpCode IntCode(int Value)
        {
            switch (Value)
            {
                case -1:
                    return OpCodes.Ldc_I4_M1;
                case 0:
                    return OpCodes.Ldc_I4_0;
                case 1:
                    return OpCodes.Ldc_I4_1;
                case 2:
                    return OpCodes.Ldc_I4_2;
                case 3:
                    return OpCodes.Ldc_I4_3;
                case 4:
                    return OpCodes.Ldc_I4_4;
                case 5:
                    return OpCodes.Ldc_I4_5;
                case 6:
                    return OpCodes.Ldc_I4_6;
                case 7:
                    return OpCodes.Ldc_I4_7;
                case 8:
                    return OpCodes.Ldc_I4_8;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void PushInt32(int Value)
        {
            if (-1 <= Value && Value <= 8)
            {
                generator.Emit(IntCode(Value));
            }
            else
            {
                generator.Emit(OpCodes.Ldc_I4, Value);
            }
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

        public void ArraySize()
        {
            generator.Emit(OpCodes.Ldlen);
        }

        public void Return()
        {
            generator.Emit(OpCodes.Ret);
        }

        public void Pop()
        {
            generator.Emit(OpCodes.Pop);
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

        public void While(Action Condition, Action Then)
        {
            Label cond = generator.DefineLabel();
            Label start = generator.DefineLabel();
            generator.Emit(OpCodes.Br_S, cond);
            generator.MarkLabel(start);
            Then();
            generator.MarkLabel(cond);
            Condition();
            generator.Emit(OpCodes.Brtrue_S, start);
        }

        public void Assert()
        {
            Label pass = generator.DefineLabel();
            generator.Emit(OpCodes.Brtrue_S, pass);
            PushString(Environment.NewLine + "Assert failure" + Environment.NewLine);
            generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.Error.Write), MiniPascalType.String.SimpleType.CLRTypeArray));
            PushInt32(-1);
            generator.Emit(OpCodes.Call, typeof(Environment).GetMethod(nameof(Environment.Exit)));
            generator.MarkLabel(pass);
        }

        public void StartBlock(Scope Scope, Action<CILEmitter> ScopeCode)
        {
            generator.BeginScope();
            CILEmitter next = new CILEmitter(generator, mainType, currentMethod, Scope, parameters, this);
            ScopeCode(next);
            generator.EndScope();
        }

        public CILEmitter StartProcedure(Identifier Identifier, Parameters Parameters, MiniPascalType ReturnType)
        {
            List<Type> types = new List<Type>();
            foreach (Variable variable in Parameters.All)
            {
                Type type = variable.Type.SimpleType.CLRType;
                bool byRef = variable.IsReference;
                bool array = variable.Type.IsArray;
                if (array)
                {
                    type = type.MakeArrayType();
                }
                if (byRef)
                {
                    type = type.MakeByRefType();
                }
                types.Add(type);
            }
            Type ret = ReturnType == null ? null : ReturnType.SimpleType.CLRType;
            MethodBuilder mb = mainType.DefineMethod(Identifier.ToString(), MethodAttributes.Private | MethodAttributes.Static, ret, types.ToArray());
            procedures.Add(Identifier, mb);
            return new CILEmitter(mb.GetILGenerator(), mainType, mb, scope, Parameters, this);
        }

        public void Call(Identifier ProcedureId)
        {
            generator.Emit(OpCodes.Call, GetProcedure(ProcedureId));
        }

        private MethodBuilder GetProcedure(Identifier ProcedureId)
        {
            if (procedures.ContainsKey(ProcedureId))
            {
                return procedures[ProcedureId];
            }
            return previous.GetProcedure(ProcedureId);
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
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.Write), SimpleType.String.CLRTypeArray));
            }
            else
            {
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.Write), Type.SimpleType.CLRTypeArray));
            }
        }

        public void CallRead(MiniPascalType Type)
        {
            generator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.ReadLine)));
            if (Type.SimpleType.Equals(SimpleType.Integer))
            {
                generator.Emit(OpCodes.Call, typeof(int).GetMethod(nameof(int.Parse), SimpleType.String.CLRTypeArray));
            }
            else if (Type.SimpleType.Equals(SimpleType.Real))
            {
                generator.Emit(OpCodes.Call, typeof(float).GetMethod(nameof(float.Parse), SimpleType.String.CLRTypeArray));
            }
            else if (Type.SimpleType.Equals(SimpleType.Boolean))
            {
                generator.Emit(OpCodes.Call, typeof(bool).GetMethod(nameof(bool.Parse), SimpleType.String.CLRTypeArray));
            }
        }

        private void CallStringCompare()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod(nameof(string.Compare), stringTypes));
        }

        private void CompareStrings(int Base, OpCode Compare)
        {
            CallStringCompare();
            Label trueRes = generator.DefineLabel();
            Label end = generator.DefineLabel();
            PushInt32(Base);
            generator.Emit(Compare);
            generator.Emit(OpCodes.Brtrue_S, trueRes);
            PushInt32(0);
            generator.Emit(OpCodes.Br, end);
            generator.MarkLabel(trueRes);
            PushInt32(1);
            generator.MarkLabel(end);
        }

        public void CallStringGreaterThan()
        {
            CompareStrings(0, OpCodes.Cgt);
        }

        public void CallStringGreaterOrEqualThan()
        {
            CompareStrings(-1, OpCodes.Cgt);
        }

        public void CallStringLessThan()
        {
            CompareStrings(0, OpCodes.Clt);
        }

        public void CallStringLessOrEqualThan()
        {
            CompareStrings(1, OpCodes.Clt);
        }

        public void CallStringEquals()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod(nameof(string.Equals), stringTypes));
            Label trueRes = generator.DefineLabel();
            Label end = generator.DefineLabel();
            generator.Emit(OpCodes.Brtrue_S, trueRes);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Br, end);
            generator.MarkLabel(trueRes);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.MarkLabel(end);
        }

        public void CallStringConcat()
        {
            generator.Emit(OpCodes.Call, typeof(string).GetMethod(nameof(string.Concat), stringTypes));
        }
    }
}
