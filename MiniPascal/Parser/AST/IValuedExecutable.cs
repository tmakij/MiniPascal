namespace MiniPL.Parser.AST
{
    public interface IValuedExecutable
    {
        ReturnValue Execute(Variables Global);
    }
}
