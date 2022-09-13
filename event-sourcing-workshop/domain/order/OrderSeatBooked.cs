using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

public class OrderSeatBooked : OrderEvent {
    public Seat BookedSeat { get; }

    public OrderSeatBooked(OrderId orderId, Seat bookedSeat) : base(orderId)
    {
        this.BookedSeat = bookedSeat;
    }

    public override void ApplyOn(Order order) {
        order.Apply(this);
    }
}
