namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class AccountOpened : AccountEvent, IEquatable<AccountOpened>
{
    public String Owner { get; }
    public String Number { get; }

    public AccountOpened(AccountId id, String owner, String number) : base(id)
    {
        Owner = owner;
        Number = number;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }

    public bool Equals(AccountOpened? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Owner == other.Owner && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AccountOpened)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Owner, Number);
    }
}