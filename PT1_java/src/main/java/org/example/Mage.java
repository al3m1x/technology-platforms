package org.example;
import java.util.*;

public class Mage implements Comparable<Mage> {
    private String name;
    private int level;
    private double power;
    boolean type;
    private Set<Mage> apprentices;

    public Mage(String name, int level, double power, boolean type, boolean natural_order) {
        this.name = name;
        this.level = level;
        this.power = power;
        this.type = type;

        if(type) {
            if(natural_order) { //natural order handled by TreeSet
                this.apprentices = new TreeSet<>();
            }
            else { //alternative order handled by MComparator
                this.apprentices = new TreeSet<>(new MComparator()); //we're passing our special Comparator
            }
        }
        else { //no sort - implementing HashSet
            this.apprentices = new HashSet<>();
        }
    }
    //getters
    public String getName() {
        return name;
    }

    public int getLevel() {
        return level;
    }

    public double getPower() {
        return power;
    }

    @Override
    public boolean equals(Object obj) { //comparing objects
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;

        Mage mage = (Mage) obj;

        return Double.compare(mage.power, power) == 0 && level == mage.level && Objects.equals(name, mage.name) && Objects.equals(apprentices, mage.apprentices);
    }

    @Override
    public int hashCode() { //hashing
        return Objects.hash(name, level, power, apprentices);
    }



    @Override
    public int compareTo(Mage o) { //function from interface Comparable, natural order - name, level, power
        if (o == null) {
            throw new NullPointerException();
        }
        if (this.getName().compareTo(o.getName()) != 0){
            return this.name.compareTo(o.name);
        }
        else if (this.level != o.level) {
            return Integer.compare(this.level, o.level);
        }
        else {
            return Double.compare(this.power, o.power);
        }
    }

    @Override
    public String toString() {
        return "Mage{" + "name='" + name + '\'' + ", level=" + level + ", power=" + power + '}';
    }

    public void AddMage(Mage mage) {
        this.apprentices.add(mage);
    }

    public void writeout(int depth) {
        for (int i=0;i<depth;i++) {
            System.out.print('-');
        }
        System.out.println(this.toString());

        for (Mage mages : this.apprentices) {
            mages.writeout(depth+1);
        }
    }

    public Map<Mage, Integer> generateApprenticeStatistics() {
        Map<Mage, Integer> statisticsMap = null;
        if (type) {
            statisticsMap = new TreeMap<>();
        }
        else {
            statisticsMap = new HashMap<>();
        }
        generateApprenticeStatisticsRekurencja(this, statisticsMap);
        printStatisticsMap(statisticsMap);
        return statisticsMap;
    }

    private int generateApprenticeStatisticsRekurencja(Mage mage, Map<Mage, Integer> statisticsMap) {
        int totalApprentices = mage.apprentices.size();
        for (Mage apprentice : mage.apprentices) {
            totalApprentices += generateApprenticeStatisticsRekurencja(apprentice, statisticsMap); //jako że returnujemy wartość oprócz aktualizowania jej w mapie, możemy zliczać liczbę potomków w ten sposób
        }
        statisticsMap.put(mage, totalApprentices);
        return totalApprentices;
    }

    private void printStatisticsMap(Map<Mage, Integer> statisticsMap) {
        for (Map.Entry<Mage, Integer> e : statisticsMap.entrySet()) {
            System.out.println("Mage: " + e.getKey().getName() + "; Sum of Apprentices: " + e.getValue()); //mage as a key, number of descendant as value in map
        }
    }
}
