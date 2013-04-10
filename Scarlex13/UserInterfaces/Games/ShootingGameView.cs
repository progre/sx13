using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;
using System;

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
            RenderUi(_game);
            if (_worldView == null || !_worldView.EqualsWorld(_game.World))
            {
                _worldView = new ShootingWorldView(_game.World)
                {
                    Renderer = Renderer,
                    SoundManager = SoundManager
                };
            }
            if (!_game.Cleared)
            {
                _worldView.Render(_game.World);
            }
            else
            {
                _worldView.RenderWarp(_game.World);
                RenderStageClear(_game.StageNo, _game.HitRatioPercent, _game.BonusTime);
            }
        }

        private void RenderUi(ShootingGame game)
        {
            Renderer.DrawText("SCARLEX   ", new Point(553, 20), new Color(255, 0, 0));
            Renderer.DrawText("       '13", new Point(553, 20), new Color(0, 160, 0));
            Renderer.DrawText("TIME", new Point(613, 80), new Color(255, 0, 0));
            Renderer.DrawText("SCORE", new Point(603, 180), new Color(255, 0, 0));
            Renderer.DrawText("LEVEL", new Point(603, 280), new Color(255, 0, 0));
            Renderer.DrawText("MISS", new Point(613, 380), new Color(255, 0, 0));

            Renderer.DrawText(game.Time.ToString("mm\\'ss\\\"ff"), new Point(573, 120), new Color(255, 255, 255));
            Renderer.DrawText(game.Score.ToString("000000"), new Point(603, 220), new Color(255, 255, 255));
            Renderer.DrawText((game.StageNo + 1).ToString("00"), new Point(633, 320), new Color(255, 255, 255));
            Renderer.DrawText("00", new Point(633, 420), new Color(255, 255, 255));
        }

        private void RenderStageClear(int stageNo, int hitRatio, int bonus)
        {
            Renderer.DrawText("LEVEL    CLEAR!!", new Point(100, 200), new Color(255, 255, 0));
            Renderer.DrawText("      " + (stageNo + 1).ToString("00"), new Point(100, 200), new Color(255, 255, 255));
            Renderer.DrawText("HIT RATIO =", new Point(110, 260), new Color(255, 255, 0));
            Renderer.DrawText("            " + hitRatio + "%", new Point(100, 260), new Color(255, 255, 255));
            if (bonus > 0)
                Renderer.DrawText("BONUS TIME +" + bonus + "sec.",
                    new Point(80, 340), new Color(0, 255, 0));
            Renderer.DrawText("WARP TO NEXT LEVEL!!", new Point(60, 420), new Color(0, 255, 255));
        }
    }
}