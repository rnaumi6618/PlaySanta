/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/

using AllInOne;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents the start scene in the game, handling the initial menu and associated functionalities.
    /// </summary>
    public class StartScene : GameScene
    {
        private MenuComponent menu;
        private HelpScene helpScene;
        private Background startBack;
        private SoundEffect buttonPress;
        private SoundEffect backSound;
        SoundEffectInstance soundEffectInstance;
        SpriteFont regularFont;
        SpriteFont hilightFont;
        string[] menuItems = { "Start game", "Help", "High Score", "Credit", "Setting(Music)", "Quit" };

        /// <summary>
        /// Gets or sets the MenuComponent associated with the start scene.
        /// </summary>
        public MenuComponent Menu { get => menu; set => menu = value; }
        GameHandler gameHandelr;

        /// <summary>
        /// Initializes a new instance of the StartScene class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        public StartScene(Game game) : base(game)
        {
            gameHandelr = (GameHandler)game;

            // Load fonts and sounds
            regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            hilightFont = game.Content.Load<SpriteFont>("fonts/HilightFont");
            buttonPress = game.Content.Load<SoundEffect>("sounds/buttonClick1");
            backSound = game.Content.Load<SoundEffect>("sounds/alwaysPlay");

            // Initialize background
            Vector2 stage = new Vector2(gameHandelr._graphics.PreferredBackBufferWidth, gameHandelr._graphics.PreferredBackBufferHeight);
            Texture2D texBackground = game.Content.Load<Texture2D>("images/startSceneBack");
            Rectangle srcRect = new Rectangle(0, 0, texBackground.Width, texBackground.Height);
            Vector2 pos = new Vector2(0, stage.Y - srcRect.Height);
            Vector2 speedBackground = new Vector2(1, 0);
            startBack = new Background(game, gameHandelr._spriteBatch, texBackground, pos, speedBackground, srcRect);
            this.Components.Add(startBack);
        }

        /// <summary>
        /// Initializes the StartScene, setting up the menu and music settings.
        /// </summary>
        public override void Initialize()
        {
            // Dispose existing menu if any
            if (menu != null)
            {
                Components.Remove(menu);
                menu.Dispose(); // Clean up resources
                menu = null;
            }

            // Setup menu and sound based on music settings
            if (gameHandelr.IsMusicOn)
            {
                menu = new MenuComponent(gameHandelr, gameHandelr._spriteBatch, regularFont, hilightFont, menuItems, buttonPress);
                if (soundEffectInstance != null)
                {
                    soundEffectInstance.Play();
                }
                else
                {
                    soundEffectInstance = backSound.CreateInstance();
                    soundEffectInstance.IsLooped = true;
                    soundEffectInstance.Play();
                }
            }
            else
            {
                menu = new MenuComponent(gameHandelr, gameHandelr._spriteBatch, regularFont, hilightFont, menuItems, null);
                if (soundEffectInstance != null)
                {
                    soundEffectInstance.Stop();
                }
            }
            this.Components.Add(Menu);

            gameHandelr.SelectedLevel = LEVEL_NONE;
            base.Initialize();
        }
        public void notPlay()
        {
            soundEffectInstance.Stop();
        }
    }
}