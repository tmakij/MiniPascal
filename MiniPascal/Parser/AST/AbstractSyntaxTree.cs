using System;
//using System.Security;
//using System.Security.Policy;
//using System.Security.Permissions;
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
            MethodBuilder main = MainType.DefineMethod("Main", MethodAttributes.Private | MethodAttributes.Static);
            ILGenerator emitter = main.GetILGenerator();

            CILEmitter c = new CILEmitter(emitter);
            statements.statements.ForEach(p =>
            {
                p.EmitIR(c);
            });

            //emitter.Emit(OpCodes.Ldstr, "Hello World!");
            //emitter.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) }));
            emitter.Emit(OpCodes.Ret);
            Type mainTypeFinal = MainType.CreateType();

            ab.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
            ab.Save("test.exe");

            AppDomain resultDomain = AppDomain.CreateDomain("asd");
            //try
            {
                resultDomain.ExecuteAssembly("test.exe");
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
