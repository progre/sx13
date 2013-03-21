using Progressive.Scarlex13.Domains.ValueObjects;
using System;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingGame
    {
        private readonly ShootingWorld _world = new ShootingWorld();
        private TimeSpan _time;

        public ShootingWorld World
        {
            get { return _world; }
        }

        public int Score { get; private set; }

        public void Update(Input input)
        {
            if ( /* ポーズ状態 */ false)
            {
                return;
            }
            _world.Update(input);
        }
    }
}