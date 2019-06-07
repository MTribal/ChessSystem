using System;
using Board;
using Board.Enums;
using Board.Exceptions;
using Xadrez;

namespace SistemaXadrez
{
    class Match
    {
        private BoardClass Board;
        private int TotalTurns;
        private Color Turn;

        public Match()
        {
            Board = new BoardClass(8, 8);
            TotalTurns = 0;
            Turn = Color.White;
            InputPieces();
        }

        public void Atualize()
        {
            Board.PrintBoard();
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
