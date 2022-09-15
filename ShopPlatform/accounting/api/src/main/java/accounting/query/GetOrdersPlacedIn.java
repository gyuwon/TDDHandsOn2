package accounting.query;

import java.util.UUID;

public record GetOrdersPlacedIn(UUID shopId, int year, int month) {
}
