namespace fr.soat.banking.domain;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(String msg) : base(msg)
    {
    }
}