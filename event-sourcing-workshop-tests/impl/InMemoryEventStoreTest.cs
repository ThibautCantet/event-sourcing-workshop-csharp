using System.Collections.Generic;
using fr.soat.banking.domain;
using fr.soat.eventsourcing.api;
using fr.soat.eventsourcing.eventpublisher;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace event_sourcing_workshop_tests;

[TestClass]
public class InMemoryEventStoreTest
{
    IEventStore _eventStore = new InMemoryEventStore(new ApplicationEventPublisher());
    private AccountId _accountId = AccountId.Next();

    [TestInitialize]
    public void SetUp()
    {
        _eventStore.Clear();
    }

    [TestMethod]
    public void Should_store_and_reload()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };

        // When
        _eventStore.Store(_accountId, events);
        List<IEvent> reloadedEvents = _eventStore.LoadEvents(_accountId);

        // Then
        CollectionAssert.AreEqual(reloadedEvents, events);
    }

    [TestMethod]
    public void Should_store_and_reload_empty_event_list()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>();

        // When
        _eventStore.Store(_accountId, events);
        List<IEvent> reloadedEvents = _eventStore.LoadEvents(_accountId);

        // Then
        CollectionAssert.AreEqual(reloadedEvents, events);
    }

    [TestMethod]
    public void Should_store_be_idempotent()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, events);

        // When
        List<IEvent> sameEvents = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, sameEvents);
        List<IEvent> reloadedEvents = _eventStore.LoadEvents(_accountId);

        // Then
        CollectionAssert.AreEqual(reloadedEvents, events);
        CollectionAssert.AreEqual(reloadedEvents, sameEvents);
    }

    [TestMethod]
    public void Should_store_a_new_event_on_existing_events()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, events);

        // When
        events.Add(new AccountCredited(_accountId, 100));
        _eventStore.Store(_accountId, events);

        // Then
        List<IEvent> reloadedEvents = _eventStore.LoadEvents(_accountId);
        CollectionAssert.AreEqual(reloadedEvents, events);
    }

    [TestMethod]
    public void Should_store_new_events_on_existing_events()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, events);

        // When
        events.Add(new AccountCredited(_accountId, 10));
        events.Add(new AccountCredited(_accountId, 20));
        events.Add(new AccountCredited(_accountId, 30));
        _eventStore.Store(_accountId, events);

        // Then
        List<IEvent> reloadedEvents = _eventStore.LoadEvents(_accountId);
        CollectionAssert.AreEqual(reloadedEvents, events);
    }

    //[TestMethod]
    public void Should_fail_to_store_when_one_concurrent_modification_occurs()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, events);

        // When
        List<IEvent> newEvents = new List<IEvent>(events);
        //newEvents.Add(new AccountCredited(_accountId));
        _eventStore.Store(_accountId, newEvents);

        // Then
        List<IEvent> anotherNewEvents = new List<IEvent>(events);
      //  anotherNewEvents.Add(new AccountWithdrawn(_accountId, 10));
        var ex = Assert.ThrowsException<EventConcurrentUpdateException>(() =>
            _eventStore.Store(_accountId, anotherNewEvents));
        Assert.Equals(ex.Message, "Failed to save events, version mismatch (there was a concurrent update)");
    }

    //[TestMethod]
    public void Should_fail_to_store_when_version_is_very_old()
    {
        // Given
        _accountId = AccountId.Next();
        List<IEvent> events = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };
        _eventStore.Store(_accountId, events);

        // When
        List<IEvent> anotherEventsLateVersion = new List<IEvent>
        {
            new AccountCredited(_accountId, 100)
        };

        // Then
        var ex = Assert.ThrowsException<EventConcurrentUpdateException>(() =>
            _eventStore.Store(_accountId, anotherEventsLateVersion));
        Assert.Equals(ex.Message, "Failed to save events, version mismatch (there was a concurrent update)");
    }
}