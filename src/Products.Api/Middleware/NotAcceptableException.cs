namespace Products.Api.Middleware
{
    public class NotAcceptableException : Exception
    {
        public NotAcceptableException(string message)
            : base($"{message}")
        { }
    }
}
