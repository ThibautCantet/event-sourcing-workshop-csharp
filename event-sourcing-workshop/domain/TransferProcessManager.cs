using fr.soat.eventsourcing.api;
using fr.soat.eventsourcing.eventpublisher;

namespace fr.soat.banking.domain;

public class TransferProcessManager : IListener {

    private readonly AccountRepository accountRepository;

    public TransferProcessManager(AccountRepository accountRepository) {
        this.accountRepository = accountRepository;
    }

    //@EventListener
    public void On(TransferRequested transferRequested) {
        Console.Out.WriteLine("consuming {0}", transferRequested.GetType().Name);
        Account receiverAccount = accountRepository.Load(transferRequested.ReceiverAccountId());
        receiverAccount.Credit(transferRequested.AccountId, transferRequested.Amount);
        accountRepository.Save(receiverAccount);
    }

    //@EventListener
    public void On(CreditRequestRefused transferFundCredited) {
        Account senderAccount = accountRepository.Load(transferFundCredited.SourceAccountId);
        senderAccount.AbortTransferRequest(transferFundCredited.AccountId, transferFundCredited.Amount);
        accountRepository.Save(senderAccount);
    }

    //@EventListener
    public void On(FundCredited fundCredited) {
        Console.Out.WriteLine("consuming {0}", fundCredited.GetType().Name);
        // when triggered on a FundCredited
        // 1. reload the sender Account
        Account senderAccount = accountRepository.Load(fundCredited.SenderAccountId);
        // 2. make debit() decision on sender account
        senderAccount.Debit(fundCredited.AccountId, fundCredited.Amount);
        // 3. save the sender Account
        accountRepository.Save(senderAccount);
    }

    public void OnMessage(IEvent msg)
    {
        if (msg.GetType() == typeof(TransferRequested))
        {
            On((TransferRequested) msg);
        }
        if (msg.GetType() == typeof(CreditRequestRefused))
        {
            On((CreditRequestRefused) msg);
        }
        if (msg.GetType() == typeof(FundCredited))
        {
            On((FundCredited) msg);
        }

    }
}
