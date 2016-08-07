using Roguelike.Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Model.GameObjects.Pickups
{
    public class PickupGenerator
    {
        private Random rng = new Random();
        private int maxPickupId = 3;

        List<float> percentages;
        
        public PickupGenerator()
        {
            percentages = new List<float>();

            /* These need to add up to 1. The first Add corresponds to ID 1. */
            percentages.Add(.25f);
            percentages.Add(.45f);
            percentages.Add(.30f);
        }

        public APickup getPickupById(int id, Level level, int x, int y)
        {
            // yes, I know this is hideous. 
            switch (id)
            {
                case 1:
                    return new HealthPickup(level, x, y);
                case 2:
                    return new ExpPickup(level, x, y, 1);
                case 3:
                    return new TorchPickup(level, x, y);
                default:
                    return new HealthPickup(level, x, y);
            }
        }

        public APickup getRandomPickup(Level level, int x, int y)
        {
            //return getPickupById(rng.Next(maxPickupId) + 1, level, x, y);
            int id = pickIdByPercentage();
            if (id == -1)
            {
                throw new Exception("pickIdByPercentage returned an invalid value.");
            }
            return getPickupById(id, level, x, y);
        }

        private int pickIdByPercentage()
        {
            double selection = rng.NextDouble();
            float percent = 0;

            for (int i = 0; i < maxPickupId; i++ )
            {
                percent += percentages[i];
                if (selection <= percent)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
