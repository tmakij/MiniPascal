namespace MiniPascal.Lexer.ScannerStates
{
    public interface IScannerState
    {
        IScannerState Read(TokenConstruction Current, char Read, StateStorage States);
    }
}
