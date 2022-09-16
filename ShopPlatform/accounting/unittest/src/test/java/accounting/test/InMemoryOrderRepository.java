package accounting.test;

import accounting.Order;
import accounting.OrderReader;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.UUID;
import java.util.stream.StreamSupport;

public class InMemoryOrderRepository extends ArrayList<Order> implements OrderReader {

    @Override
    public Iterable<Order> getOrdersPlacedIn(UUID shopId,
                                             LocalDateTime placedAtUtcStartInclusive,
                                             LocalDateTime placedAtUtcEndExclusive) {
        return StreamSupport
                .stream(spliterator(), false)
                .filter(x -> x.getShopId().equals(shopId))
                .filter(x -> x.getPlacedAtUtc().compareTo(placedAtUtcStartInclusive) >= 0)
                .filter(x -> x.getPlacedAtUtc().compareTo(placedAtUtcEndExclusive) < 0)
                .toList();
    }
}
