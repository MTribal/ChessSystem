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
        private Position Enpassant;
        private Position EnpassantCapture;
        public int TotalTurns { get; private set; }
        public Dictionary<string, int> Points { get; private set; }
        public Color Turn { get; private set; }
        public Status Status { get; private set; }

        public Match()
        {
            Board = new BoardClass(8, 8);
            TotalTurns = 0;
            Turn = Color.White;
            Enpassant = null;
            EnpassantCapture = null;
            InputPieces();
            Status = Status.Started;
            Points = new Dictionary<string, int>() { { "White", 0 }, { "Black", 0 } };
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
                if (piece is Pawn)
                {
                    Position pOrigin = posOrigin.ToPosition();
                    Position pDestin = posDestin.ToPosition();
                    if (EnpassantCapture != null)
                    {
                        if (pDestin.Equals(EnpassantCapture))
                        {
                            Board.RemovePiece(Enpassant);
                        }
                    }
                    if (pOrigin.Column - pDestin.Column == 2 || pDestin.Column - pOrigin.Column == 2)
                    {
                        Enpassant = posDestin.ToPosition();
                    }
                    else
                        Enpassant = null;
                }
                else Enpassant = null;
                int points;
                Board.InputPiece(piece, posDestin, out points);
                if (Turn == Color.White) Points[Color.White.ToString()] += points;
                else Points[Color.Black.ToString()] += points;
                piece.QttMovements++;
                TotalTurns++;
                if (Turn == Color.White) Turn = Color.Black;
                else Turn = Color.White;
            }
            else
            {
                throw new BoardException("Invalid destin position.");
            }
        }

        private bool MoveValidation(XadrezPosition posOrigin, XadrezPosition posDestin)
        {
            Piece piece = Board.GetPiece(posOrigin.ToPosition());
            if (piece == null)
            {
                throw new BoardException("Invalid origin position.");
            }
            if (piece.Color != Turn)
            {
                throw new BoardException("Invalid origin position.");
            }

            HashSet<Position> possibleMoves = GetValidMoves(piece);
            return posDestin.ToPosition().Equals(possibleMoves);
        }

        public HashSet<Position> GetValidMoves(Position pos)
        {
            if (Board.HasPiece(pos))
            {
                return GetValidMoves(Board.GetPiece(pos));
            }
            return new HashSet<Position>();
        }

        private HashSet<Position> GetValidMoves(Piece piece)
        {
            if (piece is Pawn)
                return PawnValidMoves(piece);
            else if (piece is Bishop)
                return BishopValidMoves(piece);
            else if (piece is Tower)
                return TowerValidMoves(piece);
            else if (piece is Queen)
                return QueenValidMoves(piece);
            else
                return new HashSet<Position>();
        }

        private HashSet<Position> PawnValidMoves(Piece pawn)
        {
            HashSet<Position> possibleMoves = new HashSet<Position>();
            if (Enpassant != null)
            {
                if ((sbyte)(pawn.Position.Line - Enpassant.Line) == 1 && pawn.Position.Column == Enpassant.Column)
                {
                    if (pawn.Color == Color.White)
                    {
                        Position p = new Position(Enpassant.Line, Enpassant.Column - 1);
                        possibleMoves.Add(p);
                        EnpassantCapture = p;
                    }
                    else
                    {
                        Position p = new Position(Enpassant.Line, Enpassant.Column + 1);
                        possibleMoves.Add(p);
                        EnpassantCapture = p;
                    }
                }
                else
                    EnpassantCapture = null;
            }
            else
                EnpassantCapture = null;

            if (pawn.Color == Color.White)
            {
                Position p1 = new Position(pawn.Position.Line, pawn.Position.Column - 1);
                if (ValidatePos(p1))
                {
                    possibleMoves.Add(p1);
                }
                if (pawn.QttMovements == 0)
                {
                    Position p2 = new Position(pawn.Position.Line, pawn.Position.Column - 2);
                    if (ValidatePos(p2))
                    {
                        possibleMoves.Add(p2);
                    }
                }
                Position p3 = new Position(pawn.Position.Line + 1, pawn.Position.Column - 1);
                if (Board.InternalValidatePos(p3))
                {
                    Piece piece3 = Board.GetPiece(p3);
                    if (piece3 != null && piece3.Color != pawn.Color)
                    {
                        possibleMoves.Add(piece3.Position);
                    }
                }
                Position p4 = new Position(pawn.Position.Line - 1, pawn.Position.Column - 1);
                if (Board.InternalValidatePos(p4))
                {
                    Piece piece4 = Board.GetPiece(p4);
                    if (piece4 != null && piece4.Color != pawn.Color)
                    {
                        possibleMoves.Add(piece4.Position);
                    }
                }
            }
            else
            {
                Position p1 = new Position(pawn.Position.Line, pawn.Position.Column + 1);
                if (ValidatePos(p1))
                {
                    possibleMoves.Add(p1);
                }
                if (pawn.QttMovements == 0)
                {
                    Position p2 = new Position(pawn.Position.Line, pawn.Position.Column + 2);
                    if (ValidatePos(p2))
                    {
                        possibleMoves.Add(p2);
                    }
                }
                Position p3 = new Position(pawn.Position.Line + 1, pawn.Position.Column + 1);
                if (Board.InternalValidatePos(p3))
                {
                    Piece piece3 = Board.GetPiece(p3);
                    if (piece3 != null && piece3.Color != pawn.Color)
                    {
                        possibleMoves.Add(piece3.Position);
                    }
                }
                Position p4 = new Position(pawn.Position.Line - 1, pawn.Position.Column + 1);
                if (Board.InternalValidatePos(p4))
                {
                    Piece piece4 = Board.GetPiece(p4);
                    if (piece4 != null && piece4.Color != pawn.Color)
                    {
                        possibleMoves.Add(piece4.Position);
                    }
                }
            }

            return possibleMoves;
        }

        private HashSet<Position> BishopValidMoves(Piece bishop)
        {
            HashSet<Position> possibleMoves = new HashSet<Position>();
            int line = bishop.Position.Line;
            int column = bishop.Position.Column;
            while (true) 
            {
                line++;
                column++;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == bishop.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra -> e Pra baixo

            line = bishop.Position.Line;
            column = bishop.Position.Column;
            while (true) 
            {
                line--;
                column++;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == bishop.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra <- e Pra baixo

            line = bishop.Position.Line;
            column = bishop.Position.Column;
            while (true)
            {
                line--;
                column--;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == bishop.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra <- e Pra Cima

            line = bishop.Position.Line;
            column = bishop.Position.Column;
            while (true)
            {
                line++;
                column--;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == bishop.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra -> e Pra Cima

            return possibleMoves;
        }

        private HashSet<Position> TowerValidMoves(Piece tower)
        {
            HashSet<Position> possibleMoves = new HashSet<Position>();
            int line = tower.Position.Line;
            int column = tower.Position.Column;
            while (true)
            {
                line++;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == tower.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra ->

            line = tower.Position.Line;
            column = tower.Position.Column;
            while (true)
            {
                column++;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == tower.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra baixo

            line = tower.Position.Line;
            column = tower.Position.Column;
            while (true)
            {
                line--;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == tower.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra <-

            line = tower.Position.Line;
            column = tower.Position.Column;
            while (true)
            {
                column--;
                Position pos = new Position(line, column);
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece != null)
                    {
                        if (piece.Color == tower.Color)
                            break;
                        possibleMoves.Add(new Position(line, column));
                        break;
                    }
                    possibleMoves.Add(new Position(line, column));
                }
                else
                    break;
            }  // Pra Cima

            return possibleMoves;
        }

        private HashSet<Position> QueenValidMoves(Piece queen)
        {
            HashSet<Position> possibleMoves = BishopValidMoves(queen);
            possibleMoves.UnionWith(TowerValidMoves(queen));
            return possibleMoves;
        }

        public void PrintBoard(HashSet<Position> positions)
        {
            Board.PrintBoard(positions);
        }

        private bool ValidatePos(Position pos)
        {
            return (Board.GetPiece(pos) == null);
        }

        public bool ValidateMoveTurn(Position pos)
        {
            return (Board.GetPiece(pos).Color == Turn);
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
