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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{ /// <summary>
  /// Represents the music settings scene in the game. 
  /// This scene allows players to toggle music on or off and navigate between other menu options.
  /// </summary>
    internal class MusicSettingScene : GameScene
    {
        private const int SOUND_BUTTON_IMG_WIDTH = 701;
        private const int SOUND_BUTTON_IMG_HEIGHT = 701;
        private const int OK_BUTTON_IMG_WIDTH = 701;
        private const int OK_BUTTON_IMG_HEIGHT = 701;
        private const int CANCEL_BUTTON_IMG_WIDTH = 701;
        private const int CANCEL_BUTTON_IMG_HEIGHT = 701;
        const float SCALE = 0.2f; // IMAGE SCALE
        private Texture2D backgroundTexture;
        private Texture2D backgroundTexture2;
        private Texture2D texMusicOn;
        private Texture2D texMusicOff;
        private Texture2D texSelectLevelTitle;
        private Texture2D texAcceptButton;
        private Texture2D texCancleButton;

        private SpriteBatch sb;
        private SpriteFont hilightFont;
        private List<string> levels = new List<string> { "Level 1", "Level 2" }; // Add more levels as needed
        private int selectedLevelIndex = 0;
        private bool isMusicOn;
        private bool initMusicStat;
        Vector2 stage;
        KeyboardState _previousKeyboardState;
        GameHandler gameHandler;
        int clickDownTime = 200;
        bool isClickOndown = false;
        DateTime lastClickTime = DateTime.MinValue;

        private Rectangle backgroundRect2;
        private Rectangle backgroundRect1;
        private Rectangle titleRect;

        private Vector2 SoundButtonPosition = new Vector2(Shared.stage.X / 2 - (SOUND_BUTTON_IMG_WIDTH * SCALE / 2), Shared.stage.Y / 4);

        private Vector2 okButtonPosition = new Vector2(Shared.stage.X / 2 - OK_BUTTON_IMG_WIDTH * SCALE * 2, Shared.stage.Y / 2);
        private Vector2 cancelButtonPosition = new Vector2(Shared.stage.X / 2 + OK_BUTTON_IMG_WIDTH * SCALE, Shared.stage.Y / 2);

        private Rectangle SoundButtonBounds
       => new Rectangle((SoundButtonPosition).ToPoint(), new Point((int)(SOUND_BUTTON_IMG_WIDTH * SCALE), (int)(SOUND_BUTTON_IMG_HEIGHT * SCALE)));

        private Rectangle OkButtonBounds
         => new Rectangle((okButtonPosition).ToPoint(), new Point((int)(OK_BUTTON_IMG_WIDTH * SCALE), (int)(OK_BUTTON_IMG_HEIGHT * SCALE)));
        private Rectangle CancelButtonBounds
        => new Rectangle((cancelButtonPosition).ToPoint(), new Point((int)(CANCEL_BUTTON_IMG_WIDTH * SCALE), (int)(CANCEL_BUTTON_IMG_HEIGHT * SCALE)));


        /// <summary>
        /// Initializes a new instance of the MusicSettingScene class.
        /// </summary>
        /// <param name="game">The main game object reference.</param>

        public MusicSettingScene(Game game) : base(game)
        {
            this.gameHandler = (GameHandler)game;
            sb = this.gameHandler._spriteBatch;
            hilightFont = this.gameHandler.Content.Load<SpriteFont>("fonts/TitleFont");

            // Background
            stage = new Vector2(this.gameHandler._graphics.PreferredBackBufferWidth, this.gameHandler._graphics.PreferredBackBufferHeight);
            backgroundTexture = game.Content.Load<Texture2D>("images/snow1");
            backgroundRect1 = new Rectangle(0, 0, this.gameHandler._graphics.PreferredBackBufferWidth, this.gameHandler._graphics.PreferredBackBufferHeight);

            float wScale = 0.4f;
            float hScale = 0.8f;
            int imgWidth = (int)(this.gameHandler._graphics.PreferredBackBufferWidth * wScale);
            int imgHeight = (int)(this.gameHandler._graphics.PreferredBackBufferHeight * hScale);
            backgroundTexture2 = game.Content.Load<Texture2D>("images/level-select-board");
            backgroundRect2 = new Rectangle((int)(stage.X / 2 - imgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), imgWidth, imgHeight);

            int titleImgWidth = (int)(this.gameHandler._graphics.PreferredBackBufferWidth * 0.2);
            int titleImgHeight = (int)(this.gameHandler._graphics.PreferredBackBufferHeight * 0.2);
            texSelectLevelTitle = game.Content.Load<Texture2D>("images/setting");
            titleRect = new Rectangle((int)(stage.X / 2 - titleImgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), titleImgWidth, titleImgHeight);

            texMusicOn = game.Content.Load<Texture2D>("images/music_on");
            texMusicOff = game.Content.Load<Texture2D>("images/music_off");
            texAcceptButton = game.Content.Load<Texture2D>("images/input_accept");
            texCancleButton = game.Content.Load<Texture2D>("images/input_cancel");

        }

        // <summary>
        /// Draws the music settings scene, including the background, buttons, and other UI elements.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            // Draw background
            sb.Draw(backgroundTexture, backgroundRect1, Color.White);
            sb.Draw(backgroundTexture2, backgroundRect2, Color.White);
            sb.Draw(texSelectLevelTitle, titleRect, Color.White);

            // Draw level buttons
            if (isMusicOn)
            {
                sb.Draw(texMusicOn, SoundButtonBounds, Color.White);
            }
            else
            {
                sb.Draw(texMusicOff, SoundButtonBounds, Color.White);
            }

            sb.Draw(texAcceptButton, okButtonPosition,
             new Rectangle(0, 0, OK_BUTTON_IMG_WIDTH, OK_BUTTON_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);
            sb.Draw(texCancleButton, cancelButtonPosition,
                     new Rectangle(0, 0, CANCEL_BUTTON_IMG_WIDTH, CANCEL_BUTTON_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);

            sb.End();

            base.Draw(gameTime);
        }


        /// <summary>
        /// Updates the state of the music settings scene, handling user input for toggling music and navigating the menu.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {


            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            bool isEscKeyPressed = keyboardState.IsKeyDown(Keys.Escape);
            bool wasEscKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Escape);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!isClickOndown)
                {
                    // Process the click

                    if (SoundButtonBounds.Contains(mouseState.Position))
                    {

                        Debug.Print("Sound Button Pressed");
                        isMusicOn = !isMusicOn;
                        gameHandler.IsMusicOn = isMusicOn;
                        gameHandler.IsSettingChanged = true;
                        isClickOndown = true;
                        lastClickTime = DateTime.Now;
                    }
                    if (OkButtonBounds.Contains(mouseState.Position))
                    {

                        Debug.Print("Ok Button Pressed");

                        gameHandler.IsMusicOn = isMusicOn;
                        gameHandler.StartScene.show();
                        this.hide();
                        // Set the cooldown
                        isClickOndown = true;
                        lastClickTime = DateTime.Now;

                    }

                    if (CancelButtonBounds.Contains(mouseState.Position))
                    {
                        Debug.Print("Cancel Button Pressed");
                        gameHandler.IsMusicOn = initMusicStat;
                        gameHandler.StartScene.show();
                        this.hide();
                        // Set the cooldown
                        isClickOndown = true;
                        lastClickTime = DateTime.Now;
                    }

                }
            }
            else
            {
                Debug.Print((DateTime.Now - lastClickTime).TotalMilliseconds.ToString());
                // Check if the cooldown period has elapsed
                if (isClickOndown && (DateTime.Now - lastClickTime).TotalMilliseconds >= clickDownTime)
                {
                    isClickOndown = false;
                }
            }

            if (wasEscKeyPressed && !isEscKeyPressed)
            {
                Debug.Print("Escape Key Released");
                gameHandler.StartScene.show();
                this.hide();
            }



            _previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }
        /// <summary>
        /// Initializes the state of the music settings scene, setting initial values for music settings.
        /// </summary>

        public override void Initialize()
        {
            if (gameHandler.IsMusicOn)
            {
                isMusicOn = true;
            }
            else
            {
                isMusicOn = false;
            }

            initMusicStat = isMusicOn;
            base.Initialize();
        }
        /// <summary>
        /// Starts the game level selected by the player.
        /// </summary>

        private void StartSelectedLevel()
        {
            // Implement logic to start the selected level
            string selectedLevel = levels[selectedLevelIndex];
            Console.WriteLine($"Starting {selectedLevel}");
            // Add your logic to transition to the selected level
        }
    }
}
