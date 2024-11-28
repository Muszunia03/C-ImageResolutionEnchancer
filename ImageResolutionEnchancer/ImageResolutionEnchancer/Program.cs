using System;
using System.Drawing;
using System.IO;

class ImageEnhancer
{
    static void Main(string[] args)
    {
        try
        {
            // Load the input image
            string inputPath = @"C:\gry\spognebob.jpeg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input file not found!");
                return;
            }

            Bitmap originalImage = new Bitmap(inputPath);

            // Get the new dimensions
            Console.WriteLine("Enter the scaling factor (e.g., 2 for double size):");
            double scale = double.Parse(Console.ReadLine());
            int newWidth = (int)(originalImage.Width * scale);
            int newHeight = (int)(originalImage.Height * scale);

            // Choose interpolation method
            Console.WriteLine("Choose interpolation method: 1 for Nearest-Neighbor, 2 for Bilinear:");
            int choice = int.Parse(Console.ReadLine());

            Bitmap resizedImage = null;

            if (choice == 1)
            {
                resizedImage = NearestNeighborInterpolation(originalImage, newWidth, newHeight);
            }
            else if (choice == 2)
            {
                resizedImage = BilinearInterpolation(originalImage, newWidth, newHeight);
            }
            else
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            // Set output path
            string outputFolder = @"C:\gry";
            string outputPath = Path.Combine(outputFolder, "outputImage.jpg");

            // Ensure the output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Save the resized image
            resizedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            Console.WriteLine($"Image saved successfully at: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // Nearest-Neighbor Interpolation
    static Bitmap NearestNeighborInterpolation(Bitmap original, int newWidth, int newHeight)
    {
        Bitmap resized = new Bitmap(newWidth, newHeight);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                // Map pixel coordinates to original image
                int srcX = (int)(x * (double)original.Width / newWidth);
                int srcY = (int)(y * (double)original.Height / newHeight);

                // Get the nearest pixel
                Color pixelColor = original.GetPixel(srcX, srcY);

                // Set the pixel in the new image
                resized.SetPixel(x, y, pixelColor);
            }
        }

        return resized;
    }

    // Bilinear Interpolation
    static Bitmap BilinearInterpolation(Bitmap original, int newWidth, int newHeight)
    {
        Bitmap resized = new Bitmap(newWidth, newHeight);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                // Map pixel coordinates to original image
                double srcX = x * (double)(original.Width - 1) / (newWidth - 1);
                double srcY = y * (double)(original.Height - 1) / (newHeight - 1);

                int x1 = (int)srcX;
                int y1 = (int)srcY;
                int x2 = Math.Min(x1 + 1, original.Width - 1);
                int y2 = Math.Min(y1 + 1, original.Height - 1);

                double dx = srcX - x1;
                double dy = srcY - y1;

                // Interpolate pixel values
                Color c11 = original.GetPixel(x1, y1);
                Color c21 = original.GetPixel(x2, y1);
                Color c12 = original.GetPixel(x1, y2);
                Color c22 = original.GetPixel(x2, y2);

                int r = (int)((1 - dx) * (1 - dy) * c11.R + dx * (1 - dy) * c21.R +
                              (1 - dx) * dy * c12.R + dx * dy * c22.R);
                int g = (int)((1 - dx) * (1 - dy) * c11.G + dx * (1 - dy) * c21.G +
                              (1 - dx) * dy * c12.G + dx * dy * c22.G);
                int b = (int)((1 - dx) * (1 - dy) * c11.B + dx * (1 - dy) * c21.B +
                              (1 - dx) * dy * c12.B + dx * dy * c22.B);

                // Set the pixel in the new image
                resized.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        return resized;
    }
}
