using System;
//using System.Security;
//using System.Security.Policy;
//using System.Security.Permissions;
using System.Reflection;
using System.Reflection.Emit;

namespace MiniPascal.Parser.AST
{
    public sealed class AbstractSyntaxTree
    {
        private readonly Identifier name;
        private readonly ScopedProgram statements;
        private readonly IdentifierTypes types = new IdentifierTypes();

        public AbstractSyntaxTree(ScopedProgram Statements, Identifier Name)
        {
            statements = Statements;
            name = Name;
        }

        public void CheckIdentifiers()
        {
            UsedIdentifiers identifiers = new UsedIdentifiers();
            statements.CheckIdentifiers(identifiers);
        }

        public void CheckTypes()
        {
            statements.CheckTypes(types);
        }

        public void GenerateCIL()
        {
            string moduleName = name.ToString() + ".exe";

            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MP ASSEMBLY"), AssemblyBuilderAccess.Save);
            ModuleBuilder mb = ab.DefineDynamicModule(moduleName);
            TypeBuilder MainType = mb.DefineType("MainType", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
            MethodBuilder main = MainType.DefineMethod("Main", MethodAttributes.Private | MethodAttributes.Static);
            ILGenerator emitter = main.GetILGenerator();

            CILEmitter cilEmitter = new CILEmitter(emitter);
            statements.EmitIR(cilEmitter, types);

            emitter.Emit(OpCodes.Ret);
            Type mainTypeFinal = MainType.CreateType();

            ab.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
            ab.Save(moduleName);

            AppDomain resultDomain = AppDomain.CreateDomain("executingAssembly");
            //try
            {
                resultDomain.ExecuteAssembly(moduleName);
            }
            //catch (InvalidProgramException ex)
            {
                //Console.WriteLine("Error: " + ex.StackTrace);
            }

            /*
            StrongName fullTrustAssembly = Assembly.GetExecutingAssembly().Evidence.GetHostEvidence<StrongName>();
            PermissionSet permSet = new PermissionSet(PermissionState.None);
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = "D://";

            AppDomain resultDomain = AppDomain.CreateDomain("asd", null, adSetup, permSet, fullTrustAssembly);
            resultDomain.ExecuteAssembly("test.exe");
            */
        }
    }
}
