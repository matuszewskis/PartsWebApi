namespace PartsWebApi.Utils.Exceptions
{
    using System;

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
