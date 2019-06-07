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
                Match match = new Match();
                match.Atualize();
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
