using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Games
{
    internal class ShootingGameView
    {
        private readonly ShootingGame _game;

        public ShootingGameView(ShootingGame game)
        {
            _game = game;
        }

        public Renderer Renderer { private get; set; }

        public void Render()
        {
            Renderer.Draw("back.png", new Point());
            RenderUi();
            Renderer.DrawClip("remilia.bmp", new Point(33, 98), new Size(33, 34),
                GetUnitPoint(_game.World.Player.Point));
        }

        private void RenderUi()
        {
            Renderer.DrawText("SCARLEX   ", new Point(560, 20), new Color(255, 0, 0));
            Renderer.DrawText("       '13", new Point(560, 20), new Color(0, 160, 0));
            Renderer.DrawText("TIME", new Point(620, 80), new Color(255, 0, 0));
            Renderer.DrawText("SCORE", new Point(610, 180), new Color(255, 0, 0));
            Renderer.DrawText("LEVEL", new Point(610, 280), new Color(255, 0, 0));
            Renderer.DrawText("MISS", new Point(620, 380), new Color(255, 0, 0));

            Renderer.DrawText("00'00\"00", new Point(580, 120), new Color(255, 255, 255));
            Renderer.DrawText("00000", new Point(610, 220), new Color(255, 255, 255));
            Renderer.DrawText("00", new Point(640, 320), new Color(255, 255, 255));
            Renderer.DrawText("00", new Point(640, 420), new Color(255, 255, 255));
        }

        private static Point GetUnitPoint(Point point)
        {
            point.X -= 17;
            point.Y -= 17;
            return point;
        }
    }
}