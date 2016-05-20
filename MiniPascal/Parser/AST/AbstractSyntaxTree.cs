using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MiniPascal.Parser.AST
{
    public sealed class AbstractSyntaxTree
    {
        private string Name { get; }
        private readonly ScopedProgram statements;
        private readonly Scope root = new Scope(null);

        public AbstractSyntaxTree(ScopedProgram Statements, Identifier Name)
        {
            statements = Statements;
            this.Name = Name.ToString() + ".exe";
        }

        public void CheckIdentifiers()
        {
            statements.CheckIdentifiers(root);
        }

        public void CheckTypes()
        {
            statements.CheckType(root);
        }

        public void GenerateCIL(string Location)
        {
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("MiniPascal Assembly"), AssemblyBuilderAccess.Save, Location);
            ModuleBuilder mb = ab.DefineDynamicModule(Name);
            TypeBuilder MainType = mb.DefineType("MainType", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);

            MethodBuilder main = MainType.DefineMethod("main", MethodAttributes.Private | MethodAttributes.Static);
            ILGenerator emitter = main.GetILGenerator();

            CILEmitter cilEmitter = new CILEmitter(emitter, MainType, main, null, null, null);
            statements.EmitIR(cilEmitter);
            cilEmitter.Return();

            Type mainTypeFinal = MainType.CreateType();

            ab.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
            ab.Save(Name, PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);
        }
    }
}
