namespace PhotoPong.Managers
{
    public static class Utils
    {
        public static WorldDirection Opposite(this WorldDirection dir)
        {
            return dir == WorldDirection.Left ? WorldDirection.Right : WorldDirection.Left;
        }
    }
    public enum WorldDirection
    {
        Left,Right
    }
}