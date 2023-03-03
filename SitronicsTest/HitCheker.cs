using System.Drawing;

namespace SitronicsTest
{
    public class HitChecker
    {
        private readonly int Padding;
        private readonly int Offset;

        public HitChecker(int padding, int offset)
        {
            Padding = padding;
            Offset = offset;
        }

        //Function search laser spot on bitmap and returns center coordinate of the spot or null value
        //if there is no laser spot. To reduce the search time it is using the offset parameter, that
        //defines distance between the points to be checked. 
        public Coordinate? FindHitCenter(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x += 1 + Offset)
            {
                for (int y = 0; y < bitmap.Height; y += 1 + Offset)
                {
                    if (IsPixelMarked(bitmap, x, y))
                    {
                        IEnumerable<Coordinate> markeredPixels = FindMarkeredInArea(bitmap, x, y);

                        return CalculateCentralCoordinate(markeredPixels);
                    }
                }
            }

            return null;
        }

        //Finds the marked pixels in the area around the point on the bitmap. Search area size depends
        //on the padding parameter.
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

        //Finds the central coordinate of the coordinate collection.
        private Coordinate CalculateCentralCoordinate(IEnumerable<Coordinate> coordinates)
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

        //Checks whether the pixel is marked by a laser
        private bool IsPixelMarked(Bitmap bitmap, int x, int y)
        {
            Color color = bitmap.GetPixel(x, y);
            return color.R != 0 || color.G != 0 || color.B != 0;
        }
    }
}
