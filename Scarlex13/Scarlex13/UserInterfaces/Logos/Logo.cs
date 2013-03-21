using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.UserInterfaces.Logos
{
    internal class Logo
    {
        private int _time = -1;

        public bool Done { get; private set; }

        public void Render(Renderer renderer)
        {
            _time++;
            byte alpha = 0;
            const int scene1 = 25;
            const int scene2 = 100;
            const int scene3 = 25;
            if (_time < scene1)
            {
                alpha = (byte)(255 * _time / scene1);
            }
            else if (_time < scene1 + scene2)
            {
                alpha = 255;
            }
            else if (_time < scene1 + scene2 + scene3)
            {
                int localTime = _time - (scene1 + scene2);
                alpha = (byte)(255 - 255 * localTime / scene3);
            }
            else
            {
                Done = true;
            }
            renderer.Draw("Logo.png", new Point(), alpha);
        }
    }
}