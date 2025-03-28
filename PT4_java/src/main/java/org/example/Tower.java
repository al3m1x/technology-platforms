package org.example;
import java.util.ArrayList;
import java.util.List;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;
@Entity
public class Tower {
    @Id @Getter @Setter
    private String name;
    @Getter @Setter
    private int height;
    @OneToMany(mappedBy = "tower", cascade = CascadeType.ALL, orphanRemoval = true)
    @Getter @Setter
    private List<Mage> mages;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public List<Mage> getMages() {
        return mages;
    }

    public void setMages(List<Mage> mages) {
        this.mages = mages;
    }

    public void addMage(Mage mage) {
        mages.add(mage);
    }

    public Tower(String name, int height) {
        this.name = name;
        this.height = height;
        this.mages = new ArrayList<>();
    }

    public void removeMage(Mage mage) {
        if (mages.contains(mage)) {
            mages.remove(mage);
            mage.setTower(null);
        }
    }
}
