package org.example;
import java.util.List;
import javax.persistence.*;
import java.util.Scanner;

public class Main {

    public static void startingTowersAndMages(EntityManager em) {
        EntityTransaction et = em.getTransaction();
        et.begin();

        Tower tower1 = new Tower("Tower1", 10);
        Mage mage1_1 = new Mage("Mage1_1", 5, tower1);
        Mage mage1_2 = new Mage("Mage1_2", 6, tower1);
        Mage mage1_3 = new Mage("Mage1_3", 7, tower1);
        tower1.addMage(mage1_1);
        tower1.addMage(mage1_2);
        tower1.addMage(mage1_3);
        em.persist(mage1_1);
        em.persist(mage1_2);
        em.persist(mage1_3);

        Tower tower2 = new Tower("Tower2", 15);
        Mage mage2_1 = new Mage("Mage2_1", 8, tower2);
        Mage mage2_2 = new Mage("Mage2_2", 9, tower2);
        Mage mage2_3 = new Mage("Mage2_3", 10, tower2);
        tower2.addMage(mage2_1);
        tower2.addMage(mage2_2);
        tower2.addMage(mage2_3);
        em.persist(mage2_1);
        em.persist(mage2_2);
        em.persist(mage2_3);

        Tower tower3 = new Tower("Tower3", 20);
        Mage mage3_1 = new Mage("Mage3_1", 11, tower3);
        Mage mage3_2 = new Mage("Mage3_2", 12, tower3);
        Mage mage3_3 = new Mage("Mage3_3", 13, tower3);
        tower3.addMage(mage3_1);
        tower3.addMage(mage3_2);
        tower3.addMage(mage3_3);
        em.persist(mage3_1);
        em.persist(mage3_2);
        em.persist(mage3_3);

        em.persist(tower1);
        em.persist(tower2);
        em.persist(tower3);
        et.commit();
    }

    public static void getMagesWithHigherLevel(EntityManager em, int level) {
        TypedQuery<Mage> query = em.createQuery("SELECT m FROM Mage m WHERE m.level > :level", Mage.class);
        query.setParameter("level", level);

        List<Mage> resultList = query.getResultList();
        for (Mage mage : resultList) {
            System.out.println("Mage: " + mage.getName() + ", Level: " + mage.getLevel());
        }
    }

    public static void printTowersWithLowerHeight(EntityManager em, int height) {
        TypedQuery<Tower> query = em.createQuery("SELECT t FROM Tower t WHERE t.height < :height", Tower.class);
        query.setParameter("height", height);

        List<Tower> resultList = query.getResultList();
        for (Tower tower : resultList) {
            System.out.println("Tower: " + tower.getName() + ", Height: " + tower.getHeight());
        }
    }

    public static void printMagesWithHigherLevelFromTower(EntityManager em, String towerName, int level) {
        TypedQuery<Mage> query = em.createQuery("SELECT m FROM Mage m WHERE m.level > :level AND m.tower.name = :towerName", Mage.class);
        query.setParameter("level", level);
        query.setParameter("towerName", towerName);

        List<Mage> resultList = query.getResultList();
        for (Mage mage : resultList) {
            System.out.println("Mage: " + mage.getName() + ", Level: " + mage.getLevel() + ", Tower: " + mage.getTower().getName());
        }
    }

    public static void main(String[] args) {
        EntityManagerFactory factory = Persistence.createEntityManagerFactory("persistenceUnitName");
        EntityManager em = factory.createEntityManager();
        Scanner scanner = new Scanner(System.in);

        startingTowersAndMages(em);

        while(true) {
            System.out.println("\nWybierz opcję:");
            System.out.println("1. Dodaj wieżę: at");
            System.out.println("2. Dodaj maga: am");
            System.out.println("3. Usuń wieżę: rt");
            System.out.println("4. Usuń maga: rm");
            System.out.println("5. Wyświetl wszystkie wieże i magów: p");
            System.out.println("6. Wyjście: q\n");

            String command = scanner.nextLine();

            switch(command) {
                case "at":
                    System.out.println("Podaj nazwę wieży:");
                    String name = scanner.nextLine();
                    System.out.println("Podaj wysokość wieży:");
                    int height = scanner.nextInt();
                    scanner.nextLine();

                    EntityTransaction etAddTower = em.getTransaction();
                    etAddTower.begin();
                    Tower tower = new Tower(name, height);
                    em.persist(tower);
                    etAddTower.commit();
                    System.out.println("Dodano nową wieżę.");
                    break;

                case "am":
                    System.out.println("Podaj imię maga:");
                    String mageName = scanner.nextLine();
                    System.out.println("Podaj poziom maga:");
                    int mageLevel = scanner.nextInt();
                    scanner.nextLine();
                    System.out.println("Podaj nazwę wieży, do której ma należeć mag:");
                    String towerName = scanner.nextLine();

                    Tower foundTower = em.find(Tower.class, towerName);
                    if (foundTower != null) {
                        EntityTransaction etAddMage = em.getTransaction();
                        etAddMage.begin();
                        Mage mage = new Mage(mageName, mageLevel, foundTower);
                        foundTower.addMage(mage);
                        em.persist(mage);
                        etAddMage.commit();
                        System.out.println("Dodano nowego maga.");
                    }
                    else {
                        System.out.println("Nie znaleziono wieży o podanej nazwie.");
                    }
                    break;

                case "p":
                    TypedQuery<Tower> towerQuery = em.createQuery("SELECT t FROM Tower t", Tower.class);
                    List<Tower> towers = towerQuery.getResultList();
                    for (Tower t : towers) {
                        System.out.println("Tower: " + t.getName() + " (Height: " + t.getHeight() + " meters)");

                        for (Mage m : t.getMages()) {
                            System.out.println(" -> Mage: " + m.getName() + " (Level: " + m.getLevel() + ")");
                        }
                    }
                    break;

                case "rt":
                    System.out.println("Podaj nazwę wieży do usunięcia:");
                    String towerNameToDelete = scanner.nextLine();

                    Tower towerToDelete = em.find(Tower.class, towerNameToDelete);
                    if (towerToDelete != null) {
                        EntityTransaction etRemoveTower = em.getTransaction();
                        etRemoveTower.begin();
                        em.remove(towerToDelete);
                        etRemoveTower.commit();
                        System.out.println("Usunięto wieżę: " + towerNameToDelete);
                    } else {
                        System.out.println("Nie znaleziono wieży o podanej nazwie.");
                    }
                    break;

                case "rm":
                    System.out.println("Podaj imię maga do usunięcia:");
                    String mageToDelete = scanner.nextLine();

                    TypedQuery<Mage> mageQuery = em.createQuery("SELECT m FROM Mage m WHERE m.name = :name", Mage.class);
                    mageQuery.setParameter("name", mageToDelete);
                    List<Mage> magesToDelete = mageQuery.getResultList();

                    if (!magesToDelete.isEmpty()) {
                        EntityTransaction etRemoveMage = em.getTransaction();
                        etRemoveMage.begin();
                        for (Mage mage : magesToDelete) {
                            mage.getTower().removeMage(mage);
                            em.remove(mage);
                        }
                        etRemoveMage.commit();
                        System.out.println("Usunięto magów o imieniu: " + mageToDelete);
                    } else {
                        System.out.println("Nie znaleziono maga o podanym imieniu.");
                    }
                    break;

                case "q":
                    getMagesWithHigherLevel(em, 10);
                    System.out.println("\n");
                    printTowersWithLowerHeight(em, 11);
                    System.out.println("\n");
                    printMagesWithHigherLevelFromTower(em, "Tower1", 6);

                    em.close();
                    factory.close();
                    System.exit(0);
                    break;

                default:
                    System.out.println("Niepoprawna opcja.");
            }
        }
    }
}