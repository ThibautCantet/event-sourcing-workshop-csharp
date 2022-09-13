using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class AccountId : IAggregateId
{
    public string Value { get; }

    private AccountId(string value)
    {
        Value = value;
    }

    public static AccountId Next()
    {
        return new AccountId(AggregateIdGenerator.GetAndIncrement().ToString());
    }

    public static AccountId From(String id)
    {
        return new AccountId(id);
    }

    public override string ToString()
    {
        return Value;
    }
}