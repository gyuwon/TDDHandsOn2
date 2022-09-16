package accounting.test;

import java.util.Optional;

import org.javaunit.autoparams.customization.Customizer;
import org.javaunit.autoparams.generator.ObjectContainer;
import org.javaunit.autoparams.generator.ObjectGenerator;

import accounting.ShopReader;

public class EmptyShopReaderCustomizer implements Customizer {

    @Override
    public ObjectGenerator customize(ObjectGenerator generator) {
        return (query, context) -> query.getType().equals(ShopReader.class)
            ? new ObjectContainer(create())
            : generator.generate(query, context);
    }

    private ShopReader create() {
        return id -> Optional.empty();
    }

}
