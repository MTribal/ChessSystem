using Board.Enums;
using Board;

namespace Board
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int QttMovements { get; set; }
        public BoardClass Board { get; protected set; }

        public Piece(Color color, BoardClass board)
        {
            Position = null;
            Color = color;
            Board = board;
            QttMovements = 0;
        }

    }
}
