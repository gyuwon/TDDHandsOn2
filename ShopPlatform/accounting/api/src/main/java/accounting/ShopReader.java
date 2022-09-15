package accounting;

import java.util.Optional;
import java.util.UUID;

public interface ShopReader {

    Optional<Shop> findShop(UUID id);
}
