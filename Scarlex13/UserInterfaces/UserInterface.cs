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
        private readonly ShootingGame _game = new ShootingGame();
        private readonly StageEditor _stageEditor = new StageEditor();
        private readonly RealtimeInput _input = new RealtimeInput();
        private readonly Logo _logo = new Logo();
        private readonly Renderer _renderer = new Renderer();
        private readonly SoundManager _soundManager = new SoundManager();
        private readonly Title _title;
        private readonly ShootingGameView _view;
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
        }

        public void Main()
        {
            _current = DoGame;
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
            return true;
        }

        private bool DoEditor()
        {
            return true;
        }
    }
}