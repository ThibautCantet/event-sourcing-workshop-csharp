namespace fr.soat.banking.domain;


//@EqualsAndHashCode
public class CreditRequestRefused : TransferEvent {

    public AccountId SourceAccountId { get; }

    public CreditRequestRefused(AccountId id, AccountId sourceAccountId, int amount): base(id, amount) 
    {
        this.SourceAccountId = sourceAccountId;
    }

    public override void ApplyOn(Account account) {
        account.Apply(this);
    }
}
