using fr.soat.banking.domain.order;

namespace fr.soat.banking.domain;

public class OrderFactory {

    public static Order create() {
        return new Order(OrderId.Next());
    }

}
