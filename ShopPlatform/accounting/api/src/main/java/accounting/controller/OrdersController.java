package accounting.controller;

import accounting.OrderView;
import accounting.query.GetOrdersPlacedIn;
import io.swagger.v3.oas.annotations.media.ArraySchema;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.ArrayList;

@SuppressWarnings("unused")
@RestController
@RequestMapping("/api/orders")
public class OrdersController {

    @PostMapping("/get-orders-placed-in")
    @ApiResponse(content = {
            @Content(
                    mediaType = "application/json",
                    array = @ArraySchema(schema = @Schema(implementation = OrderView.class)))
    })
    public Iterable<OrderView> getOrdersPlacedIn(@RequestBody GetOrdersPlacedIn query) {
        return new ArrayList<>();
    }
}
