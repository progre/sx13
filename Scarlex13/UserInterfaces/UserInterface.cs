using System;
using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Games;
using Progressive.Scarlex13.UserInterfaces.Logos;
using Progressive.Scarlex13.UserInterfaces.Titles;
using Progressive.Scarlex13.Domains.Applications;

namespace Progressive.Scarlex13.UserInterfaces
{
    internal class UserInterface
    {
        private ShootingGame _game = new ShootingGame();
        private readonly StageEditor _stageEditor = new StageEditor();
        private readonly RealtimeInput _input = new RealtimeInput();
        private readonly Logo _logo = new Logo();
        private readonly Renderer _renderer = new Renderer();
        private readonly SoundManager _soundManager = new SoundManager();
        private Title _title;
        private ShootingGameView _view;
        private Func<bool> _current;

        public UserInterface()
        {
            _view = new ShootingGameView(_game)
            {
                Renderer = _renderer,
                SoundManager = _soundManager
            };
            _title = new Title
            {
                Renderer = _renderer,
                SoundManager = _soundManager
            };
            _stageEditor = new StageEditor
            {
                Renderer = _renderer,
            };
        }

        public void Main()
        {
            _current = DoLogo;
            new Messaging().MessageLoop(() => _current());
        }

        private bool DoLogo()
        {
            _logo.Render(_renderer);
            _renderer.Flip();
            if (!_logo.Done) return true;
            _current = DoTitle;
            return true;
        }

        private bool DoTitle()
        {
            Input input = _input.GetInput();
            _title.Render(input);
            _renderer.Flip();
            if (_title.Start)
            {
                _current = DoGame;
                _title = new Title
                {
                    Renderer = _renderer,
                    SoundManager = _soundManager
                };

                return true;
            }
            if (_title.Edit)
            {
                _current = DoEditor;
                return true;
            }
            return !_title.Exit;
        }

        private bool DoGame()
        {
            Input input = _input.GetInput();
            _game.Update(input);
            _view.Render();
            _renderer.Flip();
            if (DxLibDLL.DX.CheckHitKey(DxLibDLL.DX.KEY_INPUT_ESCAPE) == DxLibDLL.DX.TRUE)
            {
                DxLibDLL.DX.StopSound();
                DxLibDLL.DX.StopMusic();
                _current = DoTitle;
                _game = new ShootingGame();
                _view = new ShootingGameView(_game)
                {
                    Renderer = _renderer,
                    SoundManager = _soundManager
                };
            }
            return true;
        }

        private bool DoEditor()
        {
            _stageEditor.Update(_input.GetInput());
            _stageEditor.Render();
            _renderer.Flip();
            return true;
        }
    }
}