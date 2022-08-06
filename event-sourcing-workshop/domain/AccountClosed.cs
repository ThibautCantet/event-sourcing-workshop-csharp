namespace fr.soat.banking.domain;

public class AccountClosed : AccountEvent, IEquatable<AccountClosed>
{
    public AccountClosed(AccountId accountId) : base(accountId)
    {
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AccountClosed)obj);
    }

    public override int GetHashCode()
    {
        return int.Parse(((AccountId)GetAggregateId()).Value);
    }

    public bool Equals(AccountClosed? other)
    {
        return GetAggregateId() == other.GetAggregateId();
    }
}