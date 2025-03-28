package org.example;
import org.apache.commons.lang3.tuple.Pair;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.ForkJoinPool;
import java.util.stream.Collectors;
import java.util.stream.Stream;


public class Main {

    private static final String in_dir = "C:\\Users\\al3m1x\\IdeaProjects\\PT6\\src\\Photos2";
    private static final String out_dir = "C:\\Users\\al3m1x\\IdeaProjects\\PT6\\src\\EditedPhotos2";
    private static final int number_of_threads = 8;

    public static void main(String[] args) {
        try {
            long time = System.currentTimeMillis();
            ForkJoinPool myThreads = new ForkJoinPool(number_of_threads);
            List<Pair<String, BufferedImage>> imagePairs = myThreads.submit(() -> Files.list(Path.of(in_dir)).parallel().map(Main::loadImage).collect(Collectors.toList())).get(); // paralell pozwala na przetwarzanie równoległe w potoku
            System.out.println("Loaded " + imagePairs.size() + " images"); // map(loadImage) powoduje że dla każdego pliku z listy wykonywana jest podana funkcja wczytująca ten plik

            myThreads.submit(() -> imagePairs.parallelStream().map(Main::transformImage).forEach(Main::saveImage)).get(); // get() blokuje bieżący wątek, aż wszystkie zadania w puli wątków zostaną zakończone
            System.out.println("Saved all modified images");
            System.out.println(System.currentTimeMillis() - time + " ms");
        } catch (Exception e) {
            System.out.println("Failed to process images: " + e.getMessage());
        }
    }

    private static Pair<String, BufferedImage> loadImage(Path path) {
        try {
            BufferedImage image = ImageIO.read(path.toFile());
            String name = path.getFileName().toString();
            System.out.println("Loaded image: " + name);
            return Pair.of(name, image);
        } catch (IOException e) {
            System.out.println("Error while loading image: " + path);
            return null;
        }
    }

    private static Pair<String, BufferedImage> transformImage(Pair<String, BufferedImage> pair) {
        String name = pair.getLeft();
        BufferedImage original = pair.getRight();
        if (original == null) {
            System.out.println("Error while transforming image: " + name);
            return null;
        }
        BufferedImage transformedImage = new BufferedImage(original.getWidth(), original.getHeight(), original.getType());
        int width = transformedImage.getWidth();
        int height = transformedImage.getHeight();

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                int rgb = original.getRGB(i, height - j - 1);
                Color color = new Color(rgb);
                int red = color.getRed();
                int green = color.getGreen();
                int blue = color.getBlue();
                Color outColor = new Color(red, blue, green);
                int outRgb = outColor.getRGB();
                transformedImage.setRGB(i, j, outRgb);
            }
        }

        return Pair.of(name, transformedImage);
    }

    private static void saveImage(Pair<String, BufferedImage> pair) {
        String name = pair.getLeft();
        BufferedImage image = pair.getRight();
        if (image == null) {
            System.out.println("Error while saving image: " + name);
            return;
        }
        Path outputPath = Path.of(out_dir, name);
        try {
            ImageIO.write(image, "jpg", outputPath.toFile());
            System.out.println("Saved image: " + outputPath);
        } catch (IOException e) {
            System.out.println("Error while saving image: " + outputPath);
        }
    }
}