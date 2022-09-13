using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public abstract class OrderEvent : IEvent {
    public OrderId OrderId { get; }

    private OrderId orderId;

    protected OrderEvent(OrderId orderId)
    {
        OrderId = orderId;
    }

    public IAggregateId GetAggregateId() {
        return orderId;
    }

    public abstract void ApplyOn(Order order);

}
