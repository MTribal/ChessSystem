using Board.Enums;
using Board;

namespace Board
{
    class Piece
    {
        public Position Position { get; set; }
        public Color MyProperty { get; protected set; }
        public int QttMovements { get; protected set; }
        public BoardClass Board { get; protected set; }

        public Piece(Position position, Color myProperty, BoardClass board)
        {
            Position = position;
            MyProperty = myProperty;
            Board = board;
            QttMovements = 0;
        }

    }
}
