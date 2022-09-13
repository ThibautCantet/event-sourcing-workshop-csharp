

using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

public class OrderPaymentRefused : OrderEvent {

    public OrderPaymentRefused(OrderId orderId) : base(orderId) 
    {
    }

    public override void ApplyOn(Order order) {
        order.Apply(this);
    }
}
