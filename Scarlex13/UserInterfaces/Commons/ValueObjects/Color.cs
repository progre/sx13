namespace Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects
{
    internal struct Color
    {
        public byte Blue;
        public byte Green;
        public byte Red;

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}