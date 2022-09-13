using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

public class OrderPaid : OrderEvent
{
    public PaymentReference PaymentReference { get; }

    public OrderPaid(OrderId orderId, PaymentReference paymentReference) : base(orderId)
    {
        this.PaymentReference = paymentReference;
    }

    public override void ApplyOn(Order order)
    {
        order.Apply(this);
    }
}