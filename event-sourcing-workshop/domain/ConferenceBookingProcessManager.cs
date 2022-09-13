namespace fr.soat.banking.domain;

public class ConferenceBookingProcessManager {

    private OrderRepository orderRepository;
    private ConferenceRepository conferenceRepository;
    private AccountRepository accountRepository;

    public ConferenceBookingProcessManager(OrderRepository orderRepository, ConferenceRepository conferenceRepository, AccountRepository accountRepository) {
        this.orderRepository = orderRepository;
        this.conferenceRepository = conferenceRepository;
        this.accountRepository = accountRepository;
    }

    //@EventListener
    public void on(OrderRequested orderRequested) {
        Console.Out.WriteLine("consuming {0}", orderRequested.GetType().Name);
        // expected to bookSeat() on the target conference
        // book a seat
        Conference conference = conferenceRepository.load(orderRequested.ConferenceName);
        conference.bookSeat(orderRequested.OrderId);
        conferenceRepository.save(conference);
    }

    //@EventListener
    public void on(SeatBooked seatBooked) {
        Console.Out.WriteLine("consuming {0}", seatBooked.GetType().Name);
        // expected to
        // 1. assign the seat to the order
        // 2. request a payment on the customer account
        Order order = orderRepository.load(seatBooked.OrderId);
        order.assign(seatBooked.Seat);
        orderRepository.save(order);

        Account account = accountRepository.load(order.AccountId);
        Conference conference = conferenceRepository.load(order.ConferenceName);
        account.RequestPayment(conference.SeatPrice, seatBooked.OrderId);
        accountRepository.save(account);
    }

    //@EventListener
    public void on(SeatBookingRequestRefused seatBookingRequestRefused) {
        Console.Out.WriteLine("consuming {0}", seatBookingRequestRefused.GetType().Name);
        // expected to propagate the seat booking request failure to the order through order.failSeatBooking()
        Order order = orderRepository.load(seatBookingRequestRefused.OrderId);
        order.failSeatBooking();
        orderRepository.save(order);
    }

    //@EventListener
    public void on(PaymentAccepted paymentAccepted) {
        Console.Out.WriteLine("consuming {0}", paymentAccepted.GetType().Name);
        // expected to confirm the payment to the order through order.confirmPayment()
        Order order = orderRepository.load(paymentAccepted.OrderId);
        order.confirmPayment(paymentAccepted.PaymentReference);
        orderRepository.save(order);
    }

    //@EventListener
    public void on(PaymentRefused paymentRefused) {
        Console.Out.WriteLine("consuming {0}", paymentRefused.GetType().Name);
        // expected to:
        // 1. propagate the payment refuse to the order through a order.refusePayment()
        // 2. cancel the booking on the conferenace to release the seat, through conference.cancelBooking()
        Order order = orderRepository.load(paymentRefused.OrderId);
        order.refusePayment();
        orderRepository.save(order);

        Conference conference = conferenceRepository.load(order.ConferenceName);
        conference.cancelBooking(order.Seat);
        conferenceRepository.save(conference);
    }
}
