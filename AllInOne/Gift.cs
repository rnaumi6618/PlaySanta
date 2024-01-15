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
using static System.Net.Mime.MediaTypeNames;

namespace AllInOne
{
    /// <summary>
    /// Represents a gift object in the game.
    /// The gift can move based on player input and has a defined position, texture, and scale.
    /// </summary>
    public class Gift : DrawableGameComponent
    {
        private SpriteBatch sb;
        public Texture2D tex;
        public Vector2 position;
        private float scale;
        private Vector2 speed;
        public Vector2 dimension; 


        //public Vector2 Position { get => position; set => position = value; }
        private Game g;

        /// <summary>
        /// Initializes a new instance of the Gift class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">Texture for the gift sprite.</param>
        /// <param name="position">Initial position of the gift.</param>
        /// <param name="scale">Scale of the gift sprite.</param>
        /// <param name="speed">Speed at which the gift moves.</param>

        public Gift(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, float scale,Vector2 speed) : base(game)
        {
            this.sb = sb;
            this.tex = tex;
            this.dimension = new Vector2(tex.Width, tex.Height);
            this.position = position;
            this.scale = scale;
            this.speed = speed;
        }

        /// <summary>
        /// Updates the state of the gift, including its position based on player input.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

           
          

            if (ks.IsKeyDown(Keys.Left))
            {

                position += speed;


            }
            if (ks.IsKeyDown(Keys.Right))
            {
                
                position -= speed;

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the gift on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {

            sb.Begin();
            //sb.Draw(tex, position, Color.White);
            //sb.Draw(tex, position, Color.White);
            sb.Draw(tex, position, new Rectangle(0, 0, (int)this.dimension.X, (int)this.dimension.Y), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the bounding rectangle of the gift for collision detection.
        /// </summary>
        /// <returns>The bounding rectangle.</returns>

        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)((tex.Width) * scale), (int)(tex.Height * scale));
        }
    }
}



