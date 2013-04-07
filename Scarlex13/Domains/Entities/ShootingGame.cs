using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingGame
    {
        private const int ClearedTime = 50;

        private readonly IReadOnlyList<IReadOnlyList<Enemy>> _stages
            = new EnemiesFactory().FromData(new File().GetStages());

        private int _clearedFrame = -1;
        private int _stageNo = -1;
        private TimeSpan _time;

        public ShootingGame()
        {
            CreateWorld();
        }

        public bool Cleared { get; private set; }
        public ShootingWorld World { get; private set; }
        public SoundManager SoundManager { private get; set; }
        public int Score { get; private set; }

        public void Update(Input input)
        {
            if ( /* ポーズ状態 */ false)
            {
                return;
            }
            if (!Cleared)
            {
                World.Update(input);
                return;
            }
            _clearedFrame++;
            if (_clearedFrame > ClearedTime)
            {
                CreateWorld();
            }
        }

        private void CreateWorld()
        {
            Cleared = false;
            _clearedFrame = -1;
            World = new ShootingWorld(_stages[++_stageNo]);
            World.Cleared += (sender, args) => Cleared = true;
        }
    }
}