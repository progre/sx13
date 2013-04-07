using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Games
{
    internal class ShootingGameView
    {
        private readonly ShootingGame _game;
        private bool _playedMusic;
        private ShootingWorldView _worldView;

        public ShootingGameView(ShootingGame game)
        {
            _game = game;
        }

        public Renderer Renderer { private get; set; }
        public SoundManager SoundManager { private get; set; }

        public void Render()
        {
            if (!_playedMusic)
            {
                SoundManager.PlayMusic("bgm.mp3");
                _playedMusic = true;
            }
            Renderer.Draw("back.png", new Point());
            RenderUi();
            if (_worldView == null || !_worldView.EqualsWorld(_game.World))
            {
                _worldView = new ShootingWorldView(_game.World)
                {
                    Renderer = Renderer,
                    SoundManager = SoundManager
                };
            }
            if (!_game.Cleared)
                _worldView.Render(_game.World);
            else
                _worldView.RenderWarp(_game.World);
        }

        private void RenderUi()
        {
            Renderer.DrawText("SCARLEX   ", new Point(553, 20), new Color(255, 0, 0));
            Renderer.DrawText("       '13", new Point(553, 20), new Color(0, 160, 0));
            Renderer.DrawText("TIME", new Point(613, 80), new Color(255, 0, 0));
            Renderer.DrawText("SCORE", new Point(603, 180), new Color(255, 0, 0));
            Renderer.DrawText("LEVEL", new Point(603, 280), new Color(255, 0, 0));
            Renderer.DrawText("MISS", new Point(613, 380), new Color(255, 0, 0));

            Renderer.DrawText("00'00\"00", new Point(573, 120), new Color(255, 255, 255));
            Renderer.DrawText("00000", new Point(603, 220), new Color(255, 255, 255));
            Renderer.DrawText("00", new Point(633, 320), new Color(255, 255, 255));
            Renderer.DrawText("00", new Point(633, 420), new Color(255, 255, 255));
        }
    }
}