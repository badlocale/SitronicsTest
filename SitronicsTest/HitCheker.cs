using System.Drawing;

namespace SitronicsTest
{
    public class HitCheker
    {
        private readonly int Width;
        private readonly int Height;
        private readonly int Padding;
        private readonly int Offset;

        public HitCheker(int width, int height, int padding, int offset)
        {
            Width = width;
            Height = height;
            Padding = padding;
            Offset = offset;
        }

        public Coordinate? FindHitCenter(Bitmap bitmap)
        {
            for (int x = 0; x < Width; x += 1 + Offset)
            {
                for (int y = 0; y < Height; y += 1 + Offset)
                {
                    if (IsPixelMarked(bitmap, x, y))
                    {
                        IEnumerable<Coordinate> markeredPixels = FindMarkeredInArea(bitmap, x, y);

                        return CalculateAvarageCoordinates(markeredPixels);
                    }
                }
            }

            return null;
        }

        private IEnumerable<Coordinate> FindMarkeredInArea(Bitmap bitmap, int x, int y)
        {
            List<Coordinate> markedPixels = new();

            for (int localX = x - Padding; localX <= x + Padding; localX++)
            {
                for (int localY = y - Padding; localY <= y + Padding; localY++)
                {
                    if (localX < 0 || localX > bitmap.Width ||
                        localY < 0 || localY > bitmap.Height)
                    {
                        continue;
                    }

                    if (IsPixelMarked(bitmap, localX, localY))
                    {
                        markedPixels.Add(new(localX, localY));
                    }
                }
            }

            return markedPixels;
        }

        private Coordinate CalculateAvarageCoordinates(IEnumerable<Coordinate> coordinates)
        {
            int n = coordinates.Count();
            int x = 0;
            int y = 0;

            if (n == 0)
            {
                throw new Exception("No one coorditate passed.");
            }

            foreach (Coordinate coordinate in coordinates)
            {
                x += coordinate.x;
                y += coordinate.y;
            }
            x /= n;
            y /= n;

            return new(x, y);
        }

        private bool IsPixelMarked(Bitmap bitmap, int x, int y)
        {
            Color color = bitmap.GetPixel(x, y);
            return color.R != 0 || color.G != 0 || color.B != 0;
        }
    }
}
