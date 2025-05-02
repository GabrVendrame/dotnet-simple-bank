namespace dotnet_simple_bank.Common
{
    public static class CustomExceptions
    {
        public class SecretLengthException : Exception
        {
            public SecretLengthException() : base("Secret length is invalid. Must be 64 caracters (512 bits).") { }
            public SecretLengthException(string message) : base(message) { }
            public SecretLengthException(string message, Exception inner) : base(message, inner) { }
        }

        public class NullOrEmptyEmailExcepetion : Exception
        {
            public NullOrEmptyEmailExcepetion() : base("Email is null or empty.") { }
            public NullOrEmptyEmailExcepetion(string message) : base(message) { }
            public NullOrEmptyEmailExcepetion(string message, Exception inner) : base(message, inner) { }
        }

        public class NullOrEmptyCpfCnpjException: Exception
        {
            public NullOrEmptyCpfCnpjException() : base("CpfCnpj is null or empty.") { }
            public NullOrEmptyCpfCnpjException(string message) : base(message) { }
            public NullOrEmptyCpfCnpjException(string message, Exception inner) : base(message, inner) { }
        }
    }
}
