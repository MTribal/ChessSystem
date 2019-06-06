using System;

namespace Board.Exceptions
{
    public class BoardException : ApplicationException
    {
        public BoardException(string message) : base(message)
        {
        }
    }
}
