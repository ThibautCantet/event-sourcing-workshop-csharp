using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class Order : AggregateRoot<OrderId> {
    public OrderStatus Status { get; }
    public ConferenceName ConferenceName { get; private set; }
    public AccountId AccountId { get; private set; }

    public Seat Seat { get; private set; }
    public PaymentReference PaymentReference { get; private set; }

    public Order(OrderId orderId) : base(orderId) 
    {
        Status = OrderStatus.NEW;
    }

    //@DecisionFunction
    public Order requestBooking(ConferenceName conferenceName, AccountId accountForPayment) {
        Apply(new OrderRequested(GetId(), conferenceName, accountForPayment));
        return this;
    }

    //@EvolutionFunction
    public void Apply(OrderRequested orderRequested) {
        //FIXME
        // should init the state of order (accountId, conferenceName)
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public Order assign(Seat bookedSeat) {
        //TODO(FIXME)
        //  expected output event is:
        // - OrderSeatBooked
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(OrderSeatBooked orderSeatBooked) {
        //TODO(FIXME)
        // should update state (order status and assigned seat)
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public void failSeatBooking() {
        //TODO(FIXME)
        //  expected output event is:
        // - OrderSeatBookingFailed
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(OrderSeatBookingFailed orderSeatBookingFailed) {
        //TODO(FIXME)
        // should update state:
        // - order status
        // - (no) assigned seat
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public void confirmPayment(PaymentReference paymentReference) {
        //TODO(FIXME)
        //  expected output event is:
        // - OrderPaid
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(OrderPaid orderPaid) {
        //TODO(FIXME)
        // should update state:
        // - order status
        // - the payment reference
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public void refusePayment() {
        //TODO(FIXME)
        //  expected output event is:
        // - OrderPaymentRefused
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(OrderPaymentRefused orderPaymentRefused) {
        //TODO(FIXME)
        // should update state:
        // - order status
        // - (no) payment reference
        // - but also the fact the is NO more assigned seat !
        throw new Exception("implement me !");
    }

}
