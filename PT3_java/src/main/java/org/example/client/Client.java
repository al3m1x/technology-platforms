package org.example.client;

import org.example.Message;

import java.net.Socket;

import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;


public class Client {
    private static final int SERVER_PORT = 15243;
    private static final String SERVER_ADDRESS = "localhost";
    private static final Logger logger = Logger.getLogger(Client.class.getName());
    private static boolean isValid = false;

    private static int number;

    public static void main(String[] args) {
        try {
            Scanner scanner = new Scanner(System.in);
            do {
                try {
                    System.out.println("Ile wiadomości chcesz przesłać do serwera? (integer)");
                    number = scanner.nextInt();
                    isValid = true;
                } catch (Exception exc) {
                    System.out.println("Niepoprawny format wczytanych danych.");
                    scanner.next();
                }
            }
            while (isValid == false);

            Socket socket = new Socket(SERVER_ADDRESS, SERVER_PORT);
            logger.log(Level.INFO, "Połączono z serwerem.");

            ObjectOutputStream out = new ObjectOutputStream(socket.getOutputStream());
            ObjectInputStream in = new ObjectInputStream(socket.getInputStream());

            out.writeInt(number);
            out.flush();
            logger.log(Level.INFO, "Wysłano liczbę do serwera (" + number + ").");

            String response1 = (String) in.readObject();
            logger.log(Level.INFO, "Otrzymano odpowiedź od serwera: " + response1);

            scanner.nextLine();

            for(int i=0;i<number;i++) {
                String text = scanner.nextLine();
                Message message = new Message(i, text);
                out.writeObject(message);
                out.flush();
                logger.log(Level.INFO, "Wysłano Message do serwera (" + message.getContent() + ").");
            }

            String response2 = (String) in.readObject();
            logger.log(Level.INFO, "Otrzymano odpowiedź od serwera: " + response2);

            Thread.sleep(10000);

            out.close();
            in.close();
            socket.close();
        }
        catch (Exception exc) {
            logger.log(Level.SEVERE, "Wystąpił błąd podczas komunikacji z serwerem.", exc);
        }
    }

}
