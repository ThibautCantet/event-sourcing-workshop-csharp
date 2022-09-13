using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public abstract class AccountEvent : IEvent
{
    public AccountId AccountId { get; }

    protected AccountEvent(AccountId id)
    {
        AccountId = id;
    }

    public IAggregateId GetAggregateId()
    {
        return AccountId;
    }

    public abstract void ApplyOn(Account account);
}