package org.example;

import java.util.Collection;
import java.util.Optional;

public class MageController {
    private MageRepository repository;

    public MageController(MageRepository repository) {
        this.repository = repository;
    }

    public MageRepository getRepository() {
        return repository;
    }

    public String find(String name) {
        try {
            Optional<MageDTO> optionalMageDTO = repository.find(name).map(this::convertToDTO);
            if (optionalMageDTO.isPresent()) {
                MageDTO mageDTO = optionalMageDTO.get();
                return new Mage(mageDTO.getName(), mageDTO.getLevel()).toString();
            } else {
                return "not found";
            }
        } catch (IllegalArgumentException e) {
            return "not found";
        }
    }

    public String delete(String name) {
        try {
            this.repository.delete(name);
            return "done";
        } catch (IllegalArgumentException e) {
            return "not found";
        }
    }

    public String save(String name, String level) {
        try {
            int mageLevel = Integer.parseInt(level);
            Optional<MageDTO> existingMageDTO = repository.find(name).map(this::convertToDTO);
            MageDTO mageDTO = new MageDTO(name, mageLevel);
            repository.save(convertToEntity(mageDTO));
            return "done";
        } catch (IllegalArgumentException e) {
            return "bad request";
        }
    }

    // Helper method to convert MageDTO to Mage
    private Mage convertToEntity(MageDTO mageDTO) {
        return new Mage(mageDTO.getName(), mageDTO.getLevel());
    }

    // Helper method to convert Mage to MageDTO
    private MageDTO convertToDTO(Mage mage) {
        return new MageDTO(mage.getName(), mage.getLevel());
    }
}