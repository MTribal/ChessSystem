using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class King : Piece
    {
        public King(Color color, BoardClass board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "K ";
        }
    }
}
