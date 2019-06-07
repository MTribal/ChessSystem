using System;
using System.Collections.Generic;
using Board;
using Board.Enums;
using Board.Exceptions;
using Xadrez;

namespace SistemaXadrez
{
    sealed class Match
    {
        private BoardClass Board;
        private int TotalTurns;
        private Color Turn;
        public Status Status { get; private set; }

        public Match()
        {
            Board = new BoardClass(8, 8);
            TotalTurns = 0;
            Turn = Color.White;
            InputPieces();
            Status = Status.Started;
        }

        public void Atualize()
        {
            Board.PrintBoard();
        }

        public void MovePiece(XadrezPosition posOrigin, XadrezPosition posDestin)
        {
            if (MoveValidation(posOrigin, posDestin))
            {
                Piece piece = Board.RemovePiece(posOrigin.ToPosition());
                Board.InputPiece(piece, posDestin);
                piece.QttMovements++;
            }
            else
            {
                throw new BoardException("Invalid destin position.");
            }
        }

        private bool MoveValidation(XadrezPosition posOrigin, XadrezPosition posDestin)
        {
            if (Board.GetPiece(posOrigin.ToPosition()) == null)
            {
                throw new BoardException("Invalid origin position.");
            }
            HashSet<Position> possibleMoves = GetValidMoves(Board.GetPiece(posOrigin.ToPosition()));
            return posDestin.ToPosition().Equals(possibleMoves);
        }

        private HashSet<Position> GetValidMoves(Piece piece)
        {
            HashSet<Position> possibleMoves = new HashSet<Position>();
            if (piece.Color == Color.White)
            {
                if (piece is Pawn) // Pawn
                {
                    Position p1 = new Position(piece.Position.Line, piece.Position.Column - 1);
                    if (ValidatePos(p1))
                    {
                        possibleMoves.Add(p1);
                    }
                    if (piece.QttMovements == 0)
                    {
                        Position p2 = new Position(piece.Position.Line, piece.Position.Column - 2);
                        if (ValidatePos(p2))
                        {
                            possibleMoves.Add(p2);
                        }
                    }
                }
            }
            else
            {

            }
            return possibleMoves;
        }

        private bool ValidatePos(Position pos)
        {
            return (Board.GetPiece(pos) == null);
        }

        private void InputPieces()
        {
            Board.InputPiece(new Tower(Color.White, Board), new XadrezPosition('a', 1));
            Board.InputPiece(new Knigth(Color.White, Board), new XadrezPosition('b', 1));
            Board.InputPiece(new Bishop(Color.White, Board), new XadrezPosition('c', 1));
            Board.InputPiece(new Queen(Color.White, Board), new XadrezPosition('d', 1));
            Board.InputPiece(new King(Color.White, Board), new XadrezPosition('e', 1));
            Board.InputPiece(new Bishop(Color.White, Board), new XadrezPosition('f', 1));
            Board.InputPiece(new Knigth(Color.White, Board), new XadrezPosition('g', 1));
            Board.InputPiece(new Tower(Color.White, Board), new XadrezPosition('h', 1));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('a', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('b', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('c', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('d', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('e', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('f', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('g', 2));
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('h', 2));

            Board.InputPiece(new Tower(Color.Black, Board), new XadrezPosition('a', 8));
            Board.InputPiece(new Knigth(Color.Black, Board), new XadrezPosition('b', 8));
            Board.InputPiece(new Bishop(Color.Black, Board), new XadrezPosition('c', 8));
            Board.InputPiece(new Queen(Color.Black, Board), new XadrezPosition('d', 8));
            Board.InputPiece(new King(Color.Black, Board), new XadrezPosition('e', 8));
            Board.InputPiece(new Bishop(Color.Black, Board), new XadrezPosition('f', 8));
            Board.InputPiece(new Knigth(Color.Black, Board), new XadrezPosition('g', 8));
            Board.InputPiece(new Tower(Color.Black, Board), new XadrezPosition('h', 8));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('a', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('b', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('c', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('d', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('e', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('f', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('g', 7));
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('h', 7));
        }
    }
}
