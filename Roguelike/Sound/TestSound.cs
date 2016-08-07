using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Sound
{
    public class TestSound
    {
        Roguelike Game;
        SoundEffect soundTest;
        SoundEffectInstance soundTestInstance;

        public TestSound(Roguelike game)
        {
            Game = game;
        }

        public void LoadContent()
        {
            soundTest = Game.Content.Load<SoundEffect>("Sound/TestSound");
            soundTestInstance = soundTest.CreateInstance();
        }

        public void PlaySound()
        {
            soundTestInstance.Play();
        }

        public void StopSound()
        {
            soundTestInstance.Stop();
        }
    }
}
