using System;
using Board.Exceptions;

namespace Board
{
    class BoardClass
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        public BoardClass(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[lines, columns];
        }

        public bool HasPiece(Position pos)
        {
            PosValidation(pos);
            return (Pieces[pos.Line, pos.Column] != null);
        }

        public void PosValidation(Position pos)
        {
            if (pos.Line < 0 || pos.Line >= Lines || pos.Column < 0 || pos.Column >= Columns)
            {
                throw new BoardException("Invalid position.");
            }
        }

        public void InputPiece(Piece piece, Position pos)
        {
            if (HasPiece(pos)) throw new BoardException("Already exist a piece in that position."); 
            Pieces[pos.Line, pos.Column] = piece;
        }

        public void PrintBoard()
        {
            int number = Columns;
            for (int c = 0; c < Columns; c++)
            {
                Console.Write(number + " ");
                for (int c2 = 0; c2 < Lines; c2++)
                {
                    if (Pieces[c2, c] == null) Console.Write("- ");
                    else Console.Write(Pieces[c2, c]);
                }
                number--;
                Console.WriteLine();
                if (c == Columns - 1)
                {
                    Console.Write("  ");
                    for (int c3 = 0; c3 < Lines; c3++)
                    {
                        Console.Write(Alphabet.alphabet[c3] + " ");
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
