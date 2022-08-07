namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class FundCredited : TransferEvent
{
    private AccountId senderAccountId;

    public FundCredited(AccountId accountId, AccountId senderAccountId, int amount) : base(accountId, amount)
    {
        this.senderAccountId = senderAccountId;
    }
    
    public override void ApplyOn(Account account)
    {
        account.Apply(this);
    }
}