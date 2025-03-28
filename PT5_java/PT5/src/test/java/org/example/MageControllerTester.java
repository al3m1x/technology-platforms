package org.example;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.Optional;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class MageControllerTester {
    private MageController mageController;

    @Mock
    private MageRepository mageRepository;

    @BeforeEach
    public void setUp() {
        MockitoAnnotations.openMocks(this);
        mageController = new MageController(mageRepository);
    }

    @Test
    public void deleteExistingMageTest() {
        String mageName = "Mag1";
        int mageLevel = 10;
        Mage mag = new Mage(mageName, mageLevel);

        when(mageRepository.find(mageName)).thenReturn(Optional.of(mag));

        assertEquals("done", mageController.delete(mageName));
    }

    @Test
    public void deleteNotExistingMageTest() {
        String mageName = "MagZ";
        doThrow(new IllegalArgumentException()).when(mageRepository).delete(mageName);
        assertEquals("not found", mageController.delete(mageName));
    }
    @Test
    public void findNotExistingMageTest() {
        String mageName = "MagZ";
        when(mageRepository.find(mageName)).thenReturn(Optional.empty());
        assertEquals("not found", mageController.find(mageName));
    }

    @Test
    public void findExistingMageTest() {
        Mage mage = new Mage("Mag1", 10);

        when(mageRepository.find("Mag1")).thenReturn(Optional.of(mage));

        assertEquals(mage.toString(), mageController.find("Mag1"));
    }

    @Test
    public void saveMageTest() {
        String mageName = "Merlin";
        int mageLevel = 10;
        when(mageRepository.find(mageName)).thenReturn(Optional.empty());
        assertEquals("done", mageController.save(mageName, String.valueOf(mageLevel)));
    }

    @Test
    public void saveExistingMageTest() {
        String mageName = "MagZ";
        int mageLevel = 10;
        Mage mage = new Mage(mageName, mageLevel);
        doThrow(new IllegalArgumentException()).when(mageRepository).find(mageName);
        assertEquals("bad request", mageController.save(mageName, String.valueOf(mageLevel)));
    }
}