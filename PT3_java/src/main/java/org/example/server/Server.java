package org.example.server;

import org.example.Message;
import org.example.MyRunnable;

import java.net.ServerSocket;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Server {
    private static final int SERVER_PORT = 15243;
    private static final Logger logger = Logger.getLogger(Server.class.getName());
    private static boolean isRunning = true;

    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = new ServerSocket(SERVER_PORT);
            logger.log(Level.INFO, "Serwer wystartował.");
            while(isRunning) {
                Socket clientSocket = serverSocket.accept();
                logger.log(Level.INFO, "Klient połączony: " + clientSocket.getInetAddress());
                Thread newClientThread = new Thread(new MyRunnable(clientSocket));
                newClientThread.start();
            }
        }
        catch (Exception exc) {
            logger.log(Level.SEVERE, "Wystąpił błąd podczas uruchamiania serwera.", exc);
        }
    }
}
