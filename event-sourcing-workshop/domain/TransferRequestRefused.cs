namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class TransferRequestRefused : TransferEvent
{
    private AccountId receiverAccountId;

    public TransferRequestRefused(AccountId accountId, AccountId receiverAccountId, int amount) : base(accountId,
        amount)
    {
        this.receiverAccountId = receiverAccountId;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}