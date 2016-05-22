using System;
using System.IO;
using System.Text;
using MiniPascal.Parser;
using MiniPascal.Parser.AST;
using MiniPascal.Lexer;

namespace MiniPascal
{
    public static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                return Error("The program accepts one, and only one, argument, which is the path to the source");
            }
            try
            {
                string sourcePath = args[0];
                using (StreamReader sr = new StreamReader(sourcePath, Encoding.UTF8))
                {
                    SourceStream source = new SourceStream(sr);
                    Compiler.Compile(source, Environment.CurrentDirectory);
                }
                return 0;
            }
            catch (FileNotFoundException)
            {
                return Error("The given argument path was not found");
            }
            catch (LexerException ex)
            {
                return Error(ex.Message);
            }
            catch (SyntaxException ex)
            {
                return Error(ex.Message);
            }
            catch (UninitializedVariableException ex)
            {
                return Error("Uninitialized variable " + ex.Identifier);
            }
            catch (TypeMismatchException ex)
            {
                return Error("Type mismatch: Expected \"" + ex.Expected + "\" but \"" + ex.Found + "\" was found");
            }
            catch (InvalidSimpleTypeException ex)
            {
                return Error("Type mismatch: Expected \"" + ex.Expected + "\" but \"" + ex.Found + "\" was found");
            }
            catch (VariableNameDefinedException ex)
            {
                return Error("Variable \"" + ex.Identifier + "\" is defined more than once");
            }
            catch (UndefinedOperatorException ex)
            {
                return Error("Operator \"" + ex.Operator + "\" is not defined for the type " + ex.Type);
            }
            catch (InvalidByReferenceException)
            {
                return Error("Only variables can be passed by reference");
            }
            catch (InvalidParameterCountException ex)
            {
                return Error("Invalid amount of paramaters on method call: expected " + ex.Expected + " but " + ex.Found + " was found");
            }
            catch (InvalidArrayIndexTypeException ex)
            {
                return Error("Invalid array index type found expected integer but " + (ex.Found?.ToString() ?? "none") + " was found");
            }
            catch (ArrayAssigmentExpection ex)
            {
                return Error("Invalid array assigment: expected " + ex.Expected + " but " + ex.Found + " was found");
            }
            catch (ArrayRequiredException ex)
            {
                return Error("Invalid .size operator usage: expected array but " + ex.Found + " was found");
            }
            catch (InvalidTypeException ex)
            {
                string exp = ex.Expected[0].ToString();
                for (int i = 1; i < ex.Expected.Length; i++)
                {
                    exp += ", " + ex.Expected[i];
                }
                return Error("Type mismatch: Expected \"" + exp + "\" but \"" + ex.Found + "\" was found");
            }
            catch (Exception ex)
            {
                return Error("Internal compiler errors:\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static int Error(string Message)
        {
            Console.Error.WriteLine(Message);
            return -1;
        }
    }
}
