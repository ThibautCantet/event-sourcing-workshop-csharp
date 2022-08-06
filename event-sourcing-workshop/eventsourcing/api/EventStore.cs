namespace fr.soat.eventsourcing.api;

public interface IEventStore
{
    List<IEvent> LoadEvents(IAggregateId aggregateId);
    void Store(IAggregateId aggregateId, List<IEvent> events);
    void Clear();
}