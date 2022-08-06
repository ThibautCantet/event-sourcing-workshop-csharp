namespace fr.soat.eventsourcing.api;

public class AggregateIdGenerator
{
    private static int _value;

    public static int GetAndIncrement()
    {
        return _value++;
    }
}