package accounting;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
import java.util.stream.StreamSupport;

public class OrderViewAggregator {

    private final ShopReader shopReader;

    public OrderViewAggregator(ShopReader shopReader) {
        this.shopReader = shopReader;
    }

    public Iterable<OrderView> aggregateViews(Iterable<Order> orders) {
        Map<UUID, Shop> map = new HashMap<>();
        return StreamSupport
                .stream(orders.spliterator(), false)
                .map(x -> new OrderView(
                        x.getId(),
                        x.getUserId(),
                        map.computeIfAbsent(x.getShopId(), this::getShop),
                        x.getItemId(),
                        x.getPrice(),
                        localizeStatus(x.getStatus()),
                        x.getPaymentTransactionId(),
                        x.getPlacedAtUtc()))
                .toList();
    }

    private static String localizeStatus(String status) {
        return switch (status) {
            case "Pending" -> "보류";
            case "AwaitingPayment" -> "결제대기";
            case "AwaitingShipment" -> "배송대기";
            case "Completed" -> "완료";
            default -> status;
        };
    }

    private Shop getShop(UUID shopId) {
        return shopReader.findShop(shopId).orElse(new Shop(shopId, shopId.toString()));
    }
}
