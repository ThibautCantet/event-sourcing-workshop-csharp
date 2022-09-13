using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class PaymentRefused : AccountEvent
{
    public int Amount { get; }
    public OrderId OrderId { get; }

    public PaymentRefused(AccountId id, int amount, OrderId orderId) : base(id)
    {
        this.Amount = amount;
        this.OrderId = orderId;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}