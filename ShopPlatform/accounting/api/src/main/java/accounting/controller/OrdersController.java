package accounting.controller;

import accounting.Order;
import accounting.OrderReader;
import accounting.OrderView;
import accounting.OrderViewAggregator;
import accounting.query.GetOrdersPlacedIn;
import io.swagger.v3.oas.annotations.media.ArraySchema;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.time.LocalDateTime;

@SuppressWarnings("unused")
@RestController
@RequestMapping("/api/orders")
public class OrdersController {

    private final OrderReader reader;
    private final OrderViewAggregator aggregator;

    public OrdersController(OrderReader reader, OrderViewAggregator aggregator) {
        this.reader = reader;
        this.aggregator = aggregator;
    }

    @PostMapping("/get-orders-placed-in")
    @ApiResponse(content = {
            @Content(
                    mediaType = "application/json",
                    array = @ArraySchema(schema = @Schema(implementation = OrderView.class)))
    })
    public Iterable<OrderView> getOrdersPlacedIn(@RequestBody GetOrdersPlacedIn query) {
        LocalDateTime startInclusive = LocalDateTime.of(query.year(), query.month(), 1, 0, 0);
        LocalDateTime endExclusive = startInclusive.plusMonths(1);
        Iterable<Order> orders = reader.getOrdersPlacedIn(
                query.shopId(),
                startInclusive,
                endExclusive);
        return aggregator.aggregateViews(orders);
    }
}
