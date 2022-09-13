
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class AccountRepository {

    private IEventStore eventStore;

    public AccountRepository(IEventStore eventStore) {
        this.eventStore = eventStore;
    }

    public void save(Account account) {
        AccountId aggregateId = account.GetId();
        eventStore.Store(aggregateId, account.GetChanges());
    }

    public Account load(AccountId accountId) {
        List<AccountEvent> events = asAccountEvents(eventStore.LoadEvents(accountId));
        return hydrate(accountId, events);
    }

    private static Account hydrate(AccountId accountId, List<AccountEvent> events) {
        Account account = new Account(accountId);
        foreach (var evt in events)
        {
            evt.ApplyOn(account);
        }
        return account;
    }

    private List<AccountEvent> asAccountEvents(List<IEvent> events) {
        return events.Select(evt => (AccountEvent) evt).ToList();
    }
}
