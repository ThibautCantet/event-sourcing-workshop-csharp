namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class PaymentRequested : AccountEvent {

    private int amount;

    public PaymentRequested(AccountId accountId, int amount): base(accountId)
    {
        this.amount = amount;
    }

    public override void ApplyOn(Account account) {
        account.Apply(this);
    }
}
