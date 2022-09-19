using fr.soat.banking.domain.order;
using fr.soat.eventsourcing.api;

namespace fr.soat.banking.domain;

public class Conference : AggregateRoot<ConferenceName> {

    private List<Seat> seats = new();
    public List<Seat> AvailableSeats { get; } = new();
    public int SeatPrice { get; private set; }
    public ConferenceStatus Status { get; private set; }

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
        // given the input event, init the conference state
        Status = ConferenceStatus.OPEN;
        SeatPrice = conferenceOpened.SeatPrice;
        for (int i = 0; i < conferenceOpened.Places; i++)
        {
            initializeSeats(i);
        }
        RecordChange(conferenceOpened);
    }

    private void initializeSeats(int seatNumber) {
        Seat seat = new Seat(seatNumber + 1);
        seats.Insert(seatNumber, seat);
        AvailableSeats.Insert(seatNumber, seat);
    }

    //@DecisionFunction
    public Seat bookSeat(OrderId orderId)
    {
        // if some seats are available, we should remove one seat from available seats and return it
        // The possible expected output events are:
        // - SeatBookingRequestRefused
        // - SeatBooked
        if (AvailableSeats != null)
        {
            Seat bookedSeat = AvailableSeats[0];
            AvailableSeats.RemoveAt(0);
            Apply(new SeatBooked(GetId(), orderId, bookedSeat));
            return bookedSeat;
        }

        Apply(new SeatBookingRequestRefused(GetId(), orderId));
        return null;
    }

    //@DecisionFunction
    public void cancelBooking(Seat seat) {
        // The expected output event is:
        // - SeatReleased
        Apply(new SeatReleased(GetId(), seat));
    }

    //@EvolutionFunction
    public void Apply(SeatBooked conferenceSeatBooked) {
        // given the input event:
        // - update the remaining available seats
        // - update the conference status if needed
        RecordChange(conferenceSeatBooked);
        if (AvailableSeats == null) {
            Status = ConferenceStatus.FULL;
        }
        AvailableSeats.Remove(conferenceSeatBooked.Seat);
    }

    //@EvolutionFunction
    public void Apply(SeatBookingRequestRefused seatBookingRequestRefused) {
        RecordChange(seatBookingRequestRefused);
    }

    //@EvolutionFunction
    public void Apply(SeatReleased seatReleased) {
        // similar to Apply(SeatBooked)
        RecordChange(seatReleased);
        AvailableSeats.Add(seatReleased.Seat);
        Status = ConferenceStatus.OPEN;
    }

    public override string ToString() {
        return "room " + GetId() +
                ": " +
                AvailableSeats.Count + " / " + seats.Count + " available seats" +
                " (" + Status + ")";
    }

}
