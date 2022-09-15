package accounting;

import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.Optional;
import java.util.UUID;

public class HttpShopReader implements ShopReader {

    private final HttpClient client;
    private final String host;
    private final ObjectMapper mapper = getMapper();

    private static ObjectMapper getMapper() {
        return new ObjectMapper().configure(
                DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES,
                false);
    }

    public HttpShopReader(HttpClient client, String host) {
        this.client = client;
        this.host = host;
    }

    @Override
    public Optional<Shop> findShop(UUID id) {
        URI uri = URI.create(host + "/api/shops/" + id);
        HttpResponse<String> response = getString(uri);
        if (response.statusCode() == 200) {
            try {
                return Optional.of(mapper.readValue(response.body(), Shop.class));
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        } else {
            return Optional.empty();
        }
    }

    private HttpResponse<String> getString(URI uri) {
        HttpRequest request = HttpRequest
                .newBuilder()
                .GET()
                .uri(uri)
                .build();
        try {
            return client.send(
                    request,
                    HttpResponse.BodyHandlers.ofString());
        } catch (IOException | InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
