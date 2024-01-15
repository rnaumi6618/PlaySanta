/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;

namespace AllInOne
{  /// <summary>
   /// Defines the various states Santa can be in during the game.
   /// </summary>
    public enum SantaState
    {
        Idle,
        Running,
        Jumping,
        Falling
    }
    /// <summary>
    /// Represents the Santa character in the game, managing its state, animations, and interactions.
    /// </summary>
    public class Santa : DrawableGameComponent
    {
        public const int SANTA_DEFAULT_SPRITE_POS_X = 0;
        public const int SANTA_DEFAULT_SPRITE_POS_Y = 0;
        public const int SANTA_DEFAULT_SPRITE_WIDTH = 300;
        public const int SANTA_DEFAULT_SPRITE_HEIGHT = 340;

        public const int SANTA_JUMP_SPRITE_POS_X = 1200;
        public const int SANTA_JUMP_SPRITE_POS_Y = 0;
        public const int SANTA_RUNNING_SPRITE_POS_X = 300;
        public const int SANTA_RUNNING_SPRITE_POS_Y = 0;
        public const int SANTA_RUNNING_SPRITE_COLUMS = 2;
        public const int SANTA_RUNNING_SPRITE_ROWS = 1;
        public const int SANTA_RUNNING_SPEED = 6;

        public const float SANTA_START_JUMP_POWER = -800f;
        private const float SANTA_CANCEL_JUMPL_SPEED = -100f;
        private const float MIN_JUMP_HEIGHT = 100f;
        private const float FALL_VELOCITY = 600f;
        private const float GRAVITY = 1600f;
        public int lives=3;

        private Game g;
        private SpriteBatch sb;
        public Texture2D tex;
        public Vector2 position;
        private Vector2 speed;
        private float imgScale;
        private float jumpPower;
        private float startPosY;
        private float jumpVelocity;
        private float fallVelocity;

        public SantaState State { get; private set; }
        Animation runAnimation;
        KeyboardState previousKeyboardState;


        /// <summary>
        /// Initializes a new instance of the Santa class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="tex">Texture for Santa's sprite.</param>
        /// <param name="position">Initial position of Santa.</param>
        /// <param name="speed">Speed at which Santa moves.</param>
        /// <param name="scale">Scale of Santa's sprite.</param>

        public Santa(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, Vector2 speed,float scale) : base(game)
        {
            this.g = game;
            this.sb = sb;
            this.tex = tex;
            this.position = position;
            this.startPosY = position.Y;
            this.speed = speed;
            this.imgScale = scale;
            State = SantaState.Idle;
            runAnimation = new Animation(this.g, this.sb, this.tex, this.position, 
                            SANTA_RUNNING_SPEED, SANTA_RUNNING_SPRITE_ROWS, SANTA_RUNNING_SPRITE_COLUMS, scale, SANTA_RUNNING_SPRITE_POS_X, SANTA_RUNNING_SPRITE_POS_Y,
                            SANTA_DEFAULT_SPRITE_WIDTH, SANTA_DEFAULT_SPRITE_HEIGHT);
            this.g.Components.Add(runAnimation);
        }
        /// <summary>
        /// Updates Santa's state and position based on user input and game physics.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();


            // Process Moving left/right
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                position -= speed;
                if (position.X < 0)
                {
                    position.X = 0;
                }

                runAnimation.IsLeftSide(true);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                position += speed;
                if (position.X > Shared.stage.X - SANTA_DEFAULT_SPRITE_WIDTH*0.4f)
                {
                    position.X = Shared.stage.X - SANTA_DEFAULT_SPRITE_WIDTH*0.4f;
                }
                runAnimation.IsLeftSide(false);
            }

            // Process Jumping   
            if ((State == SantaState.Idle) && (keyboardState.IsKeyDown(Keys.Right) || (keyboardState.IsKeyDown(Keys.Left))))
            {
                State = SantaState.Running;
                runAnimation.Play();
            }
            else if(State == SantaState.Running)
            {
                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasJumpKeyPressed = previousKeyboardState.IsKeyDown(Keys.Up) || previousKeyboardState.IsKeyDown(Keys.Space);

                if (!wasJumpKeyPressed && isJumpKeyPressed)
                {

                    if (State != SantaState.Jumping)
                    {
                        Jump();
                    }

                }
                else if (State == SantaState.Jumping && !isJumpKeyPressed)
                {

                    CancelJump();

                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {

                    if (State == SantaState.Jumping || State == SantaState.Falling)
                    {
                        Fall();
                    }

                }
                runAnimation.Update(gameTime);
            }
            else if ((State == SantaState.Jumping) || (State == SantaState.Falling))
            {
                position.Y = position.Y + jumpVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds + fallVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
 
                jumpVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jumpVelocity >= 0)
                {
                    State = SantaState.Falling;
                }
                 

                if (position.Y >= startPosY)
                {

                    position.Y = startPosY;
                    jumpVelocity = 0;
                    State = SantaState.Running;

                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws Santa on the screen based on the current state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
           
            if (State == SantaState.Idle)
            {
                this.sb.Draw(this.tex, position,
                    new Rectangle(SANTA_DEFAULT_SPRITE_POS_X, SANTA_DEFAULT_SPRITE_POS_Y, SANTA_DEFAULT_SPRITE_WIDTH, SANTA_DEFAULT_SPRITE_HEIGHT),
                    Color.White, 0f, Vector2.Zero, imgScale, SpriteEffects.None, 0f);

            }
            else if(State == SantaState.Running)
            {
                runAnimation.SetPosition(position);
                runAnimation.Draw(gameTime);
                
            }
            else if((State == SantaState.Jumping) || (State == SantaState.Falling))
            {
                if (runAnimation.IsLeft)
                {
                    this.sb.Draw(this.tex, position,
                     new Rectangle(SANTA_JUMP_SPRITE_POS_X, SANTA_JUMP_SPRITE_POS_Y, SANTA_DEFAULT_SPRITE_WIDTH, SANTA_DEFAULT_SPRITE_HEIGHT),
                    Color.White, 0f, Vector2.Zero, imgScale, SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    this.sb.Draw(this.tex, position,
                     new Rectangle(SANTA_JUMP_SPRITE_POS_X, SANTA_JUMP_SPRITE_POS_Y, SANTA_DEFAULT_SPRITE_WIDTH, SANTA_DEFAULT_SPRITE_HEIGHT),
                     Color.White, 0f, Vector2.Zero, imgScale, SpriteEffects.None, 0f);
                }
         
            }

            sb.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Initiates Santa's jump if not already jumping or falling.
        /// </summary>
        /// <returns>True if the jump is initiated, false otherwise.</returns>

        public bool Jump()
        {
            if (State == SantaState.Jumping || State == SantaState.Falling)
                return false;

            // add jump sound effect

            //
            State = SantaState.Jumping;

            jumpVelocity = SANTA_START_JUMP_POWER;

            return true;

        }
        /// <summary>
        /// Cancels Santa's jump, reducing his jump velocity.
        /// </summary>
        /// <returns>True if the jump is successfully cancelled, false otherwise.</returns>

        public bool CancelJump()
        {

            if (State != SantaState.Jumping || (startPosY - position.Y) < MIN_JUMP_HEIGHT)
                return false;

            jumpVelocity = jumpVelocity < SANTA_CANCEL_JUMPL_SPEED ? SANTA_CANCEL_JUMPL_SPEED : 0;

            return true;

        }
        /// <summary>
        /// Initiates Santa's fall, increasing his fall velocity.
        /// </summary>
        /// <returns>True if the fall is initiated, false otherwise.</returns>

        public bool Fall()
        {
            if (State != SantaState.Falling && State != SantaState.Jumping)
                return false;

            State = SantaState.Falling;

            fallVelocity = FALL_VELOCITY;

            return true;
        }
        /// <summary>
        /// Decreases Santa's life by one.
        /// </summary>

        public void LoseLife()
        {
            if (lives > 0)
            { 
                lives--;
            }

        }
        /// <summary>
        /// Retrieves the number of lives Santa has left.
        /// </summary>
        /// <returns>The number of lives.</returns>

        public int getSantalife()
        {
            return lives;
        }

        /// <summary>
        /// Gets the bounding rectangle of Santa for collision detection.
        /// </summary>
        /// <returns>The bounding rectangle.</returns>

        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)((tex.Width / 5) * imgScale), (int)(tex.Height * imgScale));
        }
    }
}
