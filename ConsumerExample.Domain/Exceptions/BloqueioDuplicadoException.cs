namespace ConsumerExample.Domain.Exceptions
{
    public class BloqueioDuplicadoException : Exception
    {
        public BloqueioDuplicadoException()
        {
        }

        public BloqueioDuplicadoException(string? message) : base(message)
        {
        }
    }
}
