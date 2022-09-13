using System;
using System.Collections.Generic;
using System.Linq;
using fr.soat.banking.domain;
using fr.soat.eventsourcing.eventpublisher;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace event_sourcing_workshop_tests;

[TestClass]
public class BankCommandsTest
{
    private BankCommandHandler _bankCommandHandler;

    [TestInitialize]
    public void Setup()
    {
        var applicationEventPublisher = new ApplicationEventPublisher();
        var accountRepository = new AccountRepository(new InMemoryEventStore(applicationEventPublisher));
        _bankCommandHandler   = new(accountRepository);
        var transferProcessManager = new TransferProcessManager(accountRepository);
        applicationEventPublisher.Subscribe(transferProcessManager);
    }

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


    /* transfer command  */

    [TestMethod]
    public void should_successfully_transfer()
    {
        AccountId aliceAccountId = _bankCommandHandler.OpenAccount("alice");
        _bankCommandHandler.Deposit(aliceAccountId, 200);
        AccountId bobAccountId = _bankCommandHandler.OpenAccount("bob");

        // When
        _bankCommandHandler.RequestTransfer(aliceAccountId, bobAccountId, 50);

        // Then
        Account aliceAccount = _bankCommandHandler.LoadAccount(aliceAccountId);
        Account bobAccount = _bankCommandHandler.LoadAccount(bobAccountId);
        Assert.AreEqual(aliceAccount.Balance, 150);
        CollectionAssert.AreEqual(aliceAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened),
                typeof(AccountDeposited),
                typeof(TransferRequested),
                typeof(FundDebited)
            });
        Assert.AreEqual(bobAccount.Balance, 50);
        CollectionAssert.AreEqual(bobAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened),
                typeof(FundCredited)
            });
    }

    [TestMethod]
    public void should_fail_transfer_to_closed_account()
    {
        AccountId aliceAccountId = _bankCommandHandler.OpenAccount("alice");
        _bankCommandHandler.Deposit(aliceAccountId, 200);
        AccountId bobAccountId = _bankCommandHandler.OpenAccount("bob");
        _bankCommandHandler.CloseAccount(bobAccountId);

        // When
        _bankCommandHandler.RequestTransfer(aliceAccountId, bobAccountId, 50);

        // Then
        Account aliceAccount = _bankCommandHandler.LoadAccount(aliceAccountId);
        Account bobAccount = _bankCommandHandler.LoadAccount(bobAccountId);
        Assert.AreEqual(aliceAccount.Balance, 200);
        CollectionAssert.AreEqual(aliceAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened),
                typeof(AccountDeposited),
                typeof(TransferRequested),
                typeof(TransferRequestAborted)
            });
        Assert.AreEqual(bobAccount.Balance, 0);
        CollectionAssert.AreEqual(bobAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened),
                typeof(AccountClosed),
                typeof(CreditRequestRefused)
            });
    }

    [TestMethod]
    public void should_fail_transfer_when_funds_are_insufficient()
    {
        AccountId aliceAccountId = _bankCommandHandler.OpenAccount("alice");
        _bankCommandHandler.Deposit(aliceAccountId, 200);
        AccountId bobAccountId = _bankCommandHandler.OpenAccount("bob");

        // When
        _bankCommandHandler.RequestTransfer(aliceAccountId, bobAccountId, 250);

        // Then
        Account aliceAccount = _bankCommandHandler.LoadAccount(aliceAccountId);
        Account bobAccount = _bankCommandHandler.LoadAccount(bobAccountId);
        Assert.AreEqual(aliceAccount.Balance, 200);
        CollectionAssert.AreEqual(aliceAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened),
                typeof(AccountDeposited),
                typeof(TransferRequestRefused)
            });
        Assert.AreEqual(bobAccount.Balance, 0);
        CollectionAssert.AreEqual(bobAccount.Changes.Select(e => e.GetType()).ToList(),
            new List<Type>
            {
                typeof(AccountOpened)
            });
    }
}