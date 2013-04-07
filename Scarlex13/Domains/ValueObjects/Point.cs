namespace Progressive.Scarlex13.Domains.ValueObjects
{
    internal struct Point
    {
        public const int Width = 500;
        public const int Height = 500;

        public short X;
        public short Y;

        public Point(short x, short y)
        {
            X = x;
            Y = y;
        }

        public Point Shift(int x, int y)
        {
            return new Point((short)(X + x), (short)(Y + y));
        }
    }
}