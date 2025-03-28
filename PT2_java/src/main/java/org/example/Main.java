package org.example;

import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        int numberOfThreads = 5;
        List<Thread> threadList = new ArrayList<>();
        TaskHandler taskHandler = new TaskHandler();
        ResultHandler resultHandler = new ResultHandler();

        for (int i=0;i<numberOfThreads;i++) {
            Thread thread = new Thread(new MyRunnable(taskHandler, resultHandler));
            threadList.add(thread);
            thread.start();
        }

        Scanner scanner = new Scanner(System.in);

        System.out.println("Counting values of PI.");
        int iter=1;
        while (true) {

            String input = scanner.nextLine();
            if (input.equalsIgnoreCase("quit")) {
                System.out.println(resultHandler);
                break;
            }
            try {
                //Task task = new Task(iter, iter);
                int n = Integer.parseInt(input);
                taskHandler.SetTask(iter, n);
                iter++;
            } catch (NumberFormatException e) {
                System.out.println("Error");
            }


        }
        for (Thread thread : threadList) {
            thread.interrupt();
        }
        scanner.close();

        System.out.println("All threads interrupted. Program shutting down.");
    }
}