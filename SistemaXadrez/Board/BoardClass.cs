using System;
using System.Collections.Generic;
using Xadrez;
using Board.Enums;
using Board.Exceptions;

namespace Board
{
    sealed class BoardClass
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

        internal bool InternalValidatePos(Position pos)
        {
            if (pos.Line < 0 || pos.Line >= Lines || pos.Column < 0 || pos.Column >= Columns)
            {
                return false;
            }
            return true;
        }

        public void InputPiece(Piece piece, XadrezPosition xadrezPos, out int points)
        {
            Position pos = xadrezPos.ToPosition();
            if (HasPiece(pos))
            {
                Piece hasPiece = GetPiece(pos);
                if (hasPiece.Color == piece.Color)
                {
                    points = 0;
                    throw new BoardException("Already exist a piece in that position.");
                }
                else
                {
                    Piece aux = GetPiece(pos);
                    if (aux.Color == Color.White)
                    {
                        points = aux.Value;
                    }
                    else
                    {
                        points = aux.Value;
                    }
                    RemovePiece(pos);
                }
            }
            else
            {
                points = 0;
            }
            piece.Position = pos;
            Pieces[pos.Line, pos.Column] = piece;
        }

        public void InputPiece(Piece piece, XadrezPosition xadrezPos)
        {
            Position pos = xadrezPos.ToPosition();
            if (HasPiece(pos))
            {
                Piece hasPiece = GetPiece(pos);
                if (hasPiece.Color == piece.Color) throw new BoardException("Already exist a piece in that position.");
                else
                {
                    RemovePiece(pos);
                }
            }
            piece.Position = pos;
            Pieces[pos.Line, pos.Column] = piece;
        }

        public Piece RemovePiece(Position pos)
        {
            Piece auxPiece = GetPiece(pos);
            auxPiece.Position = null;
            Pieces[pos.Line, pos.Column] = null;
            return auxPiece;
        }

        public Piece GetPiece(Position pos)
        {
            PosValidation(pos);
            return Pieces[pos.Line, pos.Column];
        }

        public void PrintBoard()
        {
            for (int c = 0; c < Columns; c++)
            {
                Console.Write(8 - c + " ");
                for (int c2 = 0; c2 < Lines; c2++)
                {
                    if (Pieces[c2, c] == null) Console.Write("- ");
                    else PrintPiece(Pieces[c2, c]);
                }
                Console.WriteLine();
                if (c == Columns - 1)
                {
                    Console.WriteLine("  A B C D E F G H");
                }
            }
        }

        public void PrintPiece(Piece piece)
        {
            if (piece.Color == Enums.Color.White)
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(piece);
                Console.ForegroundColor = aux;
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(piece);
                Console.ForegroundColor = aux;
            }
        }
    }
}
