using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class Account : AggregateRoot<AccountId>
{
    public String Owner { get; private set; }
    public String Number { get; private set; }
    public int Balance { get; private set; }
    public AccountStatus Status { get; private set; }

    public Account(AccountId accountId) : base(accountId)
    {
        Status = AccountStatus.New;
    }

    /* aggregate evolutions  */

    //EvolutionFunction
    internal void Apply(AccountOpened accountOpened)
    {
        Owner = accountOpened.Owner;
        Number = accountOpened.Number;
        Balance = 0;
        Status = AccountStatus.Open;
        RecordChange(accountOpened);
    }

    //EvolutionFunction
    internal void Apply(AccountDeposited accountDeposited)
    {
        Balance += accountDeposited.GetAmount();
        RecordChange(accountDeposited);
    }

    //EvolutionFunction
    internal void Apply(AccountWithdrawn accountWithdrawn)
    {
        Balance -= accountWithdrawn.Amount;
        RecordChange(accountWithdrawn);
    }

    //EvolutionFunction
    internal void Apply(AccountClosed accountClosed)
    {
        Status = AccountStatus.Closed;
        RecordChange(accountClosed);
    }

    /* decisions invoked by commands */

    //@DecisionFunction
    public static Account Create()
    {
        return new Account(AccountId.Next());
    }

    //@DecisionFunction
    public Account Register(String owner)
    {
        if (Status != AccountStatus.New)
        {
            throw new UnsupportedOperationException("Can not register a " + Status + " account");
        }

        AccountOpened e = new AccountOpened(GetId(), owner, Guid.NewGuid().ToString());
        Apply(e);
        return this;
    }

    //@DecisionFunction
    public Account Deposit(int amount)
    {
        // 1. Apply the business logic: check the account is valid to make a deposit (expected to be an open account)
        if (Status != AccountStatus.Open)
        {
            throw new UnsupportedOperationException("Can not deposit a " + Status + " account");
        }

        // 2. invoke the evolution function passing a new AccountDeposited event containing the mutation description (delta on the balance)
        AccountDeposited e = new AccountDeposited(GetId(), amount);
        Apply(e);
        return this;
    }

    //@DecisionFunction
    public Account Withdraw(int amount)
    {
        // 1. Apply the business logic: check:
        // - the account is valid to make a deposit (expected to be an open account)
        // - the withdraw Amount is not greater than the current balance !
        if (Status != AccountStatus.Open)
        {
            throw new UnsupportedOperationException("Can not deposit a " + Status + " account");
        }

        if (Balance < amount)
        {
            throw new InsufficientFundsException("Withdrawal of " + amount + " can not be applied with balance of " +
                                                 Balance);
        }
        // 2. invoke the evolution function passing a new AccountWithdrawn event containing the mutation description (delta on the balance)
        AccountWithdrawn e = new AccountWithdrawn(GetId(), amount);
        Apply(e);
        return this;
    }

    //@DecisionFunction
    public Account Close()
    {
        if (Status != AccountStatus.Open)
        {
            throw new UnsupportedOperationException("Can not close a " + Status + " account");
        }

        AccountClosed e = new AccountClosed(GetId());
        Apply(e);
        return this;
    }

    /* Transfer management */

    //@DecisionFunction
    public Account RequestTransfer(Account receiverAccount, int amount)
    {
        if (Status != AccountStatus.Open)
        {
            throw new UnsupportedOperationException("Can not transfer from a " + Status + " account");
        }

        if (amount <= Balance)
        {
            // transfer authorized
            Apply(new TransferRequested(GetId(), receiverAccount.GetId(), amount));
            receiverAccount.Credit(this, amount);
        }
        else
        {
            //FIXME when funds are insufficient...
            // Apply a TransferRequestRefused evolution on sender account
            throw new Exception("implement me !");
        }

        return this;
    }

    //@EvolutionFunction
    internal void Apply(TransferRequested transferRequested)
    {
        RecordChange(transferRequested);
    }

    //@EvolutionFunction
    internal void Apply(TransferRequestRefused e)
    {
        RecordChange(e);
    }

    //@DecisionFunction
    public void Credit(Account senderAccount, int amount)
    {
        //FIXME expected implementation:
        // IF the receiver account is OPEN
        // 1. Apply a FundCredited evolution on receiver account
        // 2. make debit() decition on sender account
        // ELSE
        // 1. Apply a CreditRequestRefused evolution on receiver account
        // 2. make abortTransferRequest() decision on sender account
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    internal void Apply(FundCredited e)
    {
        Balance += e.Amount;
        RecordChange(e);
    }

    //@EvolutionFunction
    internal void Apply(CreditRequestRefused e)
    {
        RecordChange(e);
    }

    //@DecisionFunction
    public void AbortTransferRequest(AccountId receiverAccountId, int amount)
    {
        Apply(new TransferRequestAborted(GetId(), receiverAccountId, amount));
    }

    //@EvolutionFunction
    internal void Apply(TransferRequestAborted transferRequestAborted)
    {
        RecordChange(transferRequestAborted);
    }

    //@DecisionFunction
    public void Debit(AccountId receiverAccountId, int amount)
    {
        Apply(new FundDebited(GetId(), receiverAccountId, amount));
    }

    //@EvolutionFunction
    internal void Apply(FundDebited e)
    {
        Balance -= e.Amount;
        RecordChange(e);
    }
}