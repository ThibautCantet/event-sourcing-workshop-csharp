namespace fr.soat.eventsourcing.eventpublisher;

public interface IListener
{
    void OnMessage(Object msg);
}