using DxLibDLL;
using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;
using Progressive.Scarlex13.UserInterfaces.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13.Domains.Applications
{
    internal class StageEditor
    {
        private readonly StageFactory _stageFactory
            = StageFactory.FromData(new File().GetExtraStages());
        private int _stageNo = -1;
        private ShootingWorld _world;
        private ShootingWorldView _view;

        public StageEditor()
        {
        }

        public Renderer Renderer { private get; set; }

        public void Update(Input input)
        {
            if (input.DirectionToggled)
            {
                switch (input.Direction)
                {
                    case 4:
                        _stageNo--;
                        if (_stageNo < 0)
                            _stageNo = 0;
                        _world = new ShootingWorld(
                            _stageFactory.GetEnemies(_stageNo));
                        _view = new ShootingWorldView(_world)
                        {
                            Renderer = Renderer
                        };
                        break;
                    case 6:
                        _stageNo++;
                        if (_stageNo > _stageFactory.LastStage)
                        {
                            _stageFactory.AddStage();
                        }
                        _world = new ShootingWorld(
                            _stageFactory.GetEnemies(_stageNo));
                        _view = new ShootingWorldView(_world)
                        {
                            Renderer = Renderer
                        };
                        break;
                }
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_1) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.AddEnemy(
                    _stageNo, EnemyType.Green, MergePoint(x, y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_2) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.AddEnemy(
                    _stageNo, EnemyType.Blue, MergePoint(x, y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_3) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.AddEnemy(
                    _stageNo, EnemyType.Red, MergePoint(x, y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_4) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.AddEnemy(
                    _stageNo, EnemyType.Silver, MergePoint(x, y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_5) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.AddEnemy(
                    _stageNo, EnemyType.Gold, MergePoint(x, y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (DX.CheckHitKey(DX.KEY_INPUT_Q) == DX.TRUE)
            {
                int x, y;
                DX.GetMousePoint(out x, out y);
                _stageFactory.RemoveEnemy(
                    _stageNo, new Point((short)x, (short)y));
                _world = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            }
            if (input.ShotToggled && input.Shot)
            {
                new File().SaveStages(_stageFactory.ToData());
            }
        }

        public void Render()
        {
            if (_view != null)
                _view.Render(_world);
            if (_stageNo < 0 || _stageFactory.LastStage < _stageNo)
                return;
            Renderer.DrawText(
                "総ライフ: " + _stageFactory.GetEnemies(_stageNo).Sum(x => x.Life),
                new Point(0, 0), new Color(255, 255, 255));
            Renderer.DrawText(
                "総スコア: " + _stageFactory.GetEnemies(_stageNo).Sum(x => (int)x.Type),
                new Point(0, 100), new Color(255, 255, 255));
        }

        private Point MergePoint(int x, int y)
        {
            short mx = (short)(((short)((x + 12) / 25)) * 25);
            short my = (short)(((short)((y + 12) / 25)) * 25);
            return new Point(mx, my);
        }
    }
}
