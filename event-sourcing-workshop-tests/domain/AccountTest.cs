using fr.soat.banking.domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace event_sourcing_workshop_tests;

[TestClass]
public class AccountTest
{
    [TestMethod]
    public void Should_succeed_a_classic_FSM_scenario()
    {
        // Given
        Account account = Account.Create();

        // When
        account.Register("toto")
            .Deposit(100)
            .Deposit(100)
            .Withdraw(200)
            .Close();

        // Then
        Assert.AreEqual(account.Owner, "toto");
        Assert.AreEqual(account.Balance, 0);
        Assert.AreEqual(account.Version(), 5);
        Assert.AreEqual(account.Status, AccountStatus.Closed);
        Assert.IsTrue(account.Changes.Exists(e => e.GetType() == typeof(AccountOpened)));
        Assert.IsTrue(account.Changes.Exists(e => e.GetType() == typeof(AccountDeposited)));
        Assert.IsTrue(account.Changes.Exists(e => e.GetType() == typeof(AccountDeposited)));
        Assert.IsTrue(account.Changes.Exists(e => e.GetType() == typeof(AccountWithdrawn)));
        Assert.IsTrue(account.Changes.Exists(e => e.GetType() == typeof(AccountClosed)));
    }

    [TestMethod]
    public void Should_fail_to_get_negative_balance()
    {
        // Given
        Account account = Account.Create();

        // When
        var ex = Assert.ThrowsException<InsufficientFundsException>(() => account.Register("toto")
            .Deposit(100)
            .Withdraw(200));
        Assert.AreEqual(ex.Message, "Withdrawal of 200 can not be applied with balance of 100");
        Assert.AreEqual(account.Balance, 100);
        Assert.AreEqual(account.Version(), 2);
    }

    [TestMethod]
    public void Should_fail_with_invalid_decisions_on_new_account()
    {
        // Given
        Account account = Account.Create();
        Assert.AreEqual(account.Status, AccountStatus.New);

        // When
        var ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Withdraw(100));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
        ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Deposit(100));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
        ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Close());
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
    }

    [TestMethod]
    public void Should_fail_with_invalid_decisions_on_open_account()
    {
        // Given
        Account account = Account.Create();
        account.Register("alice");
        Assert.AreEqual(account.Status, AccountStatus.Open);

        // When
        var ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Register("bob"));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
    }

    [TestMethod]
    public void Should_fail_with_invalid_decisions_on_closed_account()
    {
        // Given
        Account account = Account.Create();
        account.Register("alice")
            .Close();
        Assert.AreEqual(account.Status, AccountStatus.Closed);

        // When
        var ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Register("bob"));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
        ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Withdraw(100));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
        ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Deposit(100));
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
        ex = Assert.ThrowsException<UnsupportedOperationException>(() => account.Close());
        Assert.IsInstanceOfType(ex, typeof(UnsupportedOperationException));
    }
}