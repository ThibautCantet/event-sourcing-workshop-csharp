

using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

public class OrderSeatBookingFailed : OrderEvent {

    public OrderSeatBookingFailed(OrderId orderId) : base(orderId) 
    {
    }

    public override void ApplyOn(Order order) {
        order.Apply(this);
    }
}
