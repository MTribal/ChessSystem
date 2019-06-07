using System;
using Board;
using Board.Enums;
using Board.Exceptions;
using Xadrez;

namespace SistemaXadrez
{
    sealed class Program
    {
        static void Main()
        {
            Match match = new Match();
            while (match.Status != Status.Finished)
            {
                try
                {
                    match.Atualize();
                    Console.WriteLine();
                    string orig = Input("Origin: ");
                    string dest = Input("Origin: ");
                    XadrezPosition origin = new XadrezPosition(orig[0], int.Parse(orig[1].ToString()));
                    XadrezPosition destin = new XadrezPosition(dest[0], int.Parse(dest[1].ToString()));
                    match.MovePiece(origin, destin);
                }
                catch (BoardException e)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
                finally
                {
                    Console.Clear();
                }
            }
        }

        static string Input(string frase)
        {
            Console.Write(frase);
            return Console.ReadLine();
        }
    }
}
