package accounting.test;

import accounting.Order;
import org.javaunit.autoparams.customization.CompositeCustomizer;
import org.javaunit.autoparams.customization.InstanceFieldWriter;

public class AccountingCustomizer extends CompositeCustomizer {

    public AccountingCustomizer() {
        super(
            new InstanceFieldWriter(Order.class),
            new EmptyShopReaderCustomizer());
    }
}
