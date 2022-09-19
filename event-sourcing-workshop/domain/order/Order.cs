using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;


public class Order : AggregateRoot<OrderId> {
    public OrderStatus Status { get; private set; }
    public ConferenceName ConferenceName { get; private set; }
    public AccountId AccountId { get; private set; }

    public Seat Seat { get; private set; }
    public PaymentReference PaymentReference { get; private set; }

    public Order(OrderId orderId) : base(orderId) 
    {
        this.Status = OrderStatus.NEW;
    }

    //@DecisionFunction
    public Order requestBooking(ConferenceName conferenceName, AccountId accountForPayment) {
        Apply(new OrderRequested(GetId(), conferenceName, accountForPayment));
        return this;
    }

    //@EvolutionFunction
    public void Apply(OrderRequested orderRequested) {
        // should init the state of order (accountId, conferenceName)
        RecordChange(orderRequested);
        this.AccountId = orderRequested.AccountId;
        this.ConferenceName = orderRequested.ConferenceName;
    }

    //@DecisionFunction
    public Order assign(Seat bookedSeat) {
        //  expected output event is:
        // - OrderSeatBooked
        Apply(new OrderSeatBooked(GetId(), bookedSeat));
        return this;
    }

    //@EvolutionFunction
    public void Apply(OrderSeatBooked orderSeatBooked) {
        // should update state (order status and assigned seat)
        RecordChange(orderSeatBooked);
        Status = OrderStatus.SEAT_BOOKED;
        Seat = orderSeatBooked.BookedSeat;
    }

    //@DecisionFunction
    public void failSeatBooking() {
        //  expected output event is:
        // - OrderSeatBookingFailed
        Apply(new OrderSeatBookingFailed(GetId()));
    }

    //@EvolutionFunction
    public void Apply(OrderSeatBookingFailed orderSeatBookingFailed) {
        // should update state:
        // - order status
        // - (no) assigned seat
        RecordChange(orderSeatBookingFailed);
        Status = OrderStatus.SEAT_BOOKING_FAILED;
        Seat = null;
    }

    //@DecisionFunction
    public void confirmPayment(PaymentReference paymentReference) {
        //  expected output event is:
        // - OrderPaid
        Apply(new OrderPaid(GetId(), paymentReference));
    }

    //@EvolutionFunction
    public void Apply(OrderPaid orderPaid) {
        // should update state:
        // - order status
        // - the payment reference
        RecordChange(orderPaid);
        Status = OrderStatus.PAID;
        PaymentReference = orderPaid.PaymentReference;
    }

    //@DecisionFunction
    public void refusePayment() {
        //  expected output event is:
        // - OrderPaymentRefused
        Apply(new OrderPaymentRefused(GetId()));
    }

    //@EvolutionFunction
    public void Apply(OrderPaymentRefused orderPaymentRefused) {
        // should update state:
        // - order status
        // - (no) payment reference
        // - but also the fact the is NO more assigned seat !
        RecordChange(orderPaymentRefused);
        Status = OrderStatus.PAYMENT_REFUSED;
        PaymentReference = null;
        Seat = null;
    }

}
