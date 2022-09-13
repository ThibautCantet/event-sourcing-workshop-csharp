using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class OrderRepository {

    private IEventStore eventStore;

    public OrderRepository(IEventStore eventStore) {
        this.eventStore = eventStore;
    }

    public void save(Order order) {
        OrderId aggregateId = order.GetId();
        eventStore.Store(aggregateId, order.GetChanges());
    }

    public Order load(OrderId orderId) {
        List<OrderEvent> events = asOrderEvents(eventStore.LoadEvents(orderId));
        return hydrate(orderId, events);
    }

    private static Order hydrate(OrderId orderId, List<OrderEvent> events) {
        Order order = new Order(orderId);
        foreach (var evt in events)
        {
            evt.ApplyOn(order);
        }
        return order;
    }

    private List<OrderEvent> asOrderEvents(List<IEvent> events) {
        return events.Select(evt => (OrderEvent) evt).ToList();
    }
}
