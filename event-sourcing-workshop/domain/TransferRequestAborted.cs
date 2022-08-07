namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class TransferRequestAborted : TransferEvent
{
    private AccountId receiverAccountId;

    public TransferRequestAborted(AccountId accountId, AccountId receiverAccountId, int amount) : base(accountId,
        amount)
    {
        this.receiverAccountId = receiverAccountId;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}