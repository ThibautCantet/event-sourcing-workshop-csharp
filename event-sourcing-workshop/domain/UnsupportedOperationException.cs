namespace fr.soat.banking.domain;

public class UnsupportedOperationException : Exception
{
    public UnsupportedOperationException(string? message) : base(message)
    {
    }
}