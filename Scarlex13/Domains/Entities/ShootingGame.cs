using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingGame
    {
        private const int ClearedTime = 50;
        private const int FailedTime = 50;

        private readonly StageFactory _stageFactory
            = StageFactory.FromData(new File().GetStages());

        private int _intervalFrame = -1;
        private int _stageNo = 0;
        private TimeSpan _time;
        private bool _newWorld = true;

        public ShootingGame()
        {
        }

        public bool Cleared { get; private set; }
        public bool Failed { get; private set; }
        public ShootingWorld World { get; private set; }
        public SoundManager SoundManager { private get; set; }
        public int Score { get; private set; }

        public void Update(Input input)
        {
            if (_newWorld)
            {
                CreateWorld();
                _newWorld = false;
            }

            if ( /* ポーズ状態 */ false)
            {
                return;
            }
            if (!Cleared)
            {
                World.Update(input);
                if (!Failed)
                    return;
            }
            _intervalFrame++;
            if (Cleared && _intervalFrame > ClearedTime)
            {
                if (_stageNo < _stageFactory.LastStage)
                    _stageNo++;
                _newWorld = true;
                return;
            }
            if (Failed && _intervalFrame > FailedTime)
            {
                _newWorld = true;
                return;
            }
        }

        private void CreateWorld()
        {
            Cleared = false;
            Failed = false;
            _intervalFrame = -1;
            World = new ShootingWorld(_stageFactory.GetEnemies(_stageNo));
            World.Cleared += (sender, args) => Cleared = true;
            World.Failed += (sender, args) => Failed = true;
        }
    }
}