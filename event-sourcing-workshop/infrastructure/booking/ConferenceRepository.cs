using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;


public class ConferenceRepository {

    private IEventStore eventStore;

    public ConferenceRepository(IEventStore eventStore) {
        this.eventStore = eventStore;
    }

    public void save(Conference conference) {
        ConferenceName aggregateId = conference.GetId();
        eventStore.Store(aggregateId, conference.GetChanges());
    }

    public Conference load(ConferenceName conferenceName) {
        List<ConferenceEvent> events = asRoomEvents(eventStore.LoadEvents(conferenceName));
        return hydrate(conferenceName, events);
    }

    private static Conference hydrate(ConferenceName conferenceName, List<ConferenceEvent> events) {
        Conference conference = new Conference(conferenceName);
        foreach (var evt in events)
        {
            evt.ApplyOn(conference);
        }
        return conference;
    }

    private List<ConferenceEvent> asRoomEvents(List<IEvent> events) {
        return events.Select(evt => (ConferenceEvent) evt).ToList();
    }
}
