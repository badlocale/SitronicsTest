namespace SitronicsTest
{
    public record struct Coordinate(int x, int y)
    {
        public override string ToString()
        {
            return $"{x},{y}";
        }
    }
}
