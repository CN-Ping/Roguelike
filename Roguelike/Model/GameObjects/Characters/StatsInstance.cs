using Roguelike.Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Roguelike.Model
{
    /*
     * Here is a class to keep skill stats in. I don't know how we are going to use it.
     * But I didn't want to just put them at the top of Model when they aren't being used yet.
     */
    public class StatsInstance
    {
        Level currentLevel;
        bool doop = true;

        private float speed_ = 0.0f;
        public float shotSpeed = 0.0f;
        public float rateOfFire = 0.0f;

        public float maxHealth = 0.0f;
        public float pickupPotency = 0.0f;

        public float damage = 0.0f;
        public float shotSize = 0.0f;

        private float lightRange_;
        private float tempExp_;

        public int playerLevel;

        public float speed
        {
            get
            {
                return speed_;
            }
            set
            {
                speed_ = value;

                if (velocity.X != 0)
                {
                    float magnitude_x = velocity.X / Math.Abs(velocity.X);

                    velocity.X = magnitude_x * value;
                }

                if (velocity.Y != 0)
                {
                    float magnitude_y = velocity.Y / Math.Abs(velocity.Y);

                    velocity.Y = magnitude_y * value;
                }
                
                


            }
        }

        public float LightRange { 
            get{
                return lightRange_;
            } 
            set {
                lightRange_ = value;
                

                if (doop)
                {
                    doop = false;
                }
                else
                {
                    currentLevel.gameModel.gameView.minimap.sightRadius = (int)Math.Ceiling(lightRange_ / 100) + 1;
                    //currentLevel.mainChar.LefreshLight();
                }
                    
            } }

        public float tempExp
        {
            get
            {
                return tempExp_;
            }

            set
            {
                tempExp_ = value;
                
                float expReq = currentLevel.gameModel.skillTree.expRequired;
                if (tempExp_ >= expReq)
                {
                    tempExp_ = tempExp_ - expReq;
                    currentLevel.mainChar.stats.playerLevel += 1;
                    currentLevel.gameModel.skillTree.playerLevel += 1;
                    currentLevel.gameModel.skillTree.skillPointsLeft += 1;
                }

                currentLevel.gameModel.skillTree.experience = tempExp_;
            }
        }

        public float lightIntensity = 0.0f;
        public float detectedRange = 0.0f;

        public float luck = 0.0f;
        public float critChance = 0.0f;
        public float critDamage = 0.0f;
        public float evasion = 0.0f;

        public float torchRadius = 0.0f;
        public float torchDuration = 0.0f;

        public float defense = 0.0f;

        public float health = 0.0f;

        public float height = 90.0f;
        public float width = 50.0f;

        public Vector2 velocity = new Vector2();

        /* 0 = right, 1 = down, 2 = left, 3 = up*/
        public int lastShotDirection = 1;
        public int lastMoveDirection = 1;

        public bool noClip = false;

        public float distanceTraveled = 0;

        public int torchCount;

        public StatsInstance(Level levelIn)
        {
            currentLevel = levelIn;
        }

        public override String ToString()
        {
            string output = "";

            output += "health: " + health + " / " + maxHealth +"\n";
            output += "shotSpeed: " + shotSpeed + "\n";
            output += "rateOfFire: " + rateOfFire + "\n";
            output += "shotSize: " + shotSize + "\n";
            output += "speed: " + speed + "\n";
            output += "damage: " + damage + "\n";
            output += "lightRange: " + LightRange + "\n";
            output += "lightConeWidth: " + lightIntensity + "\n";
            output += "luck: " + luck + "\n";
            output += "height: " + height + "\n";
            output += "width: " + width + "\n";

            return output;
        }

        public int getDirection()
        {
            /*
            if (xVel > 0)
            {
                // Right
                lastDirection = 0;
                return 0;
            }
            if (xVel < 0)
            {
                // Left
                lastDirection = 2;
                return 2;
            }
            if (yVel > 0)
            {
                // Down
                lastDirection = 1;
                return 1;
            }
            if (yVel < 0)
            {
                // Up
                lastDirection = 3;
                return 3;
            }

            else
            {
                return lastDirection;
            }
             * */
            return lastShotDirection;
        }

        public void addExperience(int expToAdd)
        {
            currentLevel.gameModel.skillTree.experience += expToAdd;
        }

        internal void UpdateDistance(float p)
        {
            distanceTraveled += p;
        }
    }
}
