using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using System.Linq;
using System.Text;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class StageFactory
    {
        private readonly List<List<Tuple<EnemyType, Point>>> _stages;

        public static StageFactory FromData(String data)
        {
            var list = new List<object>();
            var queue = new Queue<char>();
            foreach (char c in data)
            {
                if (IsNumber(c))
                {
                    queue.Enqueue(c);
                    continue;
                }
                if (queue.Count > 0)
                {
                    list.Add(int.Parse(new string(queue.ToArray())));
                    queue.Clear();
                }
                list.Add(c);
            }

            var rnd = new Random();

            var stages = new List<List<Tuple<EnemyType, Point>>>();
            var enemies = new List<Tuple<EnemyType, Point>>();
            var enemy = new List<int>(3);
            foreach (object o in list)
            {
                if (o is int)
                {
                    enemy.Add((int)o);
                    continue;
                }
                if (!(o is char))
                {
                    enemy.Clear();
                    continue;
                }
                var c = (char)o;
                if (c != '\n')
                    continue;
                if (enemy.Count == 0 && enemies.Count > 0)
                {
                    stages.Add(enemies.ToList());
                    enemies.Clear();
                    continue;
                }
                if (enemy.Count < 3)
                {
                    enemy.Clear();
                    continue;
                }
                enemies.Add(Tuple.Create(
                    ToEnemyType(enemy[0]),
                    new Point((short)enemy[1], (short)enemy[2])));
                enemy.Clear();
            }
            return new StageFactory(stages);
        }

        private StageFactory(List<List<Tuple<EnemyType, Point>>> stages)
        {
            _stages = stages;
        }

        public IEnumerable<Enemy> GetEnemies(int stageNo)
        {
            var rnd = new Random();
            return _stages[stageNo]
                .Select(x => new Enemy(
                    x.Item1,
                    x.Item2,
                    new Random(rnd.Next())))
                .ToArray();
        }

        public int LastStage
        {
            get
            {
                return _stages.Count - 1;
            }
        }

        public string ToData()
        {
            var sb = new StringBuilder();
            foreach (var stage in _stages)
            {
                foreach (var enemy in stage)
                {
                    sb.Append(ToInt(enemy.Item1)).Append(',');
                    sb.Append(enemy.Item2.X).Append(',');
                    sb.Append(enemy.Item2.Y).Append("\r\n");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        private static bool IsNumber(char c)
        {
            return '0' <= c && c <= '9';
        }

        private static EnemyType ToEnemyType(int i)
        {
            switch (i)
            {
                case 1:
                    return EnemyType.Green;
                case 2:
                    return EnemyType.Blue;
                case 3:
                    return EnemyType.Red;
                case 4:
                    return EnemyType.Silver;
                case 5:
                    return EnemyType.Gold;
                default:
                    return 0;
            }
        }

        private static int ToInt(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Green:
                    return 1;
                case EnemyType.Blue:
                    return 2;
                case EnemyType.Red:
                    return 3;
                case EnemyType.Silver:
                    return 4;
                case EnemyType.Gold:
                    return 5;
                default:
                    return 0;
            }
        }

        public void AddEnemy(int stageNo, EnemyType enemyType, Point point)
        {
            if (_stages[stageNo].Any(x => Distance(x.Item2, point) < 3))
                return;
            _stages[stageNo].Add(Tuple.Create(enemyType, point));
        }

        public void RemoveEnemy(int stageNo, Point point)
        {
            _stages[stageNo].RemoveAll(
                x => Distance(x.Item2, point) < 15);
        }

        public double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public void AddStage()
        {
            _stages.Add(new List<Tuple<EnemyType, Point>>());
        }
    }
}