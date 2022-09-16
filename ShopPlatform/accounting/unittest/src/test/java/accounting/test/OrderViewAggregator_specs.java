package accounting.test;

import accounting.Order;
import accounting.OrderView;
import accounting.OrderViewAggregator;
import org.javaunit.autoparams.CsvAutoSource;
import org.javaunit.autoparams.customization.Customization;
import org.junit.jupiter.params.ParameterizedTest;

import java.lang.reflect.Field;
import java.util.List;
import static org.junit.jupiter.api.Assertions.assertEquals;

@SuppressWarnings("NewClassNamingConvention")
public class OrderViewAggregator_specs {

    @ParameterizedTest
    @CsvAutoSource({
            "Pending, 보류",
            "AwaitingPayment, 결제대기",
            "AwaitingShipment, 배송대기",
            "Completed, 완료",
    })
    @Customization(AccountingCustomizer.class)
    void sut_localizes_status(
        String statusValue,
        String localizedValue,
        OrderViewAggregator sut,
        Order order
    ) {
        setStatus(order, statusValue);

        Iterable<OrderView> views = sut.aggregateViews(List.of(order));

        OrderView actual = views.iterator().next();
        assertEquals(localizedValue, actual.status());
    }

    private static void setStatus(Order order, String statusValue) {
        try {
            Field status = Order.class.getDeclaredField("status");
            status.setAccessible(true);
            status.set(order, statusValue);
        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }
    }
}
