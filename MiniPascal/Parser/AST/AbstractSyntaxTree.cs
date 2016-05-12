using System;
using System.IO;
using System.Diagnostics;
//using System.Security;
//using System.Security.Policy;
//using System.Security.Permissions;
using System.Reflection;
using System.Reflection.Emit;

namespace MiniPascal.Parser.AST
{
    public sealed class AbstractSyntaxTree
    {
        private string Name { get; }
        private readonly ScopedProgram statements;
        private readonly IdentifierTypes types = new IdentifierTypes();

        public AbstractSyntaxTree(ScopedProgram Statements, Identifier Name)
        {
            statements = Statements;
            this.Name = Name.ToString() + ".exe";
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

        public void GenerateCIL(string Location)
        {
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MP ASSEMBLY"), AssemblyBuilderAccess.Save);
            ModuleBuilder mb = ab.DefineDynamicModule(Name);
            TypeBuilder MainType = mb.DefineType("MainType", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);


            //MethodBuilder strEq = StringEquals(MainType);

            MethodBuilder main = MainType.DefineMethod("Main", MethodAttributes.Private | MethodAttributes.Static);
            ILGenerator emitter = main.GetILGenerator();

            CILEmitter cilEmitter = new CILEmitter(emitter, MainType, main);
            statements.EmitIR(cilEmitter, types);

            emitter.Emit(OpCodes.Ret);
            Type mainTypeFinal = MainType.CreateType();

            ab.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
            ab.Save(Name, PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);
            
            string target = Path.Combine(Location, Name);
            if (target != Path.GetFullPath(Name) && File.Exists(target))
            {
                File.Delete(target);
            }
            File.Move(Name, target);
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

        public void Execute()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Name;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit(7500);
            Console.Write(proc.StandardOutput.ReadToEnd());
            //AppDomain resultDomain = AppDomain.CreateDomain("executingAssembly");
            //resultDomain.ExecuteAssembly(Path.Combine(Environment.CurrentDirectory, Name));
        }

        /*private MethodBuilder StringEquals(TypeBuilder MainType)
        {
            MethodBuilder ret = MainType.DefineMethod("StringEquality", MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard,
                typeof(int), null, null, new[] { typeof(string), typeof(string) }, null, null);
            ILGenerator str = ret.GetILGenerator();
            ret.DefineParameter(0, ParameterAttributes.None, null);
            ret.DefineParameter(1, ParameterAttributes.None, null);
            str.Emit(OpCodes.Ldarg_0);
            str.Emit(OpCodes.Ldarg_1);
            str.EmitCall(OpCodes.Call, typeof(string));
            str.Emit(OpCodes.Ret);
            return ret;
        }*/
    }
}
