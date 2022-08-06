namespace fr.soat.eventsourcing.api;

public interface IEvent
{
    IAggregateId GetAggregateId();
}