using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;


public class ConferenceCommandHandler {

    private OrderRepository _repository;

    public ConferenceCommandHandler(OrderRepository repository) {
        _repository = repository;
    }

    public OrderId requestOrder(ConferenceName conferenceName, AccountId accountId) {
        Order order = OrderFactory.create();
        order.requestBooking(conferenceName, accountId);
        _repository.save(order);
        return order.GetId();
    }

}
