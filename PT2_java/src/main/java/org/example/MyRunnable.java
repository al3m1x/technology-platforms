package org.example;

public class MyRunnable implements Runnable {

    private ResultHandler resultHandler;
    private TaskHandler taskHandler;

    private Result myResult;
    public MyRunnable(TaskHandler taskHandler, ResultHandler resultHandler) {
        this.resultHandler = resultHandler;
        this.taskHandler = taskHandler;
    }

    @Override
    public void run() {
        while (true) {
            try {
                Task myTask = taskHandler.GetTask();
                Result result = CountPi(myTask);

                resultHandler.SetResult(result);
                System.out.println("Wątek: " + Thread.currentThread().getId());

            }
            catch (InterruptedException e) {
                break;
            }
        }
    }

    private Result CountPi(Task task) throws InterruptedException {
        int ID = task.getID();
        double n = task.getN();
        double result = 0;
        double percent = 0;
        double iter=0;

        for (int i=1;i<=n;i++) {
            try {
            result += (Math.pow(-1, i - 1))/((2*i)-1);
            iter++;
                Thread.sleep(30);
            } catch (InterruptedException e) {
                percent = iter/n;
                myResult = new Result(ID, 4*result, percent*100);
                System.out.println(myResult);
                return myResult;
            }

        }

        try {
            Thread.sleep(2000);
            percent = iter/n;
            myResult = new Result(ID, 4*result, percent*100);
            System.out.println(myResult);
        } catch (InterruptedException e) {
            percent = iter/n;
            myResult = new Result(ID, 4*result, percent*100);
            System.out.println(myResult);
        }


        return myResult;
    }
}
