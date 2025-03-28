package org.example;

import java.util.LinkedList;
import java.util.Queue;

public class TaskHandler {
    private Queue<Task> taskHandler;

    public TaskHandler() {
        taskHandler = new LinkedList<>();
    }

    public synchronized Task GetTask() throws InterruptedException {
        while(taskHandler.isEmpty()) {
            wait();
        }
        return taskHandler.poll();
    }

    public synchronized void SetTask(int ID, int n) {
        Task task = new Task(ID, n);
        taskHandler.add(task);
        notify();
    }
}
