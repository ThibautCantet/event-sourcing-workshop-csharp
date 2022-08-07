namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public abstract class TransferEvent : AccountEvent {

    public int Amount { get; }

    public TransferEvent(AccountId accountId, int amount): base(accountId) 
    {
        Amount = amount;
    }

}
