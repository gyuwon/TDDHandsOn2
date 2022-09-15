package accounting;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.UUID;

public record OrderView(
        UUID id,
        UUID userId,
        Shop shop,
        UUID itemId,
        BigDecimal price,
        String status,
        String paymentTransactionId,
        LocalDateTime placedAtUtc
) {

}
