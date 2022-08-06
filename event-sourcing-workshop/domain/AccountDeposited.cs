namespace fr.soat.banking.domain;

public class AccountDeposited : AccountEvent, IEquatable<AccountDeposited>
{
    public int Amount { get; }

    public AccountDeposited(AccountId accountId, int amount) : base(accountId)
    {
        Amount = amount;
    }

    public int GetAmount()
    {
        return Amount;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }

    public bool Equals(AccountDeposited? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AccountDeposited)obj);
    }

    public override int GetHashCode()
    {
        return Amount;
    }
}