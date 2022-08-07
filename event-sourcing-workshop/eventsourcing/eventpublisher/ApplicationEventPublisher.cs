namespace fr.soat.eventsourcing.eventpublisher;

public class ApplicationEventPublisher
{
    public ApplicationEventPublisher()
    {
    }

    private List<IListener> subs = new();

    public void Subscribe(IListener l) {
        subs.Add(l);
    }

    public void PublishEvent(Object msg) {
        foreach (IListener l in subs) {
            l.OnMessage(msg);
        }
    }
}