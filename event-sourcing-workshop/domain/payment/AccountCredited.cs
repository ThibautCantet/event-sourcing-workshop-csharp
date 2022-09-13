namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class AccountCredited : AccountEvent, IEquatable<AccountCredited>
{
    public int Amount { get; }

    public AccountCredited(AccountId accountId, int amount) : base(accountId)
    {
        Amount = amount;
    }
    
    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }

    public bool Equals(AccountCredited? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AccountCredited)obj);
    }

    public override int GetHashCode()
    {
        return Amount;
    }
}