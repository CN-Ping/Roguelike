using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Roguelike.Model;

namespace Roguelike.View
{
    public class SpriteBatchWrapper
    {
        static Model.Model gameModel;
        public SpriteBatch s;

        public SpriteBatchWrapper(GraphicsDevice g) {
        }

        public SpriteBatchWrapper(GraphicsDevice g, Model.Model m) {
            s = new SpriteBatch(g);
            gameModel = m;
        }

        public void Draw(Texture2D t, Rectangle destination, Color c)
        {
            if (gameModel != null)
            {
                Rectangle r = new Rectangle(destination.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), destination.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2), destination.Width, destination.Height);
                s.Draw(t, r, c);
            }
            
        }
        
        public void Draw (Texture2D t, Rectangle destination, Nullable<Rectangle> source, Color c) {
            if (!source.HasValue)
            {
                Draw(t, destination, c);
                return;
            }

            else
            {
                Rectangle r = new Rectangle(destination.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), destination.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2), destination.Width, destination.Height);
                s.Draw(t, r, source, c);
            }
        }

        public void Draw (Texture2D t, Rectangle destination, Nullable<Rectangle> source, Color c, Single rotation, Vector2 origin, SpriteEffects effects, Single layer) {
            Rectangle r = new Rectangle(destination.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), destination.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2), destination.Width, destination.Height);
                s.Draw(t, r, source, c, rotation, origin, effects, layer);
        }

        public void Draw (Texture2D t, Vector2 position, Color c) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.Draw(t, p, c);
        }

        public void Draw (Texture2D t, Vector2 position, Nullable<Rectangle> source, Color c) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
                s.Draw(t, p, source, c);
        }

        public void Draw (Texture2D t, Vector2 position, Nullable<Rectangle> source, Color c, Single rotation, Vector2 origin, Single scale, SpriteEffects effect, Single layer) {

            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
                s.Draw(t, p, source, c, rotation, origin, scale, effect, layer);

        }

        public void Draw (Texture2D t, Vector2 position, Nullable<Rectangle> source, Color c, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, Single layer) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
                s.Draw(t, p, source, c, rotation, origin, scale, effect, layer);
        }

        public void DrawString (SpriteFont font, String text, Vector2 position, Color c) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c);
        }

        public void DrawString (SpriteFont font, String text, Vector2 position, Color c, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layer)	{
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c, rotation, origin, scale, effects, layer);
        }

        public void DrawString (SpriteFont font, String text, Vector2 position, Color c, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layer) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c, rotation, origin, scale, effects, layer);
        }

        public void DrawString (SpriteFont font, StringBuilder text, Vector2 position, Color c) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c);
        }

        public void DrawString (SpriteFont font, StringBuilder text, Vector2 position, Color c, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layer){
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c, rotation, origin, scale, effects, layer);
        }

        public void DrawString (SpriteFont font, StringBuilder text, Vector2 position, Color c, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layer) {
            Vector2 p = new Vector2(position.X - (gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2), position.Y - (gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));
            s.DrawString(font, text, p, c, rotation, origin, scale, effects, layer);
        }

        public void End()
        {
            s.End();
        }

        public void Begin()
        {
            s.Begin();
        }

        public void Begin(SpriteSortMode mode, BlendState state)
        {
            s.Begin(mode, state);
        }

        public void Begin(SpriteSortMode mode, BlendState blendState, SamplerState samplerState, DepthStencilState depth, RasterizerState rasterizer)
        {
            s.Begin(mode, blendState, samplerState, depth, rasterizer);
        }

        public void Begin(SpriteSortMode mode, BlendState blendState, SamplerState samplerState, DepthStencilState depth, RasterizerState rasterizer, Effect e)
        {
            s.Begin(mode, blendState, samplerState, depth, rasterizer, e);
        }

        public void Begin(SpriteSortMode mode, BlendState blendState, SamplerState samplerState, DepthStencilState depth, RasterizerState rasterizer, Effect e, Matrix m)
        {
            s.Begin(mode, blendState, samplerState, depth, rasterizer, e, m);
        }

    }
}
