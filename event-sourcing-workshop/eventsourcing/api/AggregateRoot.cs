namespace fr.soat.eventsourcing.api;

public abstract class AggregateRoot<TAggregateId>
{
    private readonly TAggregateId _id;
    public List<IEvent> Changes { get; }

    public AggregateRoot(TAggregateId id)
    {
        _id = id;
        Changes = new();
    }

    public TAggregateId GetId()
    {
        return _id;
    }

    public List<IEvent> GetChanges()
    {
        return new List<IEvent>(Changes);
    }

    protected void RecordChange(IEvent e)
    {
        Changes.Add(e);
    }

    public int Version()
    {
        return Changes.Count;
    }
}