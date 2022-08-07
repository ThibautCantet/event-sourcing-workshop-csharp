using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class AccountRepository
{
    private IEventStore _eventStore;

    public AccountRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public void Save(Account account)
    {
        // 1. retrieve all the pending changes recorded from the account aggregate
        // 2. invoke eventStore to save these changes (events)
        _eventStore.Store(account.GetId(), account.GetChanges());
    }

    public Account Load(AccountId accountId)
    {
        // 1. load from eventStore all the past events for the given account
        List<AccountEvent> events = AsAccountEvents(_eventStore.LoadEvents(accountId));
        // 2. hydrate Account to retrieve the current state
        return Hydrate(accountId, events);
    }

    private static Account Hydrate(AccountId accountId, List<AccountEvent> events)
    {
        //Apply all events on a new Account to retrieve the current state
        Account account = new Account(accountId);
        foreach (var accountEvent in events) {
            accountEvent.ApplyOn(account);            
        }
        return account;
    }

    private List<AccountEvent> AsAccountEvents(List<IEvent> events)
    {
        return events.Select(e => (AccountEvent)e).ToList();
    }
}