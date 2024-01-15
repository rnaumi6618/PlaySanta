/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;

namespace AllInOne
{
    /// <summary>
    /// Represents a snowball in the game, managing its behavior and rendering.
    /// </summary>
    public class SnowBall : DrawableGameComponent
    {
        private SpriteBatch sb;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 speed;
        private Vector2 stage;
        private float scale;
        // private SoundEffect hitSound;
        // private SoundEffect missSound;
        private SnowBall SnowBall1;
        private Santa santa;
        private double delayTime;
        private double elapsedTime = 0;
        private bool hasStarted = false;

        /// <summary>
        /// Gets or sets the speed of the snowball.
        /// </summary>
        public Vector2 Speed { get => speed; set => speed = value; }

        /// <summary>
        /// Initializes a new instance of the SnowBall class.
        /// </summary>
        /// <param name="game">The main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">The texture for the snowball.</param>
        /// <param name="santa">Reference to the Santa object in the game.</param>
        /// <param name="speed">The speed of the snowball.</param>
        /// <param name="stage">The stage dimensions.</param>
        /// <param name="scale">The scale of the snowball.</param>
        /// <param name="delay">The delay before the snowball starts moving.</param>
        public SnowBall(Game game, SpriteBatch sb,
            Texture2D tex, Santa santa, Vector2 speed, Vector2 stage, float scale, double delay) : base(game)
        {
            this.sb = sb;
            this.tex = tex;
            this.position = new Vector2((int)stage.X, 0);
            this.speed = speed;
            this.stage = stage;
            this.scale = scale;
            this.delayTime = 4;
            this.santa = santa;
            elapsedTime = 0;
        }

        /// <summary>
        /// Updates the snowball's behavior each frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Check if the snowball has reached the bottom of the stage
            if (position.Y > stage.Y - tex.Height)
            {
                // Reset snowball state and position
                this.Enabled = false;
                this.Visible = false;

                Random r = new Random();
                Vector2 nextPosition = new Vector2((int)santa.position.X, 0);
                position = nextPosition;
                hasStarted = false;
                elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            }

            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds - elapsedTime;

            // Start moving the snowball after the delay
            if (!hasStarted && (elapsedTime % delayTime == 0))
            {
                hasStarted = true;
                this.Enabled = true;
                this.Visible = true;
            }

            // Update position if snowball has started moving
            if (hasStarted)
            {
                position -= speed;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the snowball on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(tex, position, Color.White);
            sb.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the bounding rectangle of the snowball for collision detection.
        /// </summary>
        /// <returns>The bounding rectangle of the snowball.</returns>
        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)((tex.Width / 9) * scale), (int)(tex.Height * scale));
        }
    }
}