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
using System.Collections.Generic;
using System.Diagnostics;

namespace AllInOne
{

    /// <summary>
    /// Represents an animation component in the game.
    /// This class handles the drawing and updating of animated sprites based on a texture and a set of frames.
    /// </summary>
    public class Animation:DrawableGameComponent
    {
        private SpriteBatch sb;
        private Texture2D tex;
        private Vector2 position;
        private int delay;
        private Vector2 dimension;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delayCounter;
        //private const int ROWS = 5;
        //private const int COLS = 5;
        private int rows;
        private int cols;
        private float scale;
        private int startX;
        private int startY;
        private bool isLeft;

        public bool IsPlaying { get; private set; }

        public bool IsLeft { get; private set; }

        public Vector2 Position { get => position; set => position = value; }
        private Game g;

        /// <summary>
        /// Initializes a new instance of the Animation class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">Texture for the animated sprite.</param>
        /// <param name="position">Initial position of the animation.</param>
        /// <param name="delay">Frame delay for the animation.</param>
        /// <param name="rows">Number of rows in the animation sprite sheet.</param>
        /// <param name="cols">Number of columns in the animation sprite sheet.</param>
        /// <param name="scale">Scale of the animated sprite.</param>
        /// <param name="startX">X-coordinate of the starting frame in the sprite sheet.</param>
        /// <param name="startY">Y-coordinate of the starting frame in the sprite sheet.</param>
        /// <param name="width">Width of each frame in the sprite sheet.</param>
        /// <param name="height">Height of each frame in the sprite sheet.</param>

        public Animation(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, int delay, 
            int rows, int cols, float scale, int startX, int startY, int width, int height) : base(game)
        {
            this.g = game;
            this.sb = sb;
            this.tex = tex;
            this.Position = position;
            this.delay = delay;
            this.rows = rows;
            this.cols = cols;
            this.startX = startX;
            this.startY = startY;
            this.dimension = new Vector2(width, height);
            createFrames();
            hide();
            this.scale = scale;
        }

        /// <summary>
        /// Hides the animation, making it invisible and inactive.
        /// </summary>

        public void hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }
        /// <summary>
        /// Shows the animation, making it visible and active.
        /// </summary>

        public void show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// Creates the frames for the animation from the sprite sheet.
        /// </summary>

        private void createFrames()
        {
            frames = new List<Rectangle>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int x = j * (int)dimension.X+ startX;
                    int y = i * (int)dimension.Y+ startY;
                    Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(r);
                }
            }
        }

        /// <summary>
        /// Updates the animation, changing frames based on the delay.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                delayCounter++;
                if (delayCounter > delay*2)
                {
                    frameIndex++;
                    if (frameIndex > rows * cols - 1)
                    {
                        frameIndex = 0;
                    }
                    delayCounter = 0;
                }

            }

           
            base.Update(gameTime);
        }
        /// <summary>
        /// Draws the current frame of the animation on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
           
            if (frameIndex >= 0)
            {
                if(IsLeft)
                {
                    sb.Draw(tex, position, frames[frameIndex], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    sb.Draw(tex, position, frames[frameIndex], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
               
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Starts playing the animation.
        /// </summary>

        public void Play()
        {

            IsPlaying = true;

        }

        /// <summary>
        /// Stops the animation.
        /// </summary>

        public void Stop()
        {

            IsPlaying = false;

        }
        /// <summary>
        /// Sets the direction of the animation.
        /// </summary>
        /// <param name="isLeft">Indicates if the animation should face left.</param>

        public void IsLeftSide(bool isLeft)
        {
            IsLeft = isLeft;

        }
        /// <summary>
        /// Sets the position of the animation.
        /// </summary>
        /// <param name="curPosition">The current position of the animation.</param>

        public void SetPosition(Vector2 curPosition)
        {
            position = curPosition;
        }

    }
}
