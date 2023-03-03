using System.Drawing;
using System.Drawing.Imaging;

namespace SitronicsTest
{
    internal class Program
    {
        private const string Path = @"D:\Test";

        static void Main(string[] args)
        {
            Dictionary<string, Bitmap> images = LoadImagesAsBitmaps(Path);
            HitChecker hitCheker = new(10, 1);

            Console.Write("Result:\n\t");
            foreach (KeyValuePair<string, Bitmap> pair in images)
            {
                string filename = pair.Key;
                Bitmap bitmap = pair.Value;

                Coordinate? center = hitCheker.FindHitCenter(bitmap);

                if (center == null)
                {
                    continue;
                }

                Console.Write($"{GetShortFilename(filename)},{center};");
            }
        }

        private static Dictionary<string, Bitmap> LoadImagesAsBitmaps(string path)
        {
            Dictionary<string, Bitmap> images = new();

            Console.WriteLine("Files opened:");
            foreach (string filename in Directory.EnumerateFiles(path, "*.png", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine($"\t{filename}");
                Bitmap bitmap = new Bitmap(filename);
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    throw new Exception("Not suitable image format.");
                }
                images[filename] = bitmap;
            }

            return images;
        }

        private static string GetShortFilename(string filename)
        {
            return filename.Split('\\').Last().Split('.').First();
        }
    }
}