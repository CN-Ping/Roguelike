using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.LevelGeneration
{
    public class MonsterEntry<T> : MonsterEntry where T : AMonster
    {
        public MonsterEntry(float monster_berth, float player_berth, float spawn_rate) : 
            base(monster_berth, player_berth, spawn_rate)
        {

        }

        public MonsterEntry(float monster_berth, float player_berth, float spawn_rate, int numberToSpawn)
            : base(monster_berth, player_berth, spawn_rate, numberToSpawn)
        {

        }

        public override List<AMonster> GenerateMonster(Level currentLevel, int x, int y)
        {
            List<AMonster> l = new List<AMonster>();

            // this is just for spiders currently
            if (NumberToSpawn == 9)
            {
                foreach (int pos_x in positions)
                {
                    foreach (int pos_y in positions)
                    {
                        l.Add(new SpiderMonster(currentLevel, x + pos_x, y + pos_y));
                    }
                }
            }

            else
            {
                l.Add((T)Activator.CreateInstance(typeof(T), new object[] { currentLevel, x, y }));
            }

            return l;
        }
    }
}
