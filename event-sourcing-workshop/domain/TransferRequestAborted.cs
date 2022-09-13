namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class TransferRequestAborted : TransferEvent
{
    private AccountId _receiverAccountId;

    public TransferRequestAborted(AccountId accountId, AccountId receiverAccountId, int amount) : base(accountId,
        amount)
    {
        _receiverAccountId = receiverAccountId;
    }

    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}