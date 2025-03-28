package org.example;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Optional;

public class MageRepository {
    private final Collection<Mage> collection = new ArrayList<>();

    public Optional<Mage> find(String name) {
        for (Mage mage : collection) {
            if (mage.getName().equals(name)) {
                return Optional.of(mage);
            }
        }
        return Optional.empty();
    }

    public void delete(String name) {
        Optional<Mage> mage = find(name);
        if (mage.isPresent()) {
            collection.remove(mage.get());
        } else {
            throw new IllegalArgumentException("not found");
        }
    }

    public void save(Mage mag) {
        if (find(mag.getName()).isPresent()) {
            throw new IllegalArgumentException("bad request");
        } else {
            collection.add(mag);
        }
    }
}
