using fr.soat.banking.domain;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace event_sourcing_workshop_tests;

[TestClass]
public class BankCommandsTest
{
    private BankCommandHandler _bankCommandHandler = new(new AccountRepository(new InMemoryEventStore()));

    [TestMethod]
    public void should_register_a_new_account_then_use_then_close()
    {
        AccountId accountId = _bankCommandHandler.OpenAccount("toto");

        // When
        _bankCommandHandler.Deposit(accountId, 100);
        _bankCommandHandler.Deposit(accountId, 200);
        _bankCommandHandler.Withdraw(accountId, 300);
        _bankCommandHandler.CloseAccount(accountId);

        // Then
        Account reloadedAccount = _bankCommandHandler.LoadAccount(accountId);
        Assert.AreEqual(reloadedAccount.Status, AccountStatus.Closed);
        Assert.AreEqual(reloadedAccount.Balance, 0);
    }
}