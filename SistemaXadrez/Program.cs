using System;
using System.Collections.Generic;
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
                    Console.WriteLine("Turn nº: " + match.TotalTurns + "\n");
                    Console.WriteLine("White points: " + match.Points[Color.White.ToString()]);
                    Console.WriteLine("Black points: " + match.Points[Color.Black.ToString()] + "\n");
                    Console.WriteLine("Turn: " + match.Turn + "\n");
                    string orig = Input("Origin: ");
                    XadrezPosition origin = new XadrezPosition(orig[0], int.Parse(orig[1].ToString()));
                    HashSet<Position> posssibleMoves = match.GetValidMoves(origin.ToPosition());
                    if (posssibleMoves.Count == 0)
                    {
                        throw new BoardException("Invalid origin position.");
                    }
                    if (!match.ValidateMoveTurn(origin.ToPosition()))
                    {
                        throw new BoardException($"It's {match.Turn}'s turn.");
                    }
                    Console.Clear();
                    match.PrintBoard(posssibleMoves);
                    Console.WriteLine();
                    string dest = Input("Destine: ");
                    XadrezPosition destin = new XadrezPosition(dest[0], int.Parse(dest[1].ToString()));
                    match.MovePiece(origin, destin);
                }
                catch (BoardException e)
                {
                    Console.WriteLine("\n" + e.Message);
                    Console.ReadLine();
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("\nOrigin or destin invalid.");
                    Console.ReadLine();
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOrigin or denstin invalid.");
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
