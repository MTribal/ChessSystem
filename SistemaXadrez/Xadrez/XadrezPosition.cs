using Board;

namespace Xadrez
{
    class XadrezPosition
    {
        public char Column { get; set; }
        public int Line { get; set; }

        public XadrezPosition(char column, int line)
        {
            Column = column;
            Line = line;
        }

        public Position ToPosition()
        {
            return new Position(Column - 'a', 8 - Line);
        }
    }
}
