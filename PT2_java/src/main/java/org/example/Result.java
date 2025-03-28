package org.example;

public class Result {
    private int ID;
    private double pi;
    private double percent;

    public int getID() {
        return ID;
    }

    public double getPi() {
        return pi;
    }

    public double getPercent() {
        return percent;
    }

    public void setID(int ID) {
        this.ID = ID;
    }

    public void setPi(double pi) {
        this.pi = pi;
    }

    public void setPercent(double percent) {
        this.percent = percent;
    }

    @Override
    public String toString() {
        return "Result{" +
                "ID=" + ID +
                ", pi=" + pi +
                ", percent=" + percent +
                "%}\n";
    }

    public Result(int ID, double pi, double percent) {
        this.ID = ID;
        this.pi = pi;
        this.percent = percent;
    }
}
