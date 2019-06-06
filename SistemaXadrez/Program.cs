using System;
using Board;

namespace SistemaXadrez
{
    class Program
    {
        static void Main()
        {
            BoardClass board = new BoardClass(8, 8);
            board.PrintBoard();
            Console.ReadLine();
        }
    }
}
