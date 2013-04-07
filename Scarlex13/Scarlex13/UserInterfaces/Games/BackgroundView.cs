using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Games
{
    internal class BackgroundView
    {
        private readonly List<Star> _stars;

        public BackgroundView()
        {
            _stars = new List<Star>();
            var r = new Random();
            for (int i = 0; i < 50; i++)
            {
                const int light = 200;
                _stars.Add(new Star(
                    new Point((short)r.Next(Point.Width),
                        (short)r.Next(Point.Height)),
                    r.Next(1, 3),
                    new Color(
                        (byte)r.Next(light), (byte)r.Next(light), (byte)r.Next(light))));
            }
        }

        public Renderer Renderer { private get; set; }

        public void Extend()
        {
            foreach (Star star in _stars)
                star.Extend();
        }

        public void Warp()
        {
            foreach (Star star in _stars)
                star.Warp();
        }

        public void Render()
        {
            foreach (Star star in _stars)
            {
                star.Update();
                star.Render(Renderer);
            }
        }
    }
}