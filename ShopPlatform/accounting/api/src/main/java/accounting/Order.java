package accounting;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.UUID;

@SuppressWarnings("unused")
@Entity(name = "Orders")
public class Order {

    @Column(name = "Id")
    private UUID id;

    @Id
    @Column(name = "Sequence")
    private Long sequence;

    @Column(name = "UserId")
    private UUID userId;

    @Column(name = "ShopId")
    private UUID shopId;

    @Column(name = "ItemId")
    private UUID itemId;

    @Column(name = "Price")
    private BigDecimal price;

    @Column(name = "Status")
    private String status;

    @Column(name = "PaymentTransactionId")
    private String paymentTransactionId;

    @Column(name = "PlacedAtUtc")
    private LocalDateTime placedAtUtc;

    @Column(name = "StartedAtUtc")
    private LocalDateTime startedAtUtc;

    @Column(name = "PaidAtUtc")
    private LocalDateTime paidAtUtc;

    @Column(name = "ShippedAtUtc")
    private LocalDateTime shippedAtUtc;

    public UUID getId() {
        return id;
    }

    public UUID getUserId() {
        return userId;
    }

    public UUID getShopId() {
        return shopId;
    }

    public UUID getItemId() {
        return itemId;
    }

    public BigDecimal getPrice() {
        return price;
    }

    public String getStatus() {
        return status;
    }

    public String getPaymentTransactionId() {
        return paymentTransactionId;
    }

    public LocalDateTime getPlacedAtUtc() {
        return placedAtUtc;
    }

    public LocalDateTime getStartedAtUtc() {
        return startedAtUtc;
    }

    public LocalDateTime getPaidAtUtc() {
        return paidAtUtc;
    }

    public LocalDateTime getShippedAtUtc() {
        return shippedAtUtc;
    }
}
