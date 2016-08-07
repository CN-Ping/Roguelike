using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;
using Microsoft.Xna.Framework;

namespace Roguelike.Model.GameObjects.Monsters.AI
{
    public abstract class AMonsterAI
    {
        protected AMonster myMonster;

        public AMonsterAI(AMonster monster)
        {
            myMonster = monster;
        }

        public abstract Vector2 computeMovementVector(double timeMillis);

        public virtual void MonsterDied(AMonster aMonster)
        {
        }
    }
}
