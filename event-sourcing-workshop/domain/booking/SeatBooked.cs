using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class SeatBooked : ConferenceEvent {
    public OrderId OrderId { get; }
    public Seat Seat { get; }

    public SeatBooked(ConferenceName id, OrderId orderId, Seat seat): base(id) 
    {
        OrderId = orderId;
        Seat = seat;
    }

    public override void ApplyOn(Conference conference) {
        conference.Apply(this);
    }
}
