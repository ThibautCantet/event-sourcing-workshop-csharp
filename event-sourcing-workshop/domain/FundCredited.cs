namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class FundCredited : TransferEvent
{
    public AccountId SenderAccountId { get; }

    public FundCredited(AccountId accountId, AccountId senderAccountId, int amount) : base(accountId, amount)
    {
        SenderAccountId = senderAccountId;
    }
    
    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}