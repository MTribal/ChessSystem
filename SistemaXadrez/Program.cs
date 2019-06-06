using System;
using Board;
using Board.Enums;
using Xadrez;

namespace SistemaXadrez
{
    class Program
    {
        static void Main()
        {
            BoardClass board = new BoardClass(8, 8);
            Piece tower1 = new Tower(Color.White, board);
            board.InputPiece(tower1, new Position(0, 0));
            board.PrintBoard();

            Console.ReadLine();
        }
    }
}
