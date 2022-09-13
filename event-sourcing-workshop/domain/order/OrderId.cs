using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain.order;

public class OrderId : IAggregateId
{
    public string Value { get; }

    private OrderId(string value)
    {
        Value = value;
    }

    public static OrderId Next()
    {
        return new OrderId(AggregateIdGenerator.GetAndIncrement().ToString());
    }

    public static OrderId From(String id)
    {
        return new OrderId(id);
    }

    public override string ToString()
    {
        return Value;
    }
};