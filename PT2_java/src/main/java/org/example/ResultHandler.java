package org.example;

import java.util.LinkedList;
import java.util.Queue;
public class ResultHandler {
    private Queue<Result> resultHandler;
    public int iter_petla = 0;
    public ResultHandler() {
        resultHandler = new LinkedList<>();
    }

    public synchronized Result GetResult() {
        return resultHandler.poll();
    }

    public synchronized void SetResult(int ID, double pi, double percent) {
        Result ourResult = new Result(ID, pi, percent);
        resultHandler.add(ourResult);
    }

    public synchronized void SetResult(Result ourResult) {
        resultHandler.add(ourResult);
        iter_petla++;
    }

    @Override
    public String toString() {
        return "ResultHandler{" +
                "resultHandler=" + resultHandler +
                ", iter_petla=" + iter_petla +
                '}';
    }
}
