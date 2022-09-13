using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;
public class OrderRequested : OrderEvent {
    public ConferenceName ConferenceName { get; }
    public AccountId AccountId { get; }

    public OrderRequested(OrderId id, ConferenceName conferenceName, AccountId accountForPayment) : base(id)
    {
        this.ConferenceName = conferenceName;
        this.AccountId = accountForPayment;
    }

    public override void ApplyOn(Order order) {
        order.Apply(this);
    }

}
