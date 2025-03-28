package org.example;
import java.util.Random;

public class Main {
    public static void main(String[] args) {
        Random random = new Random();
        String sort1 = "alternative";
        if (sort1.length() != 0) {
            Mage mainMage = null;
            if (sort1.equals("nosort")) {
                mainMage = new Mage("Traton the Black", 143, 53283, false, false);
                for (int i=0;i<10;i++) {
                    int randomNum1 = random.nextInt(140) + 1;
                    int randomNum2 = random.nextInt(25000) + 1;
                    Mage mageChild = new Mage("Mage"+i, randomNum1, randomNum2, false, false);
                    mainMage.AddMage(mageChild);
                    int randomNum5 = random.nextInt(5) + 1;
                    for (int j=0;j<randomNum5;j++) {
                        int randomNum3 = random.nextInt(54) + 1;
                        int randomNum4 = random.nextInt(7100) + 1;
                        mageChild.AddMage(new Mage("Mage"+10*i+j, randomNum3, randomNum4, false, false));
                    }
                }
            }
            else if (sort1.equals("natural")) {
                mainMage = new Mage("Shalazam", 173, 81243, true, true);
                for (int i=0;i<10;i++) {
                    int randomNum1 = random.nextInt(160) + 1;
                    int randomNum2 = random.nextInt(29000) + 1;
                    Mage mageChild = new Mage("Mage"+i, randomNum1, randomNum2, true, true);
                    mainMage.AddMage(mageChild);
                    int randomNum5 = random.nextInt(5) + 1;
                    for (int j=0;j<randomNum5;j++) {
                        int randomNum3 = random.nextInt(60) + 1;
                        int randomNum4 = random.nextInt(8600) + 1;
                        mageChild.AddMage(new Mage("Mage"+10*i+j, randomNum3, randomNum4, true, true));
                    }
                }
            }
            else if (sort1.equals("alternative")) {
                mainMage = new Mage("Whitlock", 186, 109321, true, false);
                for (int i=0;i<10;i++) {
                    int randomNum1 = random.nextInt(170) + 1;
                    int randomNum2 = random.nextInt(34000) + 1;
                    Mage mageChild = new Mage("Mage"+i, randomNum1, randomNum2, true, false);
                    mainMage.AddMage(mageChild);
                    int randomNum5 = random.nextInt(5) + 1;
                    for (int j=0;j<randomNum5;j++) {
                        int randomNum3 = random.nextInt(75) + 1;
                        int randomNum4 = random.nextInt(11500) + 1;
                        mageChild.AddMage(new Mage("Mage"+10*i+j, randomNum3, randomNum4, true, false));
                    }
                }
            }

            //assert mainMage != null;
            mainMage.writeout(1);

            mainMage.generateApprenticeStatistics();

        }
    }
}