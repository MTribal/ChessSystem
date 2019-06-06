using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Queen : Piece
    {
        public Queen(Color color, BoardClass board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "R ";
        }
    }
}