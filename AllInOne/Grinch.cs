/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SharpDX.Direct2D1.Effects;

namespace AllInOne
{
    /// <summary>
    /// Represents a Grinch character in the game.
    /// The Grinch is an animated component that can collide with other objects.
    /// </summary>
    public class Grinch : DrawableGameComponent
    {
       
        private SpriteBatch sb;
        public Texture2D tex;
        public Vector2 position;
        private Vector2 speed;
        private float scale;
        private Vector2 dimension;
        public List<Rectangle> frames;
        private int frameIndex = 0;
        private const int ROWS = 1;
        private const int COLS = 5;
        private int delayCounter;
        private int delay;
        public bool HasCollided { get; private set; }
        public Vector2 Position { get => position; set => position = value; }
        private Game g;

        /// <summary>
        /// Initializes a new instance of the Grinch class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">Texture for the Grinch sprite.</param>
        /// <param name="position">Initial position of the Grinch.</param>
        /// <param name="speed">Speed at which the Grinch moves.</param>
        /// <param name="scale">Scale of the Grinch sprite.</param>
        /// <param name="delay">Frame delay for the animation.</param>


        public Grinch(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, Vector2 speed, float scale, int delay) : base(game)
        {
            this.sb = sb;
            this.tex = tex;
            this.position = position;
            this.speed = speed;
            this.scale = scale;
            this.delay = delay;
            this.dimension = new Vector2(tex.Width / COLS, tex.Height / ROWS);
            createFrames();

            //this.stage = stage;
        }
        /// <summary>
        /// Creates animation frames for the Grinch.
        /// </summary
        private void createFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;
                    Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(r);
                }
            }
        }

        /// <summary>
        /// Handles the collision behavior of the Grinch.
        /// </summary>
        public void Collide()
        {
            if (!HasCollided)
            {
                HasCollided = true;
               // this.tex = collidedTexture;
            }
            else
            {
                HasCollided = false;
            }
        }
        /// <summary>
        /// Updates the state of the Grinch, including position and animation.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            // Increment the delay counter
            delayCounter++;

            if (ks.IsKeyDown(Keys.Left))
            {

                if (delayCounter > delay)
                {
                    frameIndex = (frameIndex + 1) % frames.Count;
                    delayCounter = 0; // Reset the delay counter
                }

                position += speed;


            }
            if (ks.IsKeyDown(Keys.Right))
            {
                if (delayCounter > delay)
                {
                    frameIndex = (frameIndex + 1) % frames.Count;
                    delayCounter = 0; // Reset the delay counter
                }

                position -= speed;

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the Grinch on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(tex, position, frames[frameIndex], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the bounding rectangle of the Grinch for collision detection.
        /// </summary>
        /// <returns>The bounding rectangle.</returns>

        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)((tex.Width / 9) * scale), (int)(tex.Height * scale));
        }
    }
}
