using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class PaymentAccepted : AccountEvent
{
    public int Amount { get; }
    public OrderId OrderId { get; }
    public PaymentReference PaymentReference { get; }

    public PaymentAccepted(PaymentReference paymentReference, AccountId id, int amount, OrderId orderId) : base(id)
    {
        this.Amount = amount;
        this.OrderId = orderId;
        this.PaymentReference = paymentReference;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}