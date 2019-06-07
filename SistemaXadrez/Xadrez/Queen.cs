using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Queen : Piece
    {
        public Queen(Color color, BoardClass board) : base(color, board)
        {
            Value = 9;
        }

        public override string ToString()
        {
            return "Q ";
        }
    }
}