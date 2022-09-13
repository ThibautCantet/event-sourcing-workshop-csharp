using fr.soat.banking.domain;
using fr.soat.eventsourcing.api;

public abstract class ConferenceEvent : IEvent {

    private ConferenceName conferenceName;

    public ConferenceEvent(ConferenceName conferenceName) {
        this.conferenceName = conferenceName;
    }

    public IAggregateId GetAggregateId() {
        return conferenceName;
    }

    public abstract void ApplyOn(Conference conference);

}
