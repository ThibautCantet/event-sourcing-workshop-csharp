using fr.soat.eventsourcing.api;

namespace fr.soat.eventsourcing.eventpublisher;

public interface IListener
{
    void OnMessage(IEvent msg);
}