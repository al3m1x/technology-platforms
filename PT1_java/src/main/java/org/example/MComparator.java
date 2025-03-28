package org.example;

import java.util.Comparator;

public class MComparator implements Comparator<Mage> {
    @Override
    public int compare(Mage o1, Mage o2) { //function from interface Comparator - alternative order - level, name, power
        if (o1 == null || o2 == null) {
            throw new NullPointerException("Błąd.");
        }
        if (o1.getLevel() != o2.getLevel()) {
            return Integer.compare(o1.getLevel(), o2.getLevel());
        }
        else if (o1.getName().compareTo(o2.getName()) != 0){
            return o1.getName().compareTo(o2.getName());
        }
        else {
            return Double.compare(o1.getPower(), o2.getPower());
        }
    }
}
