﻿using System.Runtime.Serialization;

namespace CartingService.Application.Exceptions
{


    [Serializable]
    public class InvalidItemException : Exception
    {
        public InvalidItemException()
        {
        }

        public InvalidItemException(string? message) : base(message)
        {
        }

        public InvalidItemException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}