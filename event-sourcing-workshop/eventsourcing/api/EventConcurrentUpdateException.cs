namespace fr.soat.eventsourcing.api;

public class EventConcurrentUpdateException : Exception
{
    public EventConcurrentUpdateException(String msg) : base(msg)
    {
    }
}