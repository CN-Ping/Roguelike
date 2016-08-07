using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model
{
    /*
     * Here is a class to keep skill stats in. I don't know how we are going to use it.
     * But I didn't want to just put them at the top of Model when they aren't being used yet.
     */
    [Serializable]
    public class SkillTreeStats
    {

        public float shotSpeed;
        public float rateOfFire;
        public float maxHealth;
        public float shotSize;
        public float speed;
        public float damage;
        public float lightRange;
        public float lightIntensity;
        public float luck;

        public float pickupPotency;
        public float detectedRange;
        public float critChance;
        public float critDamage;
        public float evasion;
        public float defense;
        public float torchRadius;
        public float torchDuration;

        public int torchCount;

        public float experience = 0;
        public int skillPointsLeft = 1;
        public List<int> skillAssignment;
        public bool[] allowedSkills;

        private int numSkills = 17;

        public int playerLevel = 1;

        public float expRequired = 10;

        // never call this directly
        private SkillTreeStats()
        {
            shotSpeed = 8;
            rateOfFire = 625;
            maxHealth = 90;
            shotSize = 1;
            speed = 4;
            damage = 3;
            lightRange = 200;
            lightIntensity = 0;
            luck = 1;
            pickupPotency = 1;
            detectedRange = 0;
            critChance = 0.01f;
            critDamage = 1.5f;
            evasion = 0.01f;
            defense = 0;
            torchRadius = 200f;
            torchDuration = 10f;
            torchCount = 10;

            skillAssignment = new List<int>();
            skillAssignment.Add(-1);
            for (int i = 1; i <= numSkills; i++)
            {
                skillAssignment.Add(0);
            }

            allowedSkills = new bool[numSkills + 1];
            allowedSkills[0] = false;
        }

        public static SkillTreeStats MakeDefaultSkillTree()
        {
            return new SkillTreeStats();
        }

        public static SkillTreeStats LoadInstanceFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                Stream inStream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

                SkillTreeStats obj = (SkillTreeStats)formatter.Deserialize(inStream);

                inStream.Close();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public void SaveStatsToFile(string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter() ;

            Stream outStream = new FileStream(
            filename,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);
            
            formatter.Serialize(outStream, this);
            outStream.Close();
        }

        public StatsInstance makeInstance(Level currentLevel)
        {
            StatsInstance s = new StatsInstance(currentLevel);

            s.shotSpeed = shotSpeed;
            s.rateOfFire = rateOfFire;
            s.health = maxHealth;
            s.maxHealth = maxHealth;
            s.shotSize = shotSize;
            s.speed = speed;
            s.damage = damage;
            s.LightRange = lightRange;
            s.lightIntensity = lightIntensity;
            s.luck = luck;
            s.pickupPotency = pickupPotency;
            s.detectedRange = detectedRange;
            s.critChance = critChance;
            s.critDamage = critDamage;
            s.evasion = evasion;
            s.torchRadius = torchRadius;
            s.torchDuration = torchDuration;
            s.defense = defense;
            s.playerLevel = playerLevel;
            s.tempExp = experience;
            s.torchCount = torchCount;
            return s;
        }
    }
}
