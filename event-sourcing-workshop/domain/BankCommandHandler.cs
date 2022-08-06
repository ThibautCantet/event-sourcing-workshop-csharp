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
        //FIXME
        // 1. load the account aggregate using the repository
        // 2. invoke the decision function deposit() on the aggregate to Apply the business logic
        // 3. save the mutated aggregate with the repository
        throw new Exception("implement me !");
    }

    //@Command
    public void Withdraw(AccountId id, int amount)
    {
        //FIXME
        // 1. load the account aggregate using the repository
        // 2. invoke the decision function withdraw() on the aggregate to Apply the business logic
        // 3. save the mutated aggregate with the repository
        throw new Exception("implement me !");
    }

    //@Command
    public void CloseAccount(AccountId id)
    {
        Account account = _repository.Load(id);
        account.Close();
        _repository.Save(account);
    }
}