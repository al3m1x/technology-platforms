package org.example;

import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;


public class MyRunnable implements Runnable {

    private final Socket clientSocket;
    private final Logger logger = Logger.getLogger(MyRunnable.class.getName());

    private boolean isFinished = false;
    public MyRunnable(Socket clientSocket) {
        this.clientSocket = clientSocket;
    }

    public boolean isFinished() {
        return isFinished;
    }

    @Override
    public void run() {
        try {
            ObjectOutputStream out = new ObjectOutputStream(clientSocket.getOutputStream());
            ObjectInputStream in = new ObjectInputStream(clientSocket.getInputStream());

            Integer number = (Integer) in.readInt();
            logger.log(Level.INFO, "Wątek: " + Thread.currentThread().getId());
            logger.log(Level.INFO, "Otrzymano wartość ilości wiadomości od klienta: " + number);
            String response = new String("Gotowy na wiadomości.");
            out.writeObject(response);
            out.flush();
            logger.log(Level.INFO, "Zarządzono gotowość na odebranie wiadomości od klienta: " + response);

            for (int i=0;i<number;i++) {
                Message message = (Message) in.readObject();
                logger.log(Level.INFO, "Otrzymano wiadomość numer " + (message.getNumber() + 1) + " od klienta: " + message.getContent());
            }

            response = new String("Zakończono odbieranie wiadomości.");
            out.writeObject(response);
            out.flush();

            //isFinished = true;


            out.close();
            in.close();
            clientSocket.close();
            Thread.currentThread().interrupt();
        }
        catch (Exception exc) {
            logger.log(Level.SEVERE, "Wystąpił błąd podczas obsługi połączenia klienta.", exc);
        }
    }
}
