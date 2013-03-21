using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingWorld
    {
        private readonly IEnumerable<Enemy> _enemies;
        private readonly Player _player = new Player();
        private readonly IEnumerable<object> _shots;

        public Player Player
        {
            get { return _player; }
        }

        private IEnumerable<Enemy> Enemies
        {
            get { return _enemies; }
        }

        private IEnumerable<object> Shots
        {
            get { return _shots; }
        }

        public void Update(Input input)
        {
            Player.Update(input);
        }
    }
}