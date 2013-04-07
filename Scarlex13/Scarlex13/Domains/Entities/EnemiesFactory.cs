using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class EnemiesFactory
    {
        public IReadOnlyList<IReadOnlyList<Enemy>> FromData(String data)
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

            var stage = new List<Enemy[]>();
            var enemies = new List<Enemy>();
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
                    stage.Add(enemies.ToArray());
                    enemies.Clear();
                    continue;
                }
                if (enemy.Count < 3)
                {
                    enemy.Clear();
                    continue;
                }
                enemies.Add(new Enemy(
                    ToEnemyType(enemy[0]),
                    new Point((short)enemy[1], (short)enemy[2]),
                    new Random(rnd.Next())));
                enemy.Clear();
            }
            return stage.ToArray();
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