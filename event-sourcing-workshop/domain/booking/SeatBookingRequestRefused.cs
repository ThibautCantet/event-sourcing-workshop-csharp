using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

//@EqualsAndHashCode
public class SeatBookingRequestRefused : ConferenceEvent {
    public OrderId OrderId { get; }

    public SeatBookingRequestRefused(ConferenceName id, OrderId orderId): base(id) 
    {
        OrderId = orderId;
    }

    public override void ApplyOn(Conference conference) {
        conference.Apply(this);
    }
}
