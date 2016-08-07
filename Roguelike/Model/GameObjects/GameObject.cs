#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.View;
using Shadows2D;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Projectiles;
using Roguelike.Util;
#endregion

namespace Roguelike.Model.GameObjects
{
    public enum GameObjectType
    {
        Interactable,
        Monster,
        Light,
        None
    }

    abstract public class GameObject
    {
        public Level currentLevel;

        public LayerType layerType = LayerType.None;
        public GameObjectType gameObjectType = GameObjectType.None;

        /* The filename of the texture to be loaded into the texture2D variable */
        protected String myTextureFileName;
        public Texture2D texture2D;

        /* height and width for of the texture to be drawn */
        public int textureHeight;
        public int textureWidth;

        /* These are for computation saving */
        public int textureHeightOver2;
        public int textureWidthOver2;

        /* This is the center of the object in the world, and what is passed into the constructor */
        public Vector2 worldCenter;

        /* This is the draw location of the object (upper LH corner) */
        public Vector2 drawLocation;

        /* This is the origin of the image, or the offset to the center in pixel values relative to the texture. */
        public Vector2 origin;

        public HashSet<ATile> overlappingTiles = new HashSet<ATile>();

        protected float textureRotation = 0;

        //protected List<Vector2> boundingPointsOffset;
        public Rectangle boundingBox = new Rectangle();
        public Point boundingBoxOffset = new Point();

        protected static Texture2D dot;

        protected Color shadowLevel_ = Color.White;

        public Vector2 forceVector = new Vector2();
        public List<Vector2> forcesActingOnMe = new List<Vector2>();
        public float inertia_0_to_1 = 0.5f;
        public bool isStaticObject = true;
        protected float maxForceClamp; // defaults to double speed, probably. can be overridden

        // collision checking 
        List<GameObject> collisions = new List<GameObject>();
        HashSet<GameObject> objectsToCheck = new HashSet<GameObject>();

        public string killText = "You Lose!";

        public Color ShadowLevel
        {
            get
            {
                return shadowLevel_;
            }

            set
            {
                shadowLevel_ = value;
            }
        }

        public GameObject()
        {

            SetTexture();
        }

        public GameObject(Level level, int startX, int startY)
        {
            if (dot == null)
            {
                dot = new Texture2D(level.gameModel.Game.GraphicsDevice, 5, 5);

                Color[] data = new Color[5 * 5];
                for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
                dot.SetData(data);
            }

            currentLevel = level;

            worldCenter = new Vector2(startX, startY);
            boundingBox.X = startX;
            boundingBox.Y = startY;

            //boundingPointsOffset = new List<Vector2>();

            SetTexture();

            //GraphicsDevice device = currentLevel.gameModel.Game.GraphicsDevice;
            //new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height);

            //drawLocation = new Vector2(&xPos, &yPos);
            /*You must call SetBoundingPointsOffset() in the implementing class's constructor */
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        //abstract public void LoadContent();
        virtual public void LoadContent()
        {
            texture2D = currentLevel.gameModel.Game.Content.Load<Texture2D>(myTextureFileName);

            textureHeight = texture2D.Height;
            textureHeightOver2 = textureHeight / 2;

            textureWidth = texture2D.Width;
            textureWidthOver2 = textureWidth / 2;

            drawLocation = new Vector2(worldCenter.X - textureWidthOver2, worldCenter.Y - textureHeightOver2);
            origin = new Vector2(textureWidthOver2, textureHeightOver2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        abstract public void UnloadContent();

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        abstract public void Update(GameTime gameTime);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        abstract public void Draw(SpriteBatchWrapper spriteBatch);

        /// <summary>
        /// Just making this a function so you don't forget to set the texture string of the 
        /// GameObject. This is the correct place to set this string.
        /// It is called automagically.
        /// </summary>
        abstract public void SetTexture();

        /// <summary>
        /// This function is called when dealing damage to a game object. The implented function
        /// is responsible for affecting its health and playing an animation or whatever.
        /// 
        /// To make my life easier, and since it isn't something that everything has to implement, 
        /// I'm not making it abstract, and instead, you can just override it when you need it.
        /// </summary>
        /// <param name="fromMe">The object doing the damaging (usually pass "this")</param>
        /// <param name="thisMuch">The amount of damage to be dealt</param>
        public virtual void getDealtDamage(GameObject fromMe, float thisMuch, GameTime gameTime)
        {
            throw new NotImplementedException("Invalid: Why are you dealing damage to this without overriding the method?");
        }

        /*
         * You need to implement IsObstacle() and SetBoundingPointsOffset() if you want to have collision checking
         * for your GameObject. Also, the derived class must call SetBoundingPointsOffset() in its constructor.
         */

        /// <summary>
        /// True if this object is solid for the purposes of valid motion. False otherwise.
        /// </summary>
        /// <returns> True if this object is solid for the purposes of collision detection.</returns>
        virtual public bool IsObstacle()
        {
            return false;
        }

        /// <summary>
        /// Sets a list of offsets for points defining a convex boundary for the object.
        /// These offsets are relative to the center of the object. For example, to get the 
        /// top left hand corner of a box, the offset would be <-width/2, -height/2>.
        /// </summary>
        virtual public void SetBoundingPointsOffset()
        {
            return;
        }

        protected void SetBoundingPointsOffset(int width, int height)
        {
            boundingBoxOffset = new Point((width / 2), (height / 2));
            boundingBox = new Rectangle(((int)worldCenter.X - (int)boundingBoxOffset.X), ((int)worldCenter.Y - (int)boundingBoxOffset.Y), width, height);
        }

        /// <summary>
        /// Call when the gameObject is shot by a bullet
        /// </summary>
        /// <param name="bulletType"> The type of bullet the object was shot by </param>
        virtual public void ShotByBullet(BulletType bulletType, GameTime gameTime)
        {
            /* This needs to be implemented if you want the GameObject to do stuff when shot. */
            return;
        }

        /// <summary>
        /// Returns true if the object will cast a shadow
        /// </summary>
        /// <returns> True if the object casts a shadow </returns>
        virtual public bool CastsShadow()
        {
            return false;
        }

        /// <summary>
        /// Returns true if the object will cast a light
        /// </summary>
        /// <returns>True if the object casts a light </returns>
        virtual public bool CastsLight()
        {
            return false;
        }

        /// <summary>
        /// Draws the light if the object casts a light. You must implement
        /// this if your object casts light
        /// </summary>
        virtual public LightSource GetLightSource()
        {
            return null;
        }

        /// <summary>
        /// Draws the object to the ShadowCasterMap if the object casts a 
        /// shadow. Implement this for moving things.
        /// </summary>
        /// <param name="shadowMap"></param>
        virtual public void DrawCaster(ShadowCasterMap shadowMap)
        {
            if (this.CastsShadow())
            {
                Vector2 location = new Vector2(worldCenter.X - textureWidth / 2, worldCenter.Y - textureHeight / 2);
                shadowMap.AddShadowCaster(texture2D, location, textureWidth, textureHeight);
            }
        }

        /// <summary>
        /// Returns a list of points defining a convex boundary for the object. It must be in 
        /// the correct order. So a triangle would have points 1 to 2, 2 to 3, 3 to 1 to form
        /// the sides of the triangle.
        /// </summary>
        /// <returns>See the summary</returns>
        /*
        public List<Vector2> GetCollisionPoints()
        {
            List<Vector2> toReturn = new List<Vector2>();

            for (int i = 0; i < boundingPointsOffset.Count; i++ )
            {
                float xOffset = boundingPointsOffset[i].X;
                float yOffset = boundingPointsOffset[i].Y;
                Vector2 newPoint = new Vector2(worldCenter.X + xOffset, worldCenter.Y + yOffset);
                toReturn.Add(newPoint);
            }

            return toReturn;

        }*/

        /// <summary>
        /// Returns a list of GameObjects this one intersects with
        /// </summary>
        /// <param name="points">A list of GameObjects this GameObject collides with</param>
        /// <returns></returns>
        public List<GameObject> CollidesWith()
        {
            boundingBox.X = (int)worldCenter.X - boundingBoxOffset.X;
            boundingBox.Y = (int)worldCenter.Y - boundingBoxOffset.Y;

            collisions.Clear();

            /* Get a list of GameObjects from those ATiles */
            objectsToCheck.Clear();

            foreach (ATile tile in overlappingTiles)
            {
                /* If the tile itself is an obstacle, then we need to check it too */
                if (tile.IsObstacle())
                {
                    objectsToCheck.Add(tile);
                }

                HashSet<GameObject> blah = tile.overlappingGameObjects;
                foreach (GameObject g in blah)
                {
                    objectsToCheck.Add(g);
                }
            }

            /* Check if you collide with any objects in objectsToCheck */

            /* For each of your lines, make sure they don't intersect with lines of objects in objectsToCheck */
            foreach (GameObject them in objectsToCheck)
            {

                /* Don't check collision with self */
                if (them == this)
                {
                    continue;
                }

                if (boundingBox.Intersects(them.boundingBox))
                {
                    collisions.Add(them);
                }
            }

            return collisions;
        }

        /// <summary>
        /// Sets the rotation of the tile. Purely for cosmetic things.
        /// </summary>
        /// <param name="rotation">The radians to rotate the tile. + MathHelper.Pi will turn it upside down</param>
        public void setRotation(int half_pis)
        {

            textureRotation = half_pis * MathHelper.PiOver2;

            // rotation by 90 degrees
            if (half_pis % 2 == 1)
            {
                Point c = boundingBox.Center;
                boundingBox = new Rectangle((c.X - (boundingBox.Height / 2)), (c.Y - (boundingBox.Width / 2)), boundingBox.Height, boundingBox.Width);
            }

            else
            {
                // do nothing; the bounding square will be the same
            }

        }

        /// <summary>
        /// Updates the references to GameObjects in overlapping ATiles and updates the list 
        /// of overlapping ATiles in the GameObject
        /// </summary>
        public void UpdateATiles()
        {
            /* Remove reference to this in all the ATiles */
            foreach (ATile t in overlappingTiles)
            {
                t.removeGameObjectReference(this);
            }

            /* Update list of ATiles*/
            overlappingTiles.Clear();

            int leftMost = boundingBox.X;
            int rightMost = boundingBox.X + boundingBox.Width;
            int topMost = boundingBox.Y;
            int bottomMost = boundingBox.Y + boundingBox.Height;

            /* Make square of tiles this overlaps */
            for (float i = leftMost; i <= rightMost + currentLevel.theWorld.tileWidth; i += currentLevel.theWorld.tileWidth)
            {
                for (float j = topMost; j <= bottomMost + currentLevel.theWorld.tileHeight; j += currentLevel.theWorld.tileHeight)
                {
                    Vector2 tilePoint = new Vector2(i, j);

                    ATile overlapping = currentLevel.GetATileAt(tilePoint);
                    if (overlapping != null) // This check is because the main char can still spawn outside the game area
                    {
                        //if (overlappingTiles.Contains(overlapping) == false)
                        //{
                        overlappingTiles.Add(overlapping);
                        overlapping.addGameObjectReference(this);
                        //}

                    }
                }
            }
        }

        /// <summary>
        /// Returns whether or not the GameObject can move into the given x and y position.
        /// </summary>
        /// <param name="x">x position to check</param>
        /// <param name="y">y position to check</param>
        /// <returns>False if there is an obstacle in the way. True otherwise.</returns>
        public bool ValidMovement(float x, float y, GameTime gameTime)
        {
            bool valid = true;

            float oldxPos = worldCenter.X;
            float oldyPos = worldCenter.Y;

            boundingBox.X = (int)x - boundingBoxOffset.X;
            boundingBox.Y = (int)y - boundingBoxOffset.Y;

            worldCenter.X = x;
            worldCenter.Y = y;

            UpdateATiles();

            List<GameObject> wouldCollideWith = CollidesWith();

            for (int i = 0; i < wouldCollideWith.Count; i++)
            {
                GameObject collision = wouldCollideWith[i];

                /* Checks for specific collision cases (that aren't just stopping movement) */
                WouldCollideCases(collision, gameTime);

                if (collision.IsObstacle() && (collision.GetType() != typeof(TorchBullet) && collision.GetType() != typeof(GlowStick)))
                {

                    if (this.GetType() == typeof(MainCharacter) && collision.GetType() == typeof(SpiderMonster))
                    {
                        valid = false;
                    }

                    valid = false;

                    /* No break in case we want to put an interactable on a wall or something */
                }
            }

            worldCenter.X = oldxPos;
            worldCenter.Y = oldyPos;

            boundingBox.X = (int)oldxPos - boundingBoxOffset.X;
            boundingBox.Y = (int)oldyPos - boundingBoxOffset.Y;

            UpdateATiles();

            return valid;
        }

        /// <summary>
        /// Removes reference of this GameObject from tiles it is on
        /// </summary>
        public void RemoveFromATiles()
        {
            /* Remove reference to this in all the ATiles */
            foreach (ATile t in overlappingTiles)
            {
                t.removeGameObjectReference(this);
            }
        }

        /// <summary>
        /// Deals with the various cases where one obstacle would hit another (but doesn't
        /// because it can't move into it because it is an obstacle) and what should happen.
        /// This function should be called during Valid Movement. 
        /// </summary>
        /// <param name="collision">GameObject that THIS GameObject would hit</param>
        private void WouldCollideCases(GameObject collision, GameTime gameTime)
        {
            /* Check if this is a player triggering an interactable */
            if (this.GetType() == typeof(MainCharacter) && collision.gameObjectType == GameObjectType.Interactable)
            {
                AInteractable interact = (AInteractable)collision;
                interact.TriggerPlayerInteraction();
            }

            /* Check if this is a player running into a monster */
            if (this.GetType() == typeof(MainCharacter) && collision.gameObjectType == GameObjectType.Monster)
            {
                AMonster monster = (AMonster)collision;
                monster.DealDamage(gameTime);
            }

            /* Checks if this is a monster running into a player */
            if (this.gameObjectType == GameObjectType.Monster && collision.GetType() == typeof(MainCharacter))
            {
                AMonster monster = (AMonster)this;
                monster.DealDamage(gameTime);
                //monster.Push(forceVector * collision.inertia_0_to_1); // probably don't do this
            }

            // see if we should push shit
            //if (!collision.isStaticObject && collision.GetType() != typeof(MainCharacter)) {
            //    collision.PushNormal(forceVector * inertia_0_to_1);
            //}

            if (!this.isStaticObject && !collision.isStaticObject && (typeof(MainCharacter) != collision.GetType()))
            {
                //Console.WriteLine("doop");
                collision.PushSmart(forceVector * inertia_0_to_1, worldCenter);
            }
        }

        public virtual void ComputeVelocity(GameTime gameTime)
        {

        }

        public virtual void ApplyForceToOtherObjects(GameTime gameTime)
        {

        }

        public virtual void Move(GameTime gameTime)
        {
            if (!isStaticObject)
            {
                MathHelperHelper.Vector2Int(ref forceVector);

                if (forceVector.Length() > maxForceClamp) // as defined per object
                {
                    forceVector = MathHelperHelper.Vector2Normalize(forceVector) * maxForceClamp;
                }

                worldCenter += forceVector;
                boundingBox.X = (int)worldCenter.X - (boundingBox.Width / 2);
                boundingBox.Y = (int)worldCenter.Y - (boundingBox.Height / 2);

                UpdateATiles();
            }
        }

        public virtual void Push(Vector2 force)
        {
            forcesActingOnMe.Add(force);
        }

        public virtual void PushNormal(Vector2 force)
        {
            // do this clockwise-ly, but this may be wrong idk
            Vector2 normalForce = new Vector2(-1 * force.Y, force.X);

            forcesActingOnMe.Add(normalForce);
        }

        public virtual void PushBoth(Vector2 force)
        {
            //TODO: smarter pushing vector
            float distance = force.Length();

            Vector2 newForce = force + (new Vector2(-1 * force.Y, force.X) * 0.5f);

            //MathHelperHelper.Vector2Normalize(ref newForce);
            //forcesActingOnMe.Add(newForce * distance);

            forcesActingOnMe.Add(newForce);
        }

        public virtual void PushSmart(Vector2 force, Vector2 collisionCenter)
        {
            // the vector from x to y is given by (y - x)
            Vector2 position = worldCenter - collisionCenter;
            float m = MathHelperHelper.CrossProduct(force, position);

            Vector2 normalForce;

            // go clockwise
            if (m > 0)
            {
                normalForce = new Vector2(-1 * force.Y, force.X);
            }

            else
            {

                normalForce = new Vector2(force.Y, -1 * force.X);
            }

            float length = force.Length();

            Vector2 byOurPowersCombined = MathHelperHelper.Vector2Normalize(force + (2 * normalForce));

            byOurPowersCombined *= length;

            forcesActingOnMe.Add(byOurPowersCombined);
        }


    }

}
