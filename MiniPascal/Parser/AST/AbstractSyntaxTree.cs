using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MiniPL.Parser.AST
{
    public sealed class AbstractSyntaxTree
    {
        private readonly ScopedProgram statements;

        public AbstractSyntaxTree(ScopedProgram Statements)
        {
            statements = Statements;
        }

        public void CheckIdentifiers()
        {
            UsedIdentifiers identifiers = new UsedIdentifiers();
            statements.CheckIdentifiers(identifiers);
        }

        public void CheckTypes()
        {
            IdentifierTypes types = new IdentifierTypes();
            statements.CheckTypes(types);
        }

        public void Execute()
        {
            Variables globalScope = new Variables();
            statements.Execute(globalScope);
        }

        public void GenerateCIL()
        {
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MP ASSEMBLY"), AssemblyBuilderAccess.Save);
            ModuleBuilder mb = ab.DefineDynamicModule("test.exe");
            TypeBuilder MainType = mb.DefineType("MainType", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
            MethodBuilder main = MainType.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static);
            ILGenerator emitter = main.GetILGenerator();
            emitter.Emit(OpCodes.Ldstr, "Hello World!");
            emitter.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) }));
            emitter.Emit(OpCodes.Ret);
            Type mainTypeFinal = MainType.CreateType();
            
            ab.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
            ab.Save("test.exe");
        }
    }
}
