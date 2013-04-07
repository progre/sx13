using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using System.Linq;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class StageFactory
    {
        private Tuple<EnemyType, Point>[][] _stages;

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

            var stages = new List<Tuple<EnemyType, Point>[]>();
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
                    stages.Add(enemies.ToArray());
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
            return new StageFactory(stages.ToArray());
        }

        private StageFactory(Tuple<EnemyType, Point>[][] stages)
        {
            _stages = stages;
        }

        public IReadOnlyList<Enemy> GetEnemies(int stageNo)
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
                return _stages.Length - 1;
            }
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
    }
}