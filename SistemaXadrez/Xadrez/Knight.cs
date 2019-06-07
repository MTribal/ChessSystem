using Board;
using Board.Enums;

namespace Xadrez
{
    sealed class Knigth : Piece
    {
        public Knigth(Color color, BoardClass board) : base(color, board)
        {
            Value = 3;
        }

        public override string ToString()
        {
            return "N ";
        }
    }
}