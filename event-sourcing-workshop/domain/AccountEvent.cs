using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public abstract class AccountEvent : IEvent
{
    private readonly AccountId _accountId;

    protected AccountEvent(AccountId id)
    {
        _accountId = id;
    }

    public IAggregateId GetAggregateId()
    {
        return _accountId;
    }

    public abstract void ApplyOn(Account account);
}