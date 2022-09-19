using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class Account : AggregateRoot<AccountId>
{
    public int Balance { get; private set; }

    public Account(AccountId accountId) : base(accountId)
    {
    }
    
    //@DecisionFunction
    public void Credit(int amount)
    {
        // The expected output event is:
        // - AccountCredited
        var evt = new AccountCredited(GetId(), amount);
        Apply(evt);
    }
        
    //@EvolutionFunction
    internal void Apply(AccountCredited e)
    {
        RecordChange(e);
        Balance += e.Amount;
    }
    
    //@DecisionFunction
    public Account RequestPayment(int amount, OrderId orderId)
    {
        // 1. should always keep trace of request (PaymentRequested event)
        var paymentRequested = new PaymentRequested(GetId(), amount);
        Apply(paymentRequested); 

        if (Balance >= amount)
        {
            var paymentAccepted = new PaymentAccepted(PaymentReference.genereate(), GetId(), amount, orderId);
            Apply(paymentAccepted);
        }
        else
        {
            // Apply a TransferRequestRefused evolution on sender account
            Apply(new PaymentRefused(GetId(), amount, orderId));
        }

        return this;
    }

    //@EvolutionFunction
    internal void Apply(PaymentRequested paymentRequested)
    {
        RecordChange(paymentRequested);
    }

    //@EvolutionFunction
    public void Apply(PaymentAccepted paymentAccepted)
    {
        // should update the balance !
        RecordChange(paymentAccepted);
        Balance -= paymentAccepted.Amount;
    }

    //@EvolutionFunction
    public void Apply(PaymentRefused paymentRefused)
    {
        RecordChange(paymentRefused);
    }
}