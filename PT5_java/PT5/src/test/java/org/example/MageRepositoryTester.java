package org.example;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import java.util.Optional;
import static org.junit.jupiter.api.Assertions.*;

public class MageRepositoryTester {
    private MageRepository mageRepository;

    @BeforeEach //metoda wykona się przed każdym testem w klasie testowej, zapewnienie, że każdy test będzie wykonywany niezależnie
    void setUp() {
        mageRepository = new MageRepository();
    }

    @Test //oznaczenie metody testowej
    void deleteNotExistingMageTest() {
        assertThrows(IllegalArgumentException.class, () -> mageRepository.delete("MagZ"));
    }

    @Test
    void findNotExistingMageTest() {
        Optional<Mage> mageOptional = mageRepository.find("MagZ");
        assertFalse(mageOptional.isPresent());
    }

    @Test
    void findExistingMageTest() {
        Mage mag = new Mage("Mag1", 10);
        mageRepository.save(mag);
        Optional<Mage> mageOptional = mageRepository.find("Mag1");
        assertTrue(mageOptional.isPresent());
        assertEquals(mag, mageOptional.get());
    }

    @Test
    void saveExistingMageTest() {
        Mage mag = new Mage("Mag2", 10);
        mageRepository.save(mag);
        assertThrows(IllegalArgumentException.class, () -> mageRepository.save(mag));
    }
}
