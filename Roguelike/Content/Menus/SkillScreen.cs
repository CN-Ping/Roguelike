using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Model;

namespace Roguelike.Menus
{
    public class SkillScreen
    {
        Model.Model gameModel;

        //Texture2D splashTexture;
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        Texture2D orionTextureStars;
        Texture2D orionTextureLines;
        Texture2D background;
        SpriteFont Font;
        SpriteFont Font_14;

        int numberOfStars = 17;
        int numberOfLines = 19;
        List<Texture2D> starTextures;
        List<Texture2D> redSelectionStarsTextures;
        List<Texture2D> greenSelectionStarsTextures;
        List<Texture2D> lineTextures;
        Rectangle source;
        Vector2 location;
        Vector2 middleOfTexture;
        Vector2 skillTextPosition;
        Vector2 skillPointsAssignedPos;
        Vector2 skillPointsLeftPos;
        Vector2 pressEnterPos;
        Vector2 pressSpacePos;
        Vector2 pressEscPos;

        int startStar = 4;
        int star;

        bool enterPress = false;
        bool escPress = false;
        bool spacePress = false;

        bool downPress = false;
        bool upPress = false;
        bool leftPress = false;
        bool rightPress = false;

        bool commaPress = false;
        bool aPress = false;
        bool oPress = false;
        bool ePress = false;

        public SkillTreeStats tree;

        public SkillScreen(Model.Model model, GraphicsDeviceManager inGraphics, SkillTreeStats inTree)
        {
            gameModel = model;
            graphics = inGraphics;
            LoadContent();
            tree = inTree;
            star = startStar;
            tree.allowedSkills[star] = true;
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameModel.Game.GraphicsDevice);
            orionTextureStars = gameModel.Game.Content.Load<Texture2D>("Textures/Constellation/stars/stars");
            orionTextureLines = gameModel.Game.Content.Load<Texture2D>("Textures/Constellation/lines/lines");
            background = gameModel.Game.Content.Load<Texture2D>("Textures/starField");
            
            starTextures = new List<Texture2D>();
            starTextures.Add(null);
            redSelectionStarsTextures = new List<Texture2D>();
            redSelectionStarsTextures.Add(null);
            greenSelectionStarsTextures = new List<Texture2D>();
            greenSelectionStarsTextures.Add(null);
            for (int i = 1; i <= numberOfStars; i++)
            {
                string texture = "Textures/Constellation/stars/star" + i;
                starTextures.Add(gameModel.Game.Content.Load<Texture2D>(texture));
                string redSelectionTexture = "Textures/Constellation/stars/redSelectionStars/star" + i;
                redSelectionStarsTextures.Add(gameModel.Game.Content.Load<Texture2D>(redSelectionTexture));
                string greenSelectionTexture = "Textures/Constellation/stars/greenSelectionStars/star" + i;
                greenSelectionStarsTextures.Add(gameModel.Game.Content.Load<Texture2D>(greenSelectionTexture));
            }
            lineTextures = new List<Texture2D>();
            lineTextures.Add(null);
            for (int i = 1; i <= numberOfLines; i++)
            {
                string texture = "Textures/Constellation/lines/line" + i;
                lineTextures.Add(gameModel.Game.Content.Load<Texture2D>(texture));
            }

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            location = new Vector2(screenWidth / 2, screenHeight / 2);
            source = new Rectangle(0,0, orionTextureStars.Width, orionTextureStars.Height);
            middleOfTexture = new Vector2(orionTextureStars.Width/2, orionTextureStars.Height/2);

            Font = gameModel.Game.Content.Load<SpriteFont>("Arial_s24");
            Font_14 = gameModel.Game.Content.Load<SpriteFont>("Arial");
            skillPointsLeftPos = new Vector2(screenWidth / 4, 3 * screenHeight / 5);
            pressEnterPos = new Vector2(screenWidth /4, 3 * screenHeight / 5 + 60);
            skillTextPosition = new Vector2(screenWidth / 4, 3 * screenHeight / 4);
            skillPointsAssignedPos = new Vector2(screenWidth / 4, 3 * screenHeight / 4 + 34);
            pressSpacePos = new Vector2(screenWidth/4, 3 * screenHeight / 4 + 74);
            pressEscPos = new Vector2(20, 20);

        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            bool up = false;
            bool down = false;
            bool right = false;
            bool left = false;
            bool enter = false;
            bool space = false;

            #region key events

            /* Key down events */
            if (!enterPress && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                enterPress = true;
            }
            if (!escPress && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                escPress = true;
            }
            if (!spacePress && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                spacePress = true;
            }
            // Arrows
            if (!downPress && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                downPress = true;
            }
            else if (!upPress && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                upPress = true;
            }
            else if (!leftPress && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                leftPress = true;
            }
            else if (!rightPress && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rightPress = true;
            }
            if (gameModel.dvorak) //,aoe
            {
                if (!oPress && Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    oPress = true;
                }
                else if (!commaPress && Keyboard.GetState().IsKeyDown(Keys.OemComma))
                {
                    commaPress = true;
                }
                else if (!aPress && Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    aPress = true;
                }
                else if (!ePress && Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    ePress = true;
                }
            }
            else //wasd
            {
                if (!oPress && Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    oPress = true;
                }
                else if (!commaPress && Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    commaPress = true;
                }
                else if (!aPress && Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    aPress = true;
                }
                else if (!ePress && Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    ePress = true;
                }
            }

            /* Key up events */
            if (enterPress && Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                enterPress = false;
                enter = true;
            }
            if (escPress && Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                gameModel.startMenu();
            }
            if (spacePress && Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                spacePress = false;
                space = true;
            }
            // Arrows
            if (downPress && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                downPress = false;
                down = true;
            }
            else if (upPress && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                upPress = false;
                up = true;
            }
            else if (leftPress && Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                leftPress = false;
                left = true;
            }
            else if (rightPress && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                rightPress = false;
                right = true;
            }
            if (gameModel.dvorak) //,aoe
            {
                if (oPress && Keyboard.GetState().IsKeyUp(Keys.O))
                {
                    oPress = false;
                    down = true;
                }
                else if (commaPress && Keyboard.GetState().IsKeyUp(Keys.OemComma))
                {
                    commaPress = false;
                    up = true;
                }
                else if (aPress && Keyboard.GetState().IsKeyUp(Keys.A))
                {
                    aPress = false;
                    left = true;
                }
                else if (ePress && Keyboard.GetState().IsKeyUp(Keys.E))
                {
                    ePress = false;
                    right = true;
                }
            }
            else //wasd
            {
                if (oPress && Keyboard.GetState().IsKeyUp(Keys.S))
                {
                    oPress = false;
                    down = true;
                }
                else if (commaPress && Keyboard.GetState().IsKeyUp(Keys.W))
                {
                    commaPress = false;
                    up = true;
                }
                else if (aPress && Keyboard.GetState().IsKeyUp(Keys.A))
                {
                    aPress = false;
                    left = true;
                }
                else if (ePress && Keyboard.GetState().IsKeyUp(Keys.D))
                {
                    ePress = false;
                    right = true;
                }
            }
            #endregion

            /* Switch star state */
            #region Switch Star State
            switch (star)
            {
                #region star 1
                case 1:
                    if (right)
                    {
                        star = 2;
                    }
                    if (down)
                    {
                        star = 3;
                    }
                    break;
                #endregion
                #region star 2
                case 2:
                    if (down)
                    {
                        star = 3;
                    }
                    if (left)
                    {
                        star = 1;
                    }
                    break;
                #endregion
                #region star 3
                case 3:
                    if (up)
                    {
                        star = 2;
                    }
                    if (down)
                    { 
                        star = 4;
                    }
                    if (left)
                    {
                        star = 1;
                    }
                    break;
                #endregion
                #region star 4
                case 4:
                    if (up)
                    {
                        star = 3;
                    }
                    if (down)
                    { 
                        star = 16;
                    }
                    if (right) 
                    {
                        star = 5;
                    }
                    break;
                #endregion
                #region star 5
                case 5:
                    if (left)
                    {
                        star = 4;
                    }
                    if (right) 
                    {
                        star = 6;
                    }
                    break;
                #endregion
                #region star 6
                case 6:
                    if (up)
                    {
                        star = 5;
                    }
                    if (down)
                    { 
                        star = 13;
                    }
                    if (left)
                    {
                        star = 5;
                    }
                    if (right) 
                    {
                        star = 7;
                    }
                    break;
                #endregion
                #region star 7
                case 7:
                    if (up)
                    {
                        star = 8;
                    }
                    if (down)
                    { 
                        star = 10;
                    }
                    if (left)
                    {
                        star = 6;
                    }
                    break;
                #endregion
                #region star 8
                case 8:
                    if (up)
                    {
                        star = 9;
                    }
                    if (down)
                    { 
                        star = 7;
                    }
                    break;
                #endregion
                #region star 9
                case 9:
                    if (down)
                    { 
                        star = 8;
                    }
                    break;
                #endregion
                #region star 10
                case 10:
                    if (up)
                    {
                        star = 7;
                    }
                    if (down)
                    { 
                        star = 11;
                    }
                    break;
                #endregion
                #region star 11
                case 11:
                    if (up)
                    {
                        star = 10;
                    }
                    if (down)
                    { 
                        star = 12;
                    }
                    break;
                #endregion
                #region star 12
                case 12:
                    if (up)
                    {
                        star = 11;
                    }
                    break;
                #endregion
                #region star 13
                case 13:
                    if (up)
                    {
                        star = 6;
                    }
                    if (down)
                    { 
                        star = 14;
                    }
                    if (left)
                    {
                        star = 17;
                    }
                    if (right)
                    {
                        star = 14;
                    }
                    break;
                #endregion
                #region star 14
                case 14:
                    if (up)
                    {
                        star = 13;
                    }
                    if (left)
                    {
                        star = 15;
                    }
                    break;
                #endregion
                #region star 15
                case 15:
                    if (up)
                    {
                        star = 16;
                    }
                    if (right) 
                    {
                        star = 14;
                    }
                    break;
                #endregion
                #region star 16
                case 16:
                    if (up)
                    {
                        star = 4;
                    }
                    if (down)
                    { 
                        star = 15;
                    }
                    if (right) 
                    {
                        star = 17;
                    }
                    break;
                #endregion
                #region star 17
                case 17:
                    if (right)
                    {
                        star = 13;
                    }
                    if (left)
                    {
                        star = 16;
                    }
                    break;
                #endregion
                default:
                    break;
            }
            #endregion

            /* Affect SkillTreeStats if skill point used. */
            if (enter && tree.skillPointsLeft > 0 && tree.allowedSkills[star])
            {
                tree.skillAssignment[star] += 1;
                tree.skillPointsLeft -= 1;

                switch(star)
                {
                    #region star 1
                    case 1: /* Rate of Fire */
                        tree.rateOfFire -= 50;
                        tree.allowedSkills[2] = true;
                        tree.allowedSkills[3] = true;
                        break;
                    #endregion
                    #region star 2
                    case 2: /* Shot Speed */
                        tree.shotSpeed += 1;
                        tree.allowedSkills[1] = true;
                        tree.allowedSkills[3] = true;
                        break;
                    #endregion
                    #region star 3
                    case 3: /* Speed */
                        tree.speed += 1;
                        tree.allowedSkills[1] = true;
                        tree.allowedSkills[2] = true;
                        tree.allowedSkills[4] = true;
                        break;
                    #endregion
                    #region star 4
                    case 4: /* Max Health */
                        tree.maxHealth += 10;
                        tree.allowedSkills[3] = true;
                        tree.allowedSkills[5] = true;
                        tree.allowedSkills[16] = true;
                        break;
                    #endregion
                    #region star 5
                    case 5: /* Pickup Potency */
                        tree.pickupPotency += .25f;
                        tree.allowedSkills[4] = true;
                        tree.allowedSkills[6] = true;
                        break;
                    #endregion
                    #region star 6
                    case 6: /* Luck */
                        tree.luck += .25f;
                        tree.allowedSkills[5] = true;
                        tree.allowedSkills[7] = true;
                        tree.allowedSkills[13] = true;
                        break;
                    #endregion
                    #region star 7
                    case 7: /* Evasion */
                        tree.evasion += 0.05f;
                        tree.allowedSkills[6] = true;
                        tree.allowedSkills[8] = true;
                        tree.allowedSkills[10] = true;
                        break;
                    #endregion
                    #region star 8
                    case 8: /* Critical Chance */
                        tree.critChance += 0.05f;
                        tree.allowedSkills[7] = true;
                        tree.allowedSkills[9] = true;
                        break;
                    #endregion
                    #region star 9
                    case 9: /* Critical Damage */
                        tree.critDamage += 0.25f;
                        tree.allowedSkills[8] = true;
                        break;
                    #endregion
                    #region star 10
                    case 10: /* Light Range */
                        tree.lightRange += 25;
                        tree.allowedSkills[7] = true;
                        tree.allowedSkills[11] = true;
                        break;
                    #endregion
                    #region star 11
                    case 11: /* Light Intensity */
                        //TODO: this
                        tree.allowedSkills[10] = true;
                        tree.allowedSkills[12] = true;
                        break;
                    #endregion
                    #region star 12
                    case 12: /* Detected Range */
                        //TODO: what is this
                        tree.allowedSkills[11] = true;
                        break;
                    #endregion
                    #region star 13
                    case 13: /* Shot Size */
                        tree.shotSize += .1f;
                        tree.allowedSkills[6] = true;
                        tree.allowedSkills[14] = true;
                        tree.allowedSkills[17] = true;
                        break;
                    #endregion
                    #region star 14
                    case 14: /* Torch Duration */
                        tree.torchDuration += 3f;
                        tree.allowedSkills[13] = true;
                        tree.allowedSkills[15] = true;
                        break;
                    #endregion
                    #region star 15
                    case 15: /* Torch Radius */
                        tree.torchRadius += 25f;
                        tree.allowedSkills[14] = true;
                        tree.allowedSkills[16] = true;
                        break;
                    #endregion
                    #region star 16
                    case 16: /* Defense */
                        tree.defense += 1;
                        tree.allowedSkills[15] = true;
                        tree.allowedSkills[17] = true;
                        tree.allowedSkills[4] = true;
                        break;
                    #endregion
                    #region star 17
                    case 17: /* Damage */
                        tree.damage += 1;
                        tree.allowedSkills[13] = true;
                        tree.allowedSkills[16] = true;
                        break;
                    #endregion
                    default:
                        break;
                }
            }

            /* If space was pressed, reset the skill tree */
            if (space == true)
            {
                int playerLevel = tree.playerLevel;
                float oldexp = tree.experience;
                tree = SkillTreeStats.MakeDefaultSkillTree();
                gameModel.skillTree = tree;
                tree.allowedSkills[startStar] = true;
                tree.skillPointsLeft = playerLevel;
                tree.playerLevel = playerLevel;
                tree.experience = oldexp;
            }

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(new Color(0,0,50));

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2(0,0), Color.White);

            spriteBatch.Draw(orionTextureLines, location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);

            /* Draw the glowing lines */
            #region Draw Lines


            #region star 1
            if (tree.skillAssignment[1] > 0)
            {
                spriteBatch.Draw(lineTextures[1], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[2], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 2
            if (tree.skillAssignment[2] > 0)
            {
                spriteBatch.Draw(lineTextures[1], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[3], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 3
            if (tree.skillAssignment[3] > 0)
            {
                spriteBatch.Draw(lineTextures[2], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[3], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[4], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 4
            if (tree.skillAssignment[4] > 0)
            {
                spriteBatch.Draw(lineTextures[4], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[5], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[19], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 5
            if (tree.skillAssignment[5] > 0)
            {
                spriteBatch.Draw(lineTextures[5], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[6], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 6
            if (tree.skillAssignment[6] > 0)
            {
                spriteBatch.Draw(lineTextures[6], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[7], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[13], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 7
            if (tree.skillAssignment[7] > 0)
            {
                spriteBatch.Draw(lineTextures[7], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[8], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[10], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 8
            if (tree.skillAssignment[8] > 0)
            {
                spriteBatch.Draw(lineTextures[8], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[9], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 9
            if (tree.skillAssignment[9] > 0)
            {
                spriteBatch.Draw(lineTextures[9], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 10
            if (tree.skillAssignment[10] > 0)
            {
                spriteBatch.Draw(lineTextures[10], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[11], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 11
            if (tree.skillAssignment[11] > 0)
            {
                spriteBatch.Draw(lineTextures[11], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[12], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 12
            if (tree.skillAssignment[12] > 0)
            {
                spriteBatch.Draw(lineTextures[12], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 13
            if (tree.skillAssignment[13] > 0)
            {
                spriteBatch.Draw(lineTextures[13], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[14], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[18], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 14
            if (tree.skillAssignment[14] > 0)
            {
                spriteBatch.Draw(lineTextures[14], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[15], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 15
            if (tree.skillAssignment[15] > 0)
            {
                spriteBatch.Draw(lineTextures[15], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[16], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 16
            if (tree.skillAssignment[16] > 0)
            {
                spriteBatch.Draw(lineTextures[16], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[17], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[19], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion
            #region star 17
            if (tree.skillAssignment[17] > 0)
            {
                spriteBatch.Draw(lineTextures[17], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                spriteBatch.Draw(lineTextures[18], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            #endregion

            #endregion

            spriteBatch.Draw(orionTextureStars, location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);

            if (tree.allowedSkills[star])
            {
                spriteBatch.Draw(greenSelectionStarsTextures[star], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(redSelectionStarsTextures[star], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
            }

            /* Draw what stars you can put points into */
            for (int i = 1; i <= numberOfStars; i++)
            {
                if (i != star && tree.allowedSkills[i])
                {
                    spriteBatch.Draw(starTextures[i], location, source, Color.White, 0f, middleOfTexture, 1.0f, SpriteEffects.None, 1);
                }
            }

            /* What the prompt should be */
            #region Draw Prompt
            string prompt = "default";

            switch(star)
            {
                #region star 1
                case 1:
                    prompt = "Rate of Fire";
                    break;
                #endregion
                #region star 2
                case 2:
                    prompt = "Shot Speed";
                    break;
                #endregion
                #region star 3
                case 3:
                    prompt = "Speed";
                    break;
                #endregion
                #region star 4
                case 4:
                    prompt = "Max Health";
                    break;
                #endregion
                #region star 5
                case 5:
                    prompt = "Pickup Potency";
                    break;
                #endregion
                #region star 6
                case 6:
                    prompt = "Luck";
                    break;
                #endregion
                #region star 7
                case 7:
                    prompt = "Evasion";
                    break;
                #endregion
                #region star 8
                case 8:
                    prompt = "Critical Chance";
                    break;
                #endregion
                #region star 9
                case 9:
                    prompt = "Critical Damage";
                    break;
                #endregion
                #region star 10
                case 10:
                    prompt = "Light Range";
                    break;
                #endregion
                #region star 11
                case 11:
                    prompt = "Placeholder 1";
                    break;
                #endregion
                #region star 12
                case 12:
                    prompt = "Placeholder 2";
                    break;
                #endregion
                #region star 13
                case 13:
                    prompt = "Shot Size";
                    break;
                #endregion
                #region star 14
                case 14:
                    prompt = "Torch Duration";
                    break;
                #endregion
                #region star 15
                case 15:
                    prompt = "Torch Radius";
                    break;
                #endregion
                #region star 16
                case 16:
                    prompt = "Defense";
                    break;
                #endregion
                #region star 17
                case 17:
                    prompt = "Damage";
                    break;
                #endregion
                default:
                    break;
            }
            #endregion

            /* Skill points left */
            string skillPointsLeftStr = "   Available \nSkill Points: " + tree.skillPointsLeft;
            Vector2 fontOriginSkillPointsLeft = Font.MeasureString(skillPointsLeftStr) / 2;
            spriteBatch.DrawString(Font, skillPointsLeftStr, skillPointsLeftPos, Color.White, 0, fontOriginSkillPointsLeft, 1.0f, SpriteEffects.None, 0.5f);
                
            /* Write the text */
            Vector2 fontOrigin = Font.MeasureString(prompt)/2;
            spriteBatch.DrawString(Font, prompt, skillTextPosition, Color.White, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
         
            /* How many skill points are assigned */
            string assignedString = "Lvl: " + tree.skillAssignment[star];
            Vector2 fontOriginSkill = Font.MeasureString(assignedString) / 2;
            spriteBatch.DrawString(Font, assignedString, skillPointsAssignedPos, Color.White, 0, fontOriginSkill, 1.0f, SpriteEffects.None, 0.5f);

            /* Press enter prompt */
            if (tree.allowedSkills[star] && tree.skillPointsLeft > 0)
            {
                string pressEnterString = "Press ENTER to apply point";
                Vector2 fontOriginPressEnter = Font_14.MeasureString(pressEnterString) / 2;
                spriteBatch.DrawString(Font_14, pressEnterString, pressEnterPos, Color.White, 0, fontOriginPressEnter, 1.0f, SpriteEffects.None, 0.5f);
            }

            /* Press space prompt */
            string pressSpaceString = "Press SPACE to reset skill points";
            Vector2 fontOriginPressSpace = Font_14.MeasureString(pressSpaceString) / 2;
            spriteBatch.DrawString(Font_14, pressSpaceString, pressSpacePos, Color.Gray, 0, fontOriginPressSpace, 1.0f, SpriteEffects.None, 0.5f);

            /* Press esc prompt */
            string pressEscString = "Press ESC to leave";
            //Vector2 fontOriginPressEsc = Font_14.MeasureString(pressEscString) / 2;
            Vector2 fontOriginPressEsc = new Vector2(0,0);
            spriteBatch.DrawString(Font_14, pressEscString, pressEscPos, Color.White, 0, fontOriginPressEsc, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public void ExitSkillScreen()
        {
        }
    }
}

#region 17 states to copy paste
/*
                    #region star 1
                    case 1:
                        break;
                    #endregion
                    #region star 2
                    case 2:
                        break;
                    #endregion
                    #region star 3
                    case 3:
                        break;
                    #endregion
                    #region star 4
                    case 4:
                        break;
                    #endregion
                    #region star 5
                    case 5:
                        break;
                    #endregion
                    #region star 6
                    case 6:
                        break;
                    #endregion
                    #region star 7
                    case 7:
                        break;
                    #endregion
                    #region star 8
                    case 8:
                        break;
                    #endregion
                    #region star 9
                    case 9:
                        break;
                    #endregion
                    #region star 10
                    case 10:
                        break;
                    #endregion
                    #region star 11
                    case 11:
                        break;
                    #endregion
                    #region star 12
                    case 12:
                        break;
                    #endregion
                    #region star 13
                    case 13:
                        break;
                    #endregion
                    #region star 14
                    case 14:
                        break;
                    #endregion
                    #region star 15
                    case 15:
                        break;
                    #endregion
                    #region star 16
                    case 16:
                        break;
                    #endregion
                    #region star 17
                    case 17:
                        break;
                    #endregion
*/
#endregion