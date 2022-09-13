
//@EqualsAndHashCode

using fr.soat.eventsourcing.api;

public class ConferenceName : IAggregateId {

    private String _name;

    private ConferenceName(string name)
    {
        _name = name;
    }

    public static ConferenceName next() {
        return new ConferenceName(AggregateIdGenerator.GetAndIncrement().ToString());
    }

    public static ConferenceName name(String name) {
        return new ConferenceName(name);
    }

    public override string ToString() {
        return _name;
    }
}
