using System.IO;

namespace MiniPL
{
    public sealed class SourceStream
    {
        private readonly TextReader source;
        private char current;
        private bool endofStream;

        public SourceStream(TextReader Source)
        {
            source = Source;
        }

        public bool EndOfStream { get { return endofStream; } }

        public char Current { get { return current; } }

        public void MoveNext()
        {
            int curr = source.Read();
            if (curr == -1)
            {
                endofStream = true;
                current = char.MinValue;
                return;
            }
            current = (char)curr;
        }
    }
}
