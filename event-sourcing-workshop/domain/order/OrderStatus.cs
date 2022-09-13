namespace fr.soat.banking.domain;

public enum OrderStatus {
    NEW,
    SEAT_BOOKED,
    SEAT_BOOKING_FAILED,
    PAYMENT_REFUSED,
    PAID
}
