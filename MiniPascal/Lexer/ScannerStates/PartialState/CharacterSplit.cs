using System;
using System.Collections.Generic;

namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class CharacterSplit : IScannerState
    {
        private readonly Dictionary<char, IScannerState> moves = new Dictionary<char, IScannerState>();

        public CharacterSplit(params Tuple<char, IScannerState>[] Moves)
        {
            foreach (Tuple<char, IScannerState> Current in Moves)
            {
                moves.Add(Current.Item1, Current.Item2);
            }
        }

        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (moves.ContainsKey(Read))
            {
                Current.Append(Read);
                return moves[Read];
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
