namespace fr.soat.banking.domain;

public class BankCommandHandler
{
    private readonly AccountRepository _repository;

    public BankCommandHandler(AccountRepository repository)
    {
        _repository = repository;
    }

    //@Command
    public AccountId OpenAccount(String owner)
    {
        Account account = Account
            .Create()
            .Register(owner);

        _repository.Save(account);
        return account.GetId();
    }

    //@Command
    public Account LoadAccount(AccountId id)
    {
        return _repository.Load(id);
    }

    //@Command
    public void Deposit(AccountId id, int amount)
    {
        // 1. load the account aggregate using the repository
        Account account = _repository.Load(id);
        // 2. invoke the decision function deposit() on the aggregate to Apply the business logic
        account.Deposit(amount);
        // 3. save the mutated aggregate with the repository
        _repository.Save(account);
    }

    //@Command
    public void Withdraw(AccountId id, int amount)
    {
        // 1. load the account aggregate using the repository
        Account account = _repository.Load(id);
        // 2. invoke the decision function withdraw() on the aggregate to apply the business logic
        account.Withdraw(amount);
        // 3. save the mutated aggregate with the repository
        _repository.Save(account);
    }

    //@Command
    public void CloseAccount(AccountId id)
    {
        Account account = _repository.Load(id);
        account.Close();
        _repository.Save(account);
    }
    
    //@Command
    public void RequestTransfer(AccountId idFrom, AccountId idTo, int amount) {
        Account accountFrom = _repository.Load(idFrom);
        Account accountTo = _repository.Load(idTo);
        accountFrom.RequestTransfer(accountTo, amount);
        _repository.Save(accountFrom);
        _repository.Save(accountTo);
    }
}