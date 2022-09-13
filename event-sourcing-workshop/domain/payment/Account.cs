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
        //TODO(FIXME)
        // The expected output event is:
        // - AccountCredited
        throw new Exception("implement me !");
    }
        
    //@EvolutionFunction
    internal void Apply(AccountCredited e)
    {
        //TODO(FIXME)
        // should update the balance !
        throw new Exception("implement me !");
    }
    
    //@DecisionFunction
    public Account RequestPayment(int amount, OrderId orderId)
    {
        //TODO(FIXME)
        // 1. should always keep trace of request (PaymentRequested event)
        // 2. should then check if funds are sufficient
        // The possible expected output events are:
        // - PaymentAccepted
        // - PaymentRefused
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    internal void Apply(PaymentRequested paymentRequested)
    {
        RecordChange(paymentRequested);
    }

    //@EvolutionFunction
    public void Apply(PaymentAccepted paymentAccepted)
    {
        //TODO(FIXME)
        // should update the balance !
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(PaymentRefused paymentRefused)
    {
        RecordChange(paymentRefused);
    }
}