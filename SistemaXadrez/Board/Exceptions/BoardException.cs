using System;

namespace Board.Exceptions
{
    public sealed class BoardException : ApplicationException
    {
        public BoardException(string message) : base(message)
        {
        }
    }
}
