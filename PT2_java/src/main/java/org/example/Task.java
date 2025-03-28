package org.example;

public class Task {
    private int ID;
    private int n;

    public int getID() {
        return ID;
    }

    public int getN() {
        return n;
    }

    public void setID(int ID) {
        this.ID = ID;
    }

    public void setN(int n) {
        this.n = n;
    }

    public Task(int ID, int n) {
        this.ID = ID;
        this.n = n;
    }
}
