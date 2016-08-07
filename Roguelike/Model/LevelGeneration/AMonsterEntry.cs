using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.LevelGeneration
{
    public abstract class MonsterEntry 
    {
        public float MONSTER_BERTH;
        public float PLAYER_BERTH;
        public float SPAWN_RATE;

        public int NumberToSpawn = 1;

        protected int[] positions = new int[] { -20, 0, 20 };

        public MonsterEntry(float monster_berth, float player_berth, float spawn_rate)
        {
            MONSTER_BERTH = monster_berth;
            PLAYER_BERTH = player_berth;
            SPAWN_RATE = spawn_rate;
        }

        public MonsterEntry(float monster_berth, float player_berth, float spawn_rate, int numberToSpawn)
        {
            MONSTER_BERTH = monster_berth;
            PLAYER_BERTH = player_berth;
            SPAWN_RATE = spawn_rate;
            NumberToSpawn = numberToSpawn;
        }

        public abstract List<AMonster> GenerateMonster(Level currentLevel, int x, int y);
    }
}
