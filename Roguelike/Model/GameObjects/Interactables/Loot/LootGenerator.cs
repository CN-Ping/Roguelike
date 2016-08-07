using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Pickups;

namespace Roguelike.Model.GameObjects.Loot
{
    public class LootGenerator
    {
        private int maxLootId = 14;
        private Random rng = new Random();

        List<int> rudimentaryItemPool = new List<int>();

        public LootGenerator()
        {
            rudimentaryItemPool.Add(1);
            rudimentaryItemPool.Add(2);
            rudimentaryItemPool.Add(3);
            rudimentaryItemPool.Add(5);
            rudimentaryItemPool.Add(6);
            rudimentaryItemPool.Add(7);

            // dark helmet and darth vader's helmet are mutually exclusive
            // should probably add jayne's hat in here too
            if (rng.Next(2) == 0)
            {
                rudimentaryItemPool.Add(8);
            }
            else
            {
                rudimentaryItemPool.Add(10);
            }

            rudimentaryItemPool.Add(9);
            rudimentaryItemPool.Add(11);
            rudimentaryItemPool.Add(12);
            rudimentaryItemPool.Add(13);
            rudimentaryItemPool.Add(14);
        }

        public ALoot getLootById(int id, Level level, int x, int y)
        {
            // yes, I know this is hideous. 
            switch (id)
            {
                case 0:
                    return new FillerLoot(level, x, y);
                case 1:
                    return new JaynesHatLoot(level, x, y);
                case 2:
                    return new HeartContainerLoot(level, x, y);
                case 3:
                    return new CloverLoot(level, x, y);
                case 5:
                    return new DCellLoot(level, x, y);
                case 6:
                    return new HM05Loot(level, x, y);
                case 7:
                    return new RabbitFootLoot(level, x, y);
                case 8:
                    return new DarkHelmetLoot(level, x, y);
                case 9:
                    return new CarbosLoot(level, x, y);
                case 10:
                    return new DarthVaderHelmetLoot(level, x, y);
                case 11:
                    return new GoogleFiberLoot(level, x, y);
                case 12:
                    return new DualWield(level, x, y);
                case 13:
                    return new TargetingComputerLoot(level, x, y);
                case 14:
                    return new XLoot(level, x, y);
                default:
                    return new FillerLoot(level, x, y);
            }
        }

        public ALoot getLootByName(String name, Level level, int x, int y)
        {
            // yes, I know this is hideous. 
            switch (name)
            {
                case "Filler":
                    return new FillerLoot(level, x, y);
                case "JaynesHat":
                    return new JaynesHatLoot(level, x, y);
                case "HeartContainer":
                    return new HeartContainerLoot(level, x, y);
                case "Clover":
                    return new CloverLoot(level, x, y);
                case "D-Cell":
                    return new DCellLoot(level, x, y);
                default:
                    return new FillerLoot(level, x, y);
            }
        } 

        public ALoot getRandomLoot(Level level, int x, int y)
        {
            return getLootById(rng.Next(maxLootId) + 1, level, x, y);
        }

        public ALoot getRandomLootFromPool(Level level, int x, int y)
        {
            int idx = rng.Next(rudimentaryItemPool.Count);

            ALoot l = getLootById(rudimentaryItemPool[idx], level, x, y);

            rudimentaryItemPool.RemoveAt(idx);
            return l;
        }
    }
}
