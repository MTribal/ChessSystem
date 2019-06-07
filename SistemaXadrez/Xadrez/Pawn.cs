using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Pawn : Piece
    {
        public Pawn(Color color, BoardClass board) : base(color, board)
        {
            Value = 1;
        }

        public override string ToString()
        {
            return "P ";
        }
    }
}