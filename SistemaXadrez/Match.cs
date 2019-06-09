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
        private Position MinRock;
        private Position MaxRock;
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
            MinRock = null;
            MaxRock = null;
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
                Position pOrigin = posOrigin.ToPosition();
                Position pDestin = posDestin.ToPosition();
                if (piece is Pawn)
                {
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
                    if (pDestin.Column == 0 || pDestin.Column == 7)
                    {
                        piece = new Queen(piece.Color, Board);
                        if (Turn == Color.White) Points[Color.White.ToString()] += 9;
                        else Points[Color.Black.ToString()] += 9;
                    }
                }
                else Enpassant = null;
                if (piece is King)
                {
                    if (pDestin.Equals(MinRock))
                    {
                        Piece tower = Board.RemovePiece(new Position(pDestin.Line + 1, pDestin.Column));
                        Board.InputPiece(tower, new Position(pDestin.Line - 1, pDestin.Column));
                    }
                    else if (pDestin.Equals(MaxRock))
                    {
                        Piece tower = Board.RemovePiece(new Position(pDestin.Line - 2, pDestin.Column));
                        Board.InputPiece(tower, new Position(pDestin.Line + 1, pDestin.Column));
                    }
                }
                else
                {
                    MinRock = null;
                    MaxRock = null;
                }
                int points;
                Board.InputPiece(piece, posDestin.ToPosition(), out points);
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
            else if (piece is Knigth)
                return KnightValidMoves(piece);
            else if (piece is King)
                return KingValidMoves(piece, false);
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

        private HashSet<Position> KnightValidMoves(Piece knight)
        {
            HashSet<Position> possibleMoves = new HashSet<Position>();
            int line = knight.Position.Line;
            int column = knight.Position.Column;
            HashSet<Position> pendentMoves = new HashSet<Position>();
            pendentMoves.Add(new Position(line + 1, column - 2));
            pendentMoves.Add(new Position(line - 1, column - 2));
            pendentMoves.Add(new Position(line + 2, column - 1));
            pendentMoves.Add(new Position(line + 2, column + 1));
            pendentMoves.Add(new Position(line - 2, column - 1));
            pendentMoves.Add(new Position(line - 2, column + 1));
            pendentMoves.Add(new Position(line + 1, column + 2));
            pendentMoves.Add(new Position(line - 1, column + 2));
            foreach (Position pos in pendentMoves)
            {
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece == null || piece.Color != knight.Color)
                        possibleMoves.Add(pos);
                }
            }
            return possibleMoves;
        }

        private HashSet<Position> KingValidMoves(Piece king, bool gambiarra)
        {
            int line = king.Position.Line;
            int column = king.Position.Column;
            HashSet<Position> possibleMoves = new HashSet<Position>();
            HashSet<Position> pendentMoves = new HashSet<Position>
            {
                new Position(line + 1, column + 1),
                new Position(line - 1, column + 1),
                new Position(line + 1, column - 1),
                new Position(line - 1, column- 1),
                new Position(line, column + 1),
                new Position(line, column - 1),
                new Position(line - 1, column),
                new Position(line + 1, column),
            };
            foreach (Position pos in pendentMoves)
            {
                if (Board.InternalValidatePos(pos))
                {
                    Piece piece = Board.GetPiece(pos);
                    if (piece == null || piece.Color != king.Color)
                        possibleMoves.Add(pos);
                }
            }
            Piece minRock3 = Board.GetPiece(new Position(line + 3, column));
            Piece minRock2 = Board.GetPiece(new Position(line + 2, column));
            Piece minRock1 = Board.GetPiece(new Position(line + 1, column));

            Piece maxRock4 = Board.GetPiece(new Position(line - 4, column));
            Piece maxRock3 = Board.GetPiece(new Position(line - 3, column));
            Piece maxRock2 = Board.GetPiece(new Position(line - 2, column));
            Piece maxRock1 = Board.GetPiece(new Position(line - 1, column));
            if (gambiarra == false)
            {
                if (king.QttMovements == 0 && minRock1 == null && minRock2 == null &&
                            minRock3 is Tower && minRock3.QttMovements == 0 &&
                            !(ExisteChecks(new Position(line + 1, column), king.Color)) &&
                            !(ExisteChecks(new Position(line + 2, column), king.Color)))
                {
                    MinRock = new Position(line + 2, column);
                    possibleMoves.Add(MinRock);
                }
                else
                    MinRock = null;
                if (king.QttMovements == 0 && maxRock1 == null && maxRock2 == null & maxRock3 == null
                            && maxRock4 is Tower && maxRock4.QttMovements == 0 &&
                            !(ExisteChecks(new Position(line - 1, column), king.Color)) &&
                            !(ExisteChecks(new Position(line - 2, column), king.Color)))
                {
                    MaxRock = new Position(line - 2, column);
                    possibleMoves.Add(MaxRock);
                }
                else
                    MaxRock = null;
            }
            return possibleMoves;
        }

        private bool ExisteChecks(Position king, Color color)
        {
            HashSet<Position> possibleMoves;
            for (int c = 0; c < Board.Lines; c++)
            {
                for (int c2 = 0; c2 < Board.Columns; c2++)
                {
                    Piece piece = Board.GetPiece(new Position(c, c2));
                    if (piece != null)
                    {
                        if (piece.Color != color)
                        {
                            if (piece is Pawn)
                                possibleMoves = PawnValidMoves(piece);
                            else if (piece is Bishop)
                                possibleMoves = BishopValidMoves(piece);
                            else if (piece is Tower)
                                possibleMoves = TowerValidMoves(piece);
                            else if (piece is Queen)
                                possibleMoves = QueenValidMoves(piece);
                            else if (piece is Knigth)
                                possibleMoves = KnightValidMoves(piece);
                            else if (piece is King)
                                possibleMoves = KingValidMoves(piece, true);
                            else
                                possibleMoves = new HashSet<Position>();
                        }
                        else
                            possibleMoves = new HashSet<Position>();

                        if (king.Equals(possibleMoves))
                            return true;
                    }
                }
            }
            return false;
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
            Board.InputPiece(new Tower(Color.White, Board), new XadrezPosition('a', 1).ToPosition());
            Board.InputPiece(new Knigth(Color.White, Board), new XadrezPosition('b', 1).ToPosition());
            Board.InputPiece(new Bishop(Color.White, Board), new XadrezPosition('c', 1).ToPosition());
            Board.InputPiece(new Queen(Color.White, Board), new XadrezPosition('d', 1).ToPosition());
            Board.InputPiece(new King(Color.White, Board), new XadrezPosition('e', 1).ToPosition());
            Board.InputPiece(new Bishop(Color.White, Board), new XadrezPosition('f', 1).ToPosition());
            Board.InputPiece(new Knigth(Color.White, Board), new XadrezPosition('g', 1).ToPosition());
            Board.InputPiece(new Tower(Color.White, Board), new XadrezPosition('h', 1).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('a', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('b', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('c', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('d', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('e', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('f', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('g', 2).ToPosition());
            Board.InputPiece(new Pawn(Color.White, Board), new XadrezPosition('h', 2).ToPosition());

            Board.InputPiece(new Tower(Color.Black, Board), new XadrezPosition('a', 8).ToPosition());
            Board.InputPiece(new Knigth(Color.Black, Board), new XadrezPosition('b', 8).ToPosition());
            Board.InputPiece(new Bishop(Color.Black, Board), new XadrezPosition('c', 8).ToPosition());
            Board.InputPiece(new Queen(Color.Black, Board), new XadrezPosition('d', 8).ToPosition());
            Board.InputPiece(new King(Color.Black, Board), new XadrezPosition('e', 8).ToPosition());
            Board.InputPiece(new Bishop(Color.Black, Board), new XadrezPosition('f', 8).ToPosition());
            Board.InputPiece(new Knigth(Color.Black, Board), new XadrezPosition('g', 8).ToPosition());
            Board.InputPiece(new Tower(Color.Black, Board), new XadrezPosition('h', 8).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('a', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('b', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('c', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('d', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('e', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('f', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('g', 7).ToPosition());
            Board.InputPiece(new Pawn(Color.Black, Board), new XadrezPosition('h', 7).ToPosition());
        }
    }
}
