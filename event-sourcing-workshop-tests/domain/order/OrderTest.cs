using System;
using System.Collections.Generic;
using System.Linq;
using fr.soat.eventsourcing.eventpublisher;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace fr.soat.banking.domain;

[TestClass]
public class OrderTest {

    ConferenceName conferenceName = ConferenceName.name("picasso dans tous ses etats");
    AccountId myAccountId = AccountId.Next();
    OrderRepository repository = new(new InMemoryEventStore(new ApplicationEventPublisher()));
    private Seat seatOne = new(1);
    private PaymentReference  paymentReference = PaymentReference.genereate();

    [TestMethod]
    public void should_create_then_book_then_pay_order() {
        Order order = OrderFactory.create();
        order.requestBooking(conferenceName, myAccountId)
                .assign(seatOne)
                .confirmPayment(paymentReference);
        repository.save(order);

        order = repository.load(order.GetId());
        Assert.AreEqual(order.Status, OrderStatus.PAID);
        Assert.AreEqual(order.Seat, seatOne);
        Assert.AreEqual(order.PaymentReference, paymentReference);
        CollectionAssert.AreEqual(order.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(OrderRequested),
                        typeof(OrderSeatBooked),
                        typeof(OrderPaid)
                });
    }

    [TestMethod]
    public void should_create_then_book_then_fail() {
        Order order = OrderFactory.create();
        order.requestBooking(conferenceName, myAccountId)
                .failSeatBooking();
        repository.save(order);

        order = repository.load(order.GetId());
        Assert.AreEqual(order.Status, OrderStatus.SEAT_BOOKING_FAILED);
        Assert.IsNull(order.Seat);
        Assert.IsNull(order.PaymentReference);
        CollectionAssert.AreEqual(order.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(OrderRequested),
                        typeof(OrderSeatBookingFailed)
                });
    }

    [TestMethod]
    public void should_create_then_book_then_pay_then_fail() {
        Order order = OrderFactory.create();
        order.requestBooking(conferenceName, myAccountId)
                .assign(seatOne)
                .refusePayment();
        repository.save(order);

        order = repository.load(order.GetId());
        Assert.AreEqual(order.Status, OrderStatus.PAYMENT_REFUSED);
        Assert.IsNull(order.Seat);
        Assert.IsNull(order.PaymentReference);
        CollectionAssert.AreEqual(order.Changes.Select(e => e.GetType()).ToList(),
                new List<Type>
                {
                        typeof(OrderRequested),
                        typeof(OrderSeatBooked),
                        typeof(OrderPaymentRefused),
                });
    }
}
