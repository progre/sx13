using System;
using System.Collections.Generic;
using System.Linq;
using Progressive.Scarlex13.Domains.Entities;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Games
{
    internal class ShootingWorldView
    {
        private readonly BackgroundView _backgroundView = new BackgroundView(true);

        private readonly Dictionary<Character, int> _dieFrame
            = new Dictionary<Character, int>();

        private readonly HashSet<Enemy> _damagedEnemies = new HashSet<Enemy>(); 

        private readonly ShootingWorld _world;
        private Renderer _renderer;
        private int _warpMove;

        private SoundManager _soundManager;

        public ShootingWorldView(ShootingWorld world)
        {
            _world = world;
            _dieFrame.Add(_world.Player, 0);
            foreach (Enemy enemy in world.Enemies)
            {
                _dieFrame.Add(enemy, 0);
                enemy.Damaged += (sender, args)
                    => _damagedEnemies.Add((Enemy)sender);
            }

            world.Cleared += (sender, args) =>
            {
                _backgroundView.Warp();
            };
        }

        public Renderer Renderer
        {
            private get { return _renderer; }
            set
            {
                _renderer = value;
                _backgroundView.Renderer = value;
            }
        }

        public SoundManager SoundManager
        {
            set
            {
                _soundManager = value;
                _world.Player.Shot += (sender, args) => _soundManager.Play("shot.ogg");
                _world.Player.Died += (sender, args) => _soundManager.Play("miss.ogg");
                EventHandler onEnemyDied = (sender, args) => _soundManager.Play("explosion.ogg");
                EventHandler onEnemyDamaged = (sender, args) => _soundManager.Play("hit.ogg");
                foreach (Enemy enemy in _world.Enemies)
                {
                    enemy.Damaged += onEnemyDamaged;
                    enemy.Died += onEnemyDied;
                }
            }
        }

        public bool EqualsWorld(ShootingWorld world)
        {
            return _world == world;
        }

        public void Render(ShootingWorld world)
        {
            _backgroundView.Render();
            RenderCharactors(world);
        }

        public void RenderWarp(ShootingWorld world)
        {
            _backgroundView.Render();
            _warpMove -= 20;
            Renderer.DrawClip("remilia.png", new Point(33, 98), new Size(33, 34),
                GetUnitPoint(world.Player.Point.Shift(0, _warpMove)));
        }

        private void RenderCharactors(ShootingWorld world)
        {
            RenderShots(world.Shots);
            foreach (Enemy enemy in world.Enemies)
            {
                if (enemy.Life > 0)
                {
                    RenderEnemy(enemy);
                }
                else
                {
                    int frame = _dieFrame[enemy];
                    _dieFrame[enemy] = ++frame;
                    RenderExplosion(enemy.Point, frame,
                        world.Enemies.Count(x => x.Life > 0) < 10);
                }
            }
            Player player = world.Player;
            if (player.Life > 0)
            {
                Renderer.DrawClip("remilia.png", new Point(33, 98), new Size(33, 34),
                    GetUnitPoint(player.Point));
            }
            else
            {
                int frame = _dieFrame[player];
                _dieFrame[player] = ++frame;
                RenderExplosion(player.Point, frame, false);
            }
        }

        private void RenderShots(IEnumerable<Shot> shots)
        {
            foreach (Shot shot in shots)
            {
                Point point = new Point(
                            (short)(shot.Point.X - 1),shot.Point.Y);
                var shotFile = shot.Homing ? "shot2.png" : "shot.png";
                switch (shot.Direction.Value)
                {
                    case 8:
                        Renderer.Draw(shotFile, point);
                        break;
                    case 9:
                        Renderer.DrawRotate(shotFile, point, Math.PI / 4);
                        break;
                    case 6:
                        Renderer.DrawRotate(shotFile, point, Math.PI / 2);
                        break;
                    case 3:
                        Renderer.DrawRotate(shotFile, point, Math.PI * 3 / 4);
                        break;
                    case 2:
                        Renderer.DrawRotate(shotFile, point, Math.PI);
                        break;
                    case 1:
                        Renderer.DrawRotate(shotFile, point, Math.PI * 5 / 4);
                        break;
                    case 4:
                        Renderer.DrawRotate(shotFile, point, Math.PI * 3 / 2);
                        break;
                    case 7:
                        Renderer.DrawRotate(shotFile, point, Math.PI * 7 / 4);
                        break;
                    case 5:
                        Renderer.DrawRotate(shotFile, point, new Random().NextDouble() * Math.PI * 2);
                        break;
                    default:
                        break;
                }

            }
        }

        private void RenderEnemy(Enemy enemy)
        {
            Tuple<string, int, int> resource = ToResource(enemy.Type);
            Point src;
            switch (enemy.Direction.Value)
            {
                case 5:
                case 8:
                    src = new Point((short)resource.Item2, (short)(resource.Item3 * 3));
                    break;
                case 7:
                case 4:
                case 1:
                    src = new Point((short)resource.Item2, (short)resource.Item3);
                    break;
                case 9:
                case 6:
                case 3:
                    src = new Point((short)resource.Item2, (short)(resource.Item3 * 2));
                    break;
                default:
                    src = new Point((short)resource.Item2, 0);
                    break;
            }
            if (_damagedEnemies.Contains(enemy))
            {
                Renderer.DrawClipWhite(
                    resource.Item1,
                    src,
                    new Size((short)resource.Item2, (short)resource.Item3),
                    GetUnitPoint(enemy.Point, resource.Item2, resource.Item3));
                _damagedEnemies.Remove(enemy);
            }
            else
            {
                Renderer.DrawClip(
                    resource.Item1,
                    src,
                    new Size((short)resource.Item2, (short)resource.Item3),
                    GetUnitPoint(enemy.Point, resource.Item2, resource.Item3));
            }
        }

        private static Tuple<string, int, int> ToResource(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Green:
                    return Tuple.Create("tewi.png", 32, 32);
                case EnemyType.Blue:
                    return Tuple.Create("nazoorin.png", 32, 34);
                case EnemyType.Red:
                    return Tuple.Create("aya.png", 32, 35);
                case EnemyType.Silver:
                    return Tuple.Create("reimu.png", 32, 32);
                case EnemyType.Gold:
                    return Tuple.Create("marisa.png", 32, 32);
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void RenderExplosion(Point point, int frame, bool extend)
        {
            const int frameLen = 5;
            int clipNo =
                frame < frameLen ? 0 :
                    frame < frameLen * 2 ? 1 :
                        frame < frameLen * 3 ? 2 :
                            frame < frameLen * 4 ? 1 :
                                frame < frameLen * 5 ? 0 :
                                    frame == frameLen * 5 ? -1 :
                                        -2;
            if (extend && clipNo == -1)
                _backgroundView.Extend();

            if (clipNo < 0)
                return;
            Renderer.DrawClip("explosion.png",
                new Point((short)(33 * clipNo), 0), new Size(33, 33),
                point.Shift(-17, -17));
        }

        private static Point GetUnitPoint(Point point)
        {
            return point.Shift(-17, -17);
        }

        private static Point GetUnitPoint(Point point, int width, int height)
        {
            return point.Shift(-(width >> 1), -(height >> 1));
        }
    }
}