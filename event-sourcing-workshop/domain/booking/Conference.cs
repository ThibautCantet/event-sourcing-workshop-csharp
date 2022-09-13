using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class Conference : AggregateRoot<ConferenceName> {

    private List<Seat> seats = new();
    public List<Seat> AvailableSeats { get; } = new();
    public int SeatPrice { get; private set; }
    public ConferenceStatus Status { get; }

    public Conference(ConferenceName conferenceName): base(conferenceName) {
        Status = ConferenceStatus.NEW;
    }

    //@DecisionFunction
    public Conference open(int places, int seatPrice) {
        Apply(new ConferenceOpened(GetId(), places, seatPrice));
        return this;
    }

    //@EvolutionFunction
    public void Apply(ConferenceOpened conferenceOpened) {
        //TODO(FIXME)
        // given the input event, init the conference state
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public Seat bookSeat(OrderId orderId)
    {
        //TODO(FIXME)
        // if some seats are available, we should remove one seat from available seats and return it
        // The possible expected output events are:
        // - SeatBookingRequestRefused
        // - SeatBooked
        throw new Exception("implement me !");
    }

    //@DecisionFunction
    public void cancelBooking(Seat seat) {
        //TODO(FIXME)
        // The expected output event is:
        // - SeatReleased
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(SeatBooked conferenceSeatBooked) {
        //TODO(FIXME)
        // given the input event:
        // - update the remaining available seats
        // - update the conference status if needed
        throw new Exception("implement me !");
    }

    //@EvolutionFunction
    public void Apply(SeatBookingRequestRefused seatBookingRequestRefused) {
        RecordChange(seatBookingRequestRefused);
    }

    //@EvolutionFunction
    public void Apply(SeatReleased seatReleased) {
        //TODO(FIXME)
        // similar to Apply(SeatBooked)
        throw new Exception("implement me !");
    }

    public override string ToString() {
        return "room " + GetId() +
                ": " +
                AvailableSeats.Count + " / " + seats.Count + " available seats" +
                " (" + Status + ")";
    }

}
