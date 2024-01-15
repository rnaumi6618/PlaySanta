/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents a scrolling background in the game.
    /// This class handles the drawing and updating of a continuous, scrolling background image.
    /// </summary>
    internal class Background : DrawableGameComponent
    {
        private SpriteBatch sb;
        private Texture2D tex;
        private Vector2 position1, position2;
        private Vector2 speed;
        private Rectangle srcRect;

        /// <summary>
        /// Initializes a new instance of the Background class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">Texture for the background image.</param>
        /// <param name="position">Initial position of the background image.</param>
        /// <param name="speed">Speed at which the background scrolls.</param>
        /// <param name="srcRect">Source rectangle for the background texture.</param>

        public Background(Game game, SpriteBatch sb, Texture2D tex,
            Vector2 position, Vector2 speed, Rectangle srcRect) : base(game)
        {
            this.sb = sb;
            this.tex = tex;
            this.position1 = position;
            this.position2 = new Vector2(position1.X + srcRect.Width, position1.Y);
            this.speed = speed;
            this.srcRect = srcRect;
        }
        /// <summary>
        /// Draws the scrolling background on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            //v4
            sb.Draw(tex, position1, srcRect, Color.White);
            sb.Draw(tex, position2, srcRect, Color.White);
            sb.End();

            base.Draw(gameTime);

        }
        /// <summary>
        /// Updates the position of the background for scrolling effect.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
           
            if (ks.IsKeyDown(Keys.Left))
            {

                speed = new Vector2(-8, 0);
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                speed = new Vector2(8, 0);
            }
            else
            {
                speed = new Vector2(0, 0);
            }
            position1 -= speed;
            position2 -= speed;
            // Check if images scroll off the left side and reposition
            if (position1.X < -srcRect.Width)
            {
                position1.X = position2.X + srcRect.Width;
            }
            if (position2.X < -srcRect.Width)
            {
                position2.X = position1.X + srcRect.Width;
            }

            // New checks: Check if images scroll off the right side and reposition
            if (position1.X > srcRect.Width)
            {
                position1.X = position2.X - srcRect.Width;
            }
            if (position2.X > srcRect.Width)
            {
                position2.X = position1.X - srcRect.Width;
            }
            base.Update(gameTime);

        }

    }
}
