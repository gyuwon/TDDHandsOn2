package accounting;

import java.time.LocalDateTime;
import java.util.UUID;

public interface OrderReader {

    Iterable<Order> getOrdersPlacedIn(
            UUID shopId,
            LocalDateTime placedAtUtcStartInclusive,
            LocalDateTime placedAtUtcEndExclusive);
}
