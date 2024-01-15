/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AllInOne
{
    /// <summary>
    /// Represents the level selection scene in the game. 
    /// This scene allows players to choose from different levels and navigate to the selected level.
    /// </summary>

    internal class LevelSelectionScene : GameScene
    {
        private const int BUTTON_IMG_WIDTH = 2074;
        private const int BUTTON_IMG_HEIGHT = 701;

        const float SCALE = 0.2f; // IMAGE SCALE
        private Texture2D backgroundTexture;
        private Texture2D backgroundTexture2;
        private Texture2D texLevelButton;
        private Texture2D texSelectLevelTitle;

        SpriteFont regularFont;
        SpriteFont hilightFont;
        private SoundEffect buttonPress;
        private SpriteBatch sb;
        private LevelComponent menu;
        string[] menuItems = { "Level 1", "Level 2" };

        private int selectedLevelIndex = 0;
        Vector2 stage;
        KeyboardState _previousKeyboardState;
        GameHandler gameHandelr;

        public LevelComponent Menu { get => menu; set => menu = value; }
        private Rectangle backgroundRect2;
        private Rectangle backgroundRect1;
        private Rectangle titleRect;

        private Vector2 LevelOneButtonPosition = new Vector2(Shared.stage.X / 2 - (BUTTON_IMG_WIDTH * SCALE / 2), Shared.stage.Y / 4);
        private Vector2 LevelTwoButtonPosition = new Vector2(Shared.stage.X / 2 - (BUTTON_IMG_WIDTH * SCALE / 2), Shared.stage.Y / 4 + (int)(BUTTON_IMG_HEIGHT * SCALE * 1.5));

        private Rectangle LevelOneButtonBounds
         => new Rectangle((LevelOneButtonPosition).ToPoint(), new Point((int)(BUTTON_IMG_WIDTH * SCALE), (int)(BUTTON_IMG_HEIGHT * SCALE)));
        private Rectangle LevelTwoButtonBounds
       => new Rectangle((LevelTwoButtonPosition).ToPoint(), new Point((int)(BUTTON_IMG_WIDTH * SCALE), (int)(BUTTON_IMG_HEIGHT * SCALE)));

        /// <summary>
        /// Initializes a new instance of the LevelSelectionScene class.
        /// </summary>
        /// <param name="game">The main game object reference.</param>

        public LevelSelectionScene(Game game) : base(game)
        {
            gameHandelr = (GameHandler)game;
            sb = this.gameHandelr._spriteBatch;
            hilightFont = this.gameHandelr.Content.Load<SpriteFont>("fonts/TitleFont");

            // Background
            stage = new Vector2(this.gameHandelr._graphics.PreferredBackBufferWidth, this.gameHandelr._graphics.PreferredBackBufferHeight);
            backgroundTexture = game.Content.Load<Texture2D>("images/snow1");
            backgroundRect1 = new Rectangle(0, 0, this.gameHandelr._graphics.PreferredBackBufferWidth, this.gameHandelr._graphics.PreferredBackBufferHeight);

            float wScale = 0.4f;
            float hScale = 0.8f;
            int imgWidth = (int)(this.gameHandelr._graphics.PreferredBackBufferWidth * wScale);
            int imgHeight = (int)(this.gameHandelr._graphics.PreferredBackBufferHeight * hScale);
            backgroundTexture2 = game.Content.Load<Texture2D>("images/level-select-board");
            backgroundRect2 = new Rectangle((int)(stage.X / 2 - imgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), imgWidth, imgHeight);

            int titleImgWidth = (int)(this.gameHandelr._graphics.PreferredBackBufferWidth * 0.2);
            int titleImgHeight = (int)(this.gameHandelr._graphics.PreferredBackBufferHeight * 0.1);
            texSelectLevelTitle = game.Content.Load<Texture2D>("images/level-select");
            titleRect = new Rectangle((int)(stage.X / 2 - titleImgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), titleImgWidth, titleImgHeight);

            texLevelButton = game.Content.Load<Texture2D>("images/input_textbox");

            regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            hilightFont = game.Content.Load<SpriteFont>("fonts/HilightFont");
            buttonPress = game.Content.Load<SoundEffect>("sounds/buttonClick1");

            menu = new LevelComponent(gameHandelr, gameHandelr._spriteBatch, regularFont, hilightFont, menuItems, buttonPress);
            this.Components.Add(menu);
        }

        /// <summary>
        /// Draws the level selection scene, including the background, level buttons, and titles.
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
            sb.Draw(texLevelButton, LevelOneButtonBounds, Color.White);

            Vector2 LevelOneText = new Vector2(LevelOneButtonPosition.X + BUTTON_IMG_WIDTH * SCALE / 3, LevelOneButtonPosition.Y + 30);
            // sb.DrawString(hilightFont, menuItems[0], LevelOneText, Color.Black);

            sb.Draw(texLevelButton, LevelTwoButtonBounds, Color.White);

            Vector2 LevelTwoText = new Vector2(LevelTwoButtonPosition.X + BUTTON_IMG_WIDTH * SCALE / 3, LevelTwoButtonPosition.Y + 30);
            //sb.DrawString(hilightFont, menuItems[1], LevelTwoText, Color.Black);

            sb.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the state of the level selection scene, handling user input for selecting and navigating levels.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            KeyboardState keyboardState = Keyboard.GetState();
            bool isEscKeyPressed = keyboardState.IsKeyDown(Keys.Escape);
            bool wasEscKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Escape);

            bool isEnterKeyPressed = keyboardState.IsKeyDown(Keys.Enter);
            bool wasEnterKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Enter);

            int selectedIndex = menu.SelectedIndex;

            if ((selectedIndex == 0 && keyboardState.IsKeyDown(Keys.Enter)) ||
                (LevelOneButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed))
            {

                Debug.Print("Level One Button Pressed");

                gameHandelr.ActionSceneLevel1.show();
                gameHandelr.SelectedLevel = LEVEL_INTERMEDIATE;
                this.hide();

            }


            if (wasEscKeyPressed && !isEscKeyPressed)
            {
                Debug.Print("Escape Key Released");
                gameHandelr.StartScene.show();
                this.hide();
            }

            if ((selectedIndex == 1 && keyboardState.IsKeyDown(Keys.Enter)) ||
                (LevelTwoButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed))
            {
                Debug.Print("Level Two Button Pressed");

                gameHandelr.ActionSceneLevel2.show();
                gameHandelr.SelectedLevel = LEVEL_PRIMARY;
                this.hide();
            }

            _previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// Starts the game level selected by the player.
        /// </summary>

        private void StartSelectedLevel()
        {
            // Implement logic to start the selected level
            string selectedLevel = menuItems[selectedLevelIndex];
            Console.WriteLine($"Starting {selectedLevel}");
            // Add your logic to transition to the selected level
        }
    }
}
