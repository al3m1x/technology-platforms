package org.example;
import lombok.Getter;
import lombok.Setter;
import javax.persistence.*;
@Entity
public class Mage {
    @Id @Getter @Setter
    private String name;
    @Getter @Setter
    private int level;
    @Getter @Setter @ManyToOne
    @JoinColumn(name = "Tower")
    private Tower tower;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public Tower getTower() {
        return tower;
    }

    public void setTower(Tower tower) {
        this.tower = tower;
    }

    public Mage(String name, int level, Tower tower) {
        this.name = name;
        this.level = level;
        this.tower = tower;
    }
}
