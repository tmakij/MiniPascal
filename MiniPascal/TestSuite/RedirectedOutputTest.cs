using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public abstract class RedirectedOutputTest
    {
        protected string output { get { return proc.StandardOutput.ReadToEnd(); } }
        private Process proc;

        protected void Redirect(string FirstBlockInput)
        {
            const string binaryName = "TestBinary";
            string temp = Path.GetTempPath();
            string fullName = Path.Combine(temp, binaryName);
            string input = "program " + binaryName + ";" + FirstBlockInput;
            using (StringReader stringReader = new StringReader(input))
            {
                SourceStream ss = new SourceStream(stringReader);
                Compiler.Compile(ss, temp);
            }
            proc = new Process();
            proc.StartInfo.FileName = fullName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit(7500);
        }

        [TearDown]
        private void Dispose()
        {
            if (proc != null)
            {
                proc.Dispose();
                proc = null;
            }
        }
    }
}
