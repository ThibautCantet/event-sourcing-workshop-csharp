namespace fr.soat.banking.domain;


//@EqualsAndHashCode
public class CreditRequestRefused : TransferEvent {

    private AccountId sourceAccountId;

    public CreditRequestRefused(AccountId id, AccountId sourceAccountId, int amount): base(id, amount) 
    {
        this.sourceAccountId = sourceAccountId;
    }

    public override void ApplyOn(Account account) {
        account.Apply(this);
    }
}
