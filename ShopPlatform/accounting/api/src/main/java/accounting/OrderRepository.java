package accounting;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;

import java.time.LocalDateTime;
import java.util.UUID;

public interface OrderRepository extends CrudRepository<Order, Long>, OrderReader {

    @Query("""
SELECT x
FROM Orders x
WHERE
    x.shopId = :shopId
    AND x.placedAtUtc >= :placedAtUtcStartInclusive
    AND x.placedAtUtc < :placedAtUtcEndExclusive
""")
    Iterable<Order> getOrdersPlacedIn(
            UUID shopId,
            LocalDateTime placedAtUtcStartInclusive,
            LocalDateTime placedAtUtcEndExclusive);
}
