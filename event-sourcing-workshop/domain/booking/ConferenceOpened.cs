using fr.soat.banking.domain;

//@EqualsAndHashCode
public class ConferenceOpened : ConferenceEvent {
    public int Places { get; }
    public int SeatPrice { get; }

    public ConferenceOpened(ConferenceName conferenceName, int places, int seatPrice): base(conferenceName)
    {
        Places = places;
        SeatPrice = seatPrice;
    }

    public override void ApplyOn(Conference conference) {
        conference.Apply(this);
    }
}
