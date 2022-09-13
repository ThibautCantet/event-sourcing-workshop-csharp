
//@EqualsAndHashCode

using fr.soat.banking.domain;

public class SeatReleased : ConferenceEvent {
    public Seat Seat { get; }

    public SeatReleased(ConferenceName id, Seat bookedSeat): base(id) 
    {
        Seat = bookedSeat;
    }

    public override void ApplyOn(Conference conference) {
        conference.Apply(this);
    }
}
