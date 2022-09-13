using System;
using System.Collections.Generic;
using System.Linq;
using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.eventpublisher;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace fr.soat.banking.domain;

[TestClass]
public class AccountTest {

    AccountId myAccountId = AccountId.Next();
    AccountRepository repository = new(new InMemoryEventStore(new ApplicationEventPublisher()));

    [TestMethod]
    public void should_credit_an_account() {
        Account myAccount = new Account(myAccountId);
        myAccount.Credit(100);
        repository.save(myAccount);

        myAccount = repository.load(myAccountId);
        myAccount.Credit(10);
        repository.save(myAccount);

        myAccount = repository.load(myAccountId);
        Assert.AreEqual(myAccount.Balance, 110);
        CollectionAssert.AreEqual(myAccount.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(AccountCredited),
                        typeof(AccountCredited)
                });
    }

    [TestMethod]
    public void should_pay_with_an_account() {
        Account myAccount = new Account(myAccountId);
        myAccount.Credit(100);
        repository.save(myAccount);
        myAccount.RequestPayment(10, OrderId.Next());
        repository.save(myAccount);

        myAccount = repository.load(myAccountId);
        Assert.AreEqual(myAccount.Balance, 90);
        CollectionAssert.AreEqual(myAccount.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(AccountCredited),
                        typeof(PaymentRequested),
                        typeof(PaymentAccepted)
                });
    }

    [TestMethod]
    public void should_reject_payment_when_funds_are_insufficient() {
        Account myAccount = new Account(myAccountId);
        myAccount.Credit(100);
        repository.save(myAccount);
        myAccount.RequestPayment(200, OrderId.Next());
        repository.save(myAccount);

        myAccount = repository.load(myAccountId);
        Assert.AreEqual(myAccount.Balance, 100);
        CollectionAssert.AreEqual(myAccount.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(AccountCredited),
                        typeof(PaymentRequested),
                        typeof(PaymentRefused)
                });
    }


}
