using System;
using Board;
using Board.Enums;
using Board.Exceptions;
using Xadrez;

namespace SistemaXadrez
{
    class Program
    {
        static void Main()
        {
            try
            {
                BoardClass board = new BoardClass(8, 8);
                Piece tower1W = new Tower(Color.White, board);
                Piece kingW = new King(Color.White, board);
                board.InputPiece(tower1W, new Position(0, 0));
                board.InputPiece(kingW, new Position(1, 0));
                board.PrintBoard();
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
