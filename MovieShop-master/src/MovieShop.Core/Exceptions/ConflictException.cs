using System;

namespace MovieShop.Core.Exceptions
{
    public class ConflictException: Exception
    {
        public ConflictException(string message): base(message)
        {
            
        }
    }
}