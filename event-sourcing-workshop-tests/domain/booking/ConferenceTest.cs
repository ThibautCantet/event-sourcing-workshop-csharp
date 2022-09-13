

using fr.soat.banking.domain;
using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.eventpublisher;
using fr.soat.eventsourcing.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ConferenceTest {

    ConferenceRepository repository = new(new InMemoryEventStore(new ApplicationEventPublisher()));

    [TestMethod]
    public void should_create_book_then_release_seat() {
        Conference picasso = new Conference(ConferenceName.name("picasso dans tous ses etats")).open(3, 10);
        Assert.AreEqual(picasso.Status, ConferenceStatus.OPEN);
        Seat seat1 = picasso.bookSeat(OrderId.Next());
        Seat seat2 = picasso.bookSeat(OrderId.Next());
        Seat seat3 = picasso.bookSeat(OrderId.Next());

        Assert.IsNull(picasso.AvailableSeats);
        Assert.AreEqual(picasso.Status, ConferenceStatus.FULL);
        picasso.cancelBooking(seat2);

        repository.save(picasso);
        Conference reloadedPicasso = repository.load(picasso.GetId());
        CollectionAssert.Contains(picasso.AvailableSeats, seat2);
        Assert.AreEqual(picasso.SeatPrice, 10);
        Assert.AreEqual(picasso.Status, ConferenceStatus.OPEN);
    }

    [TestMethod]
    public void should_return_empty_when_booking_with_no_more_places() {
        Conference picasso = new Conference(ConferenceName.name("picasso dans tous ses etats")).open(3, 10);
        Assert.AreEqual(picasso.Status, ConferenceStatus.OPEN);
        Seat seat1 = picasso.bookSeat(OrderId.Next());
        Seat seat2 = picasso.bookSeat(OrderId.Next());
        Assert.AreEqual(picasso.Status, ConferenceStatus.OPEN);
        Seat seat3 = picasso.bookSeat(OrderId.Next());

        Assert.IsNull(picasso.bookSeat(OrderId.Next()));
        Assert.AreEqual(picasso.Status, ConferenceStatus.FULL);
    }

}
