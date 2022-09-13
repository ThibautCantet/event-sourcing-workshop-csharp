namespace fr.soat.banking.domain;

public class PaymentReference
{
    private String reference;

    private PaymentReference(string id)
    {
        reference = id;
    }

    public static PaymentReference genereate() {
        return new PaymentReference(Guid.NewGuid().ToString());
    }

    public static PaymentReference from(String id) {
        return new PaymentReference(id);
    }

    public override string ToString()
    {
        return reference;
    }
}