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
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AllInOne
{
    /// <summary>
    /// Represents the game end scene in the game.
    /// This scene is displayed at the end of the game, allowing players to save their score and decide to continue playing or exit.
    /// </summary>

    internal class GameEndScene : GameScene
    {
        private const int TXT_BOX_IMG_WIDTH = 2074;
        private const int TXT_BOX_IMG_HEIGHT = 701;
        private const int OK_BUTTON_IMG_WIDTH = 701;
        private const int OK_BUTTON_IMG_HEIGHT = 701;
        private const int CANCEL_BUTTON_IMG_WIDTH = 701;
        private const int CANCEL_BUTTON_IMG_HEIGHT = 701;
        private const int USER_NAME_LENGTH_LIMIT = 20;
        private const int BACK_DEFAULT_SPRITE_POS_X = 0;
        private const int BACK_DEFAULT_SPRITE_POS_Y = 0;

        private const String SCORE_DB_PATH = "GameScoreDb.txt";
        private Texture2D texBackground;
        private Texture2D texScore;
        private Texture2D texInputBox;
        private Texture2D texAcceptButton;
        private Texture2D texCancleButton;
        private SpriteBatch sb;
        readonly SpriteFont hilightFont;
        readonly SpriteFont RegularFont;
        private bool txtBoxHasFocus = false;
        private bool isOkClicked = false;
        Vector2 stage;
        GameHandler game;
        const float SCALE = 0.23f; // IMAGE SCALE
        KeyboardState _previousKeyboardState;

        private string textBoxDisplayCharacters = "";
        private bool isRegisterTextInput = false;
        private bool isClieckSaveButton = false;
        private bool isSaved = false;

        private int clickDownTime = 200;
        private bool isStartClickOndown = false;
        private DateTime lastClickTime = DateTime.MinValue;

        private Vector2 textBoxPosition = new Vector2(Shared.stage.X / 3, Shared.stage.Y * 2 / 4);
        private Vector2 okButtonPosition = new Vector2(Shared.stage.X / 3, Shared.stage.Y * 3 / 4);
        private Vector2 cancelButtonPosition = new Vector2(Shared.stage.X / 3 + OK_BUTTON_IMG_WIDTH * SCALE * 2, Shared.stage.Y * 3 / 4);
        private int curLevel;
        private int curScore;

        private Rectangle TextBoxBounds
           => new Rectangle(
               (textBoxPosition).ToPoint(), new Point((int)(TXT_BOX_IMG_WIDTH * SCALE), (int)(TXT_BOX_IMG_HEIGHT * SCALE)));
        private Rectangle OkButtonBounds
            => new Rectangle((okButtonPosition).ToPoint(), new Point((int)(OK_BUTTON_IMG_WIDTH * SCALE), (int)(OK_BUTTON_IMG_HEIGHT * SCALE)));
        private Rectangle CancelButtonBounds
       => new Rectangle((cancelButtonPosition).ToPoint(), new Point((int)(CANCEL_BUTTON_IMG_WIDTH * SCALE), (int)(CANCEL_BUTTON_IMG_HEIGHT * SCALE)));

        /// <summary>
        /// Initializes a new instance of the GameEndScene class.
        /// </summary>
        /// <param name="game">The main game object reference.</param>

        public GameEndScene(Game game) : base(game)
        {
            this.game = (GameHandler)game;
            sb = this.game._spriteBatch;
            texBackground = game.Content.Load<Texture2D>("images/input_background");
            texScore = game.Content.Load<Texture2D>("images/input_textbox");
            texInputBox = game.Content.Load<Texture2D>("images/input_textbox");
            texAcceptButton = game.Content.Load<Texture2D>("images/input_accept");
            texCancleButton = game.Content.Load<Texture2D>("images/input_cancel");
            hilightFont = this.game.Content.Load<SpriteFont>("fonts/HilightFont");
            textBoxDisplayCharacters = "";
        }

        /// <summary>
        /// Draws the game end scene, including score display, input box, and buttons.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            Vector2 textPosition = new Vector2(textBoxPosition.X + (textBoxPosition.X / 12), textBoxPosition.Y + (textBoxPosition.Y / 6));
            Vector2 scoreBoxPosition = new Vector2(textBoxPosition.X, (textBoxPosition.Y - (int)(OK_BUTTON_IMG_HEIGHT * SCALE * 1.5)));
            Vector2 textScorePosition = new Vector2(scoreBoxPosition.X + (scoreBoxPosition.X / 12), scoreBoxPosition.Y + (scoreBoxPosition.Y / 6));

            sb.Begin();

            if (!isClieckSaveButton)
            {
                sb.Draw(texScore, scoreBoxPosition,
               new Rectangle(0, 0, TXT_BOX_IMG_WIDTH, TXT_BOX_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);

                string scoreMsg = "\n Score: " + curScore.ToString() + "\n Save your record!!";
                sb.DrawString(hilightFont, scoreMsg, textScorePosition, Color.DarkBlue);

                sb.Draw(texInputBox, textBoxPosition,
                    new Rectangle(0, 0, TXT_BOX_IMG_WIDTH, TXT_BOX_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);

                if (textBoxDisplayCharacters.Length <= 0)
                {

                    if (isOkClicked)
                    {
                        sb.DrawString(hilightFont, "Please Enter your Name", textPosition, Color.Red);

                    }
                    else
                    {
                        sb.DrawString(hilightFont, "Please Enter your Name", textPosition, Color.Gray);
                    }
                }
                else
                {
                    sb.DrawString(hilightFont, textBoxDisplayCharacters, textPosition, Color.Black);
                }

            }
            else
            {
                Vector2 noticeMssgPosition = new Vector2(scoreBoxPosition.X , scoreBoxPosition.Y );

                string saveStatus = "";
                if(isSaved)
                {
                    saveStatus = "Your record has been saved successfully.\n";
                }
                string completeMsg = "Would you like to continue playing the game?";
                sb.DrawString(hilightFont, saveStatus+completeMsg, noticeMssgPosition, Color.DarkBlue);
            }


            sb.Draw(texAcceptButton, okButtonPosition,
                new Rectangle(0, 0, OK_BUTTON_IMG_WIDTH, OK_BUTTON_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);
            sb.Draw(texCancleButton, cancelButtonPosition,
                     new Rectangle(0, 0, CANCEL_BUTTON_IMG_WIDTH, CANCEL_BUTTON_IMG_HEIGHT), Color.White, 0f, Vector2.Zero, SCALE, SpriteEffects.None, 0f);
          
            sb.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the game end scene, handling user input for text entry and button interactions.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            bool isKeyPressed = keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up);
            bool wasKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Space) || _previousKeyboardState.IsKeyDown(Keys.Up);
            bool isEscKeyPressed = keyboardState.IsKeyDown(Keys.Escape);
            bool wasEscKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Escape);


            if(!isStartClickOndown)
            {

                if (OkButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    isStartClickOndown = true;
                    lastClickTime = DateTime.Now;
                    Debug.Print("Ok Button Pressed");
                    if (isClieckSaveButton)
                    {

                        if ((int)game.SelectedLevel == LEVEL_INTERMEDIATE)
                        {
                            game.ActionSceneLevel1.hide();
                            game.SelectedLevel = LEVEL_NONE;
                            game.StartScene.show();


                        }
                        else if ((int)game.SelectedLevel == LEVEL_PRIMARY)
                        {

                            game.ActionSceneLevel2.hide();
                            game.SelectedLevel = LEVEL_NONE;
                            game.StartScene.show();


                        }

                        this.hide();

                    }
                    else
                    {

                        // Save the file name and score to a file or perform any other necessary action
                        if (textBoxDisplayCharacters.Length <= 0)
                        {
                            isOkClicked = true;
                        }
                        else
                        {
                            SaveToFile(textBoxDisplayCharacters, curScore);
                            isClieckSaveButton = true;
                
                        }

                    }

                }

                if (wasEscKeyPressed && !isEscKeyPressed)
                {
                    isStartClickOndown = true;
                    lastClickTime = DateTime.Now;
                    if (isClieckSaveButton)
                    {
                        game.Exit();

                    }
                    else
                    {
                        isClieckSaveButton = true;
                    }
                }

                if (CancelButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    isStartClickOndown = true;
                    lastClickTime = DateTime.Now;
                    Debug.Print("Cancel Button Pressed");

                    if (isClieckSaveButton)
                    {
                       game.Exit();

                    }
                    else
                    {
                        isClieckSaveButton = true;
                    }

                }

            }
            else
            {
                // Check if the cooldown period has elapsed
                if (isStartClickOndown && (DateTime.Now - lastClickTime).TotalMilliseconds >= clickDownTime)
                {
                    isStartClickOndown = false;
                }
            }



          
            _previousKeyboardState = keyboardState;

            if (txtBoxHasFocus)
            {
                RegisterFocusedButtonForTextInput(OnInput);
                txtBoxHasFocus = !txtBoxHasFocus;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Saves the player's score and name to a file.
        /// </summary>
        /// <param name="userName">The name of the player.</param>
        /// <param name="score">The score achieved by the player.</param>

        public void SaveToFile(string userName, double score)
        {
            try
            {
                // Combine the file name with the current directory to get the full path
                string filePath = Path.Combine(Environment.CurrentDirectory, SCORE_DB_PATH);

                // Open or create the file for writing
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    // Write the score to the file
                    writer.WriteLine($"{userName} {score}");
                }

                Console.WriteLine($"File saved successfully: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
            isSaved = true;
        }

        /// <summary>
        /// Registers a method for handling text input when a specific UI element is focused.
        /// </summary>
        /// <param name="method">The method to handle text input events.</param>

        public void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            isRegisterTextInput = true;
            game.Window.TextInput += method;
        }

        /// <summary>
        /// Unregisters a method from handling text input when a specific UI element loses focus.
        /// </summary>
        /// <param name="method">The method that was handling text input events.</param>

        public void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            isRegisterTextInput = false;
            game.Window.TextInput -= method;
        }
        /// <summary>
        /// Handles input from the user, including text entry and backspace functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Provides data for the text input event.</param>

        public void OnInput(object sender, TextInputEventArgs e)
        {
            var k = e.Key;
            var c = e.Character;

            if (k == Keys.Back && textBoxDisplayCharacters.Length > 0)
            {
                // Remove the last character when the backspace key is pressed
                textBoxDisplayCharacters = textBoxDisplayCharacters.Substring(0, textBoxDisplayCharacters.Length - 1);
            }
            else if (!char.IsControl(c))
            {
                // Append the character to the existing text (ignore control characters)
                if (USER_NAME_LENGTH_LIMIT > textBoxDisplayCharacters.Length)
                {
                    textBoxDisplayCharacters += c;
                }

            }

            Debug.Print(textBoxDisplayCharacters);
        }

        /// <summary>
        /// Loads content for the game end scene.
        /// </summary>

        protected override void LoadContent()
        {
            base.LoadContent();
        }
        /// <summary>
        /// Hides the game end scene.
        /// </summary>
        public override void hide()
        {
            txtBoxHasFocus = false;

            base.hide();
        }
        /// <summary>
        /// Shows the game end scene and sets the focus on the text box.
        /// </summary>
        public override void show()
        {
            txtBoxHasFocus = true;
            base.show();
        }
        /// <summary>
        /// Initializes the game end scene, resetting text input and score information.
        /// </summary>
        
        public override void Initialize()
        {
            if (isRegisterTextInput)
            {
                UnRegisterFocusedButtonForTextInput(OnInput);
                textBoxDisplayCharacters = "";
            }

            if ((int)game.SelectedLevel == LEVEL_INTERMEDIATE)
            {
                curScore = game.ActionSceneLevel1.CurrentScore;

            }
            else if ((int)game.SelectedLevel == LEVEL_PRIMARY)
            {
                curScore = game.ActionSceneLevel2.CurrentScore;
            }

            isClieckSaveButton = false;
            isSaved = false;
            isStartClickOndown = false;

            base.Initialize();
        }
    }
}
