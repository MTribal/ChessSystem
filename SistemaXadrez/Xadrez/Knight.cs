using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Knigth : Piece
    {
        public Knigth(Color color, BoardClass board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "N ";
        }
    }
}