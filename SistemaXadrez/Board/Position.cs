using System.Collections.Generic;

namespace Board
{
    sealed class Position
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                Position p = (Position) obj;
                return (Line == p.Line && Column == p.Column);
            }
            else
            {
                if (obj is HashSet<Position>)
                {
                    foreach (Position p in (HashSet<Position>) obj)
                    {
                        if (Line == p.Line && Column == p.Column)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string ToString()
        {
            return "(" + Line + ", " + Column + ")";
        }
    }
}
