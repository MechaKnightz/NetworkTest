namespace GameLibrary
{
    public class Resolution
    {
        public Resolution(int width, int height, float aspectRatio = 0)
        {
            Width = width;
            Height = height;
            AspectRatio = aspectRatio;
        }

        public float AspectRatio { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
