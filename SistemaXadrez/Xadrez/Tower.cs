using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Tower : Piece
    {
        public Tower(Color color, BoardClass board) : base(color, board)
        {
            Value = 5;
        }

        public override string ToString()
        {
            return "R ";
        }
    }
}
