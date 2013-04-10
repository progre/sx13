using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Titles
{
    internal class Title
    {
        private const bool DebugMode = true;
        private const int Scene1 = 10;
        private const int Scene2 = 80;
        private const int Scene3 = 20;
        private bool _ranking;
        private int _selection;
        private int _time = -1;

        public Renderer Renderer { private get; set; }
        public SoundManager SoundManager { private get; set; }
        public bool Start { get; private set; }
        public bool Edit { get; private set; }
        public bool Exit { get; private set; }

        public void Render(Input input)
        {
            if (DebugMode && input.Pause)
            {
                Edit = true;
                return;
            }
            _time++;
            if (_time < Scene1 + Scene2 + Scene3)
            {
                RenderPreTitleLogo(Renderer);
                return;
            }

            if (!_ranking)
            {
                if (input.DirectionToggled)
                {
                    switch (input.Direction)
                    {
                        case 2:
                            _selection++;
                            if (_selection > 2)
                                _selection = 0;
                            break;
                        case 8:
                            _selection--;
                            if (_selection < 0)
                                _selection = 2;
                            break;
                    }
                }
                if (input.ShotToggled && input.Shot)
                {
                    switch (_selection)
                    {
                        case 0:
                            Start = true;
                            break;
                        case 1:
                            _ranking = true;
                            break;
                        case 2:
                            Exit = true;
                            break;
                    }
                }
            }
            else
            {
                if (input.ShotToggled && input.Shot)
                    _ranking = false;
            }

            if (!_ranking)
            {
                Renderer.Draw("title2.png", new Point());
                Renderer.Draw("title1.png", new Point());
                RenderMenu(Renderer);
            }
            else
            {
                Renderer.Draw("title2.png", new Point(), 127);
                Renderer.Draw("title1.png", new Point(), 127);
                Renderer.DrawText("Ranking not found...", new Point(),
                    new Color(255, 255, 255));
            }
        }

        private void RenderPreTitleLogo(Renderer renderer)
        {
            if (_time < Scene1)
            {
                return;
            }
            if (_time < Scene1 + Scene2)
            {
                if (_time == Scene1)
                    SoundManager.Play("miss.ogg");

                renderer.Draw("title1.png", new Point());
                return;
            }
            if (_time == Scene1 + Scene2)
                SoundManager.Play("miss.ogg");

            renderer.Draw("title2.png", new Point());
            renderer.Draw("title1.png", new Point());
            RenderMenu(renderer);
        }

        private void RenderMenu(Renderer renderer)
        {
            var selectionColor = new Color(0, 255, 255);
            var unselectionColor = new Color(0, 0, 255);
            renderer.DrawText("GAME START", new Point { X = 354, Y = 400 },
                _selection == 0 ? selectionColor : unselectionColor);
            renderer.DrawText("  RECORD  ", new Point { X = 354, Y = 420 },
                _selection == 1 ? selectionColor : unselectionColor);
            renderer.DrawText("   EXIT   ", new Point { X = 354, Y = 440 },
                _selection == 2 ? selectionColor : unselectionColor);
        }
    }
}