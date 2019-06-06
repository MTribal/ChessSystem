using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Pawn : Piece
    {
        public Pawn(Color color, BoardClass board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "P ";
        }
    }
}