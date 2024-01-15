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
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents a level selection component in the game. 
    /// This component allows players to navigate through and select different game levels.
    /// </summary>
    public class LevelComponent : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;

        private List<string> menuItems;
        public int SelectedIndex { get; set; }
        private Vector2 position;
        private Color regularColor = Color.Black;
        private Color hilightColor = Color.Red;
        private KeyboardState oldState;
        private SoundEffect buttonPressSound;
        GameHandler gameHandler;

        /// <summary>
        /// Initializes a new instance of the LevelComponent class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="regularFont">Font for regular menu items.</param>
        /// <param name="hilightFont">Font for the highlighted menu item.</param>
        /// <param name="menus">Array of level item names.</param>
        /// <param name="buttonPressed">Sound effect to play when a level item is selected.</param>

        public LevelComponent(Game game, SpriteBatch sb,
            SpriteFont regularFont, SpriteFont hilightFont,
            string[] menus, SoundEffect buttonPressed) : base(game)
        {
            gameHandler = (GameHandler)game;
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            menuItems = menus.ToList();
            position = new Vector2(Shared.stage.X * 0.47f, Shared.stage.Y * 0.35f);
            if (buttonPressed != null)
            {
                this.buttonPressSound = buttonPressed;
            }

        }

        // <summary>
        /// Updates the level component, handling navigation and selection of level items.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            int previousIndex = SelectedIndex;

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                SelectedIndex = (SelectedIndex + 1) % menuItems.Count;
                if (SelectedIndex != previousIndex)
                {
                    if (buttonPressSound != null)
                    {
                        buttonPressSound.Play();
                    }

                }
            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                {
                    SelectedIndex = menuItems.Count - 1;
                }
                if (SelectedIndex != previousIndex)
                {
                    if (buttonPressSound != null)
                    {
                        buttonPressSound.Play();
                    }

                }
            }

            oldState = ks;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the level items on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPos = position;
            sb.Begin();

            // Create a single 1x1 pixel texture for drawing rectangles
            Texture2D rectTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });

            for (int i = 0; i < menuItems.Count; i++)
            {
                SpriteFont font = SelectedIndex == i ? hilightFont : regularFont;
                Vector2 size = font.MeasureString(menuItems[i]);

                // Padding for the rectangle
                int padding = 100;
                Rectangle bgRect = new Rectangle((int)tempPos.X - padding / 2, (int)tempPos.Y, (int)size.X + padding, (int)size.Y);

                // Draw the rectangle
                Color stringColor = SelectedIndex == i ? Color.LightSkyBlue : Color.Transparent;
                sb.Draw(rectTexture, bgRect, stringColor);

                // Draw the text
                sb.DrawString(font, menuItems[i], tempPos, SelectedIndex == i ? hilightColor : regularColor);
                //tempPos.Y += font.LineSpacing;
                tempPos.Y += gameHandler._graphics.PreferredBackBufferHeight / 4;
            }
            sb.End();
            rectTexture.Dispose(); // Dispose of the texture

            base.Draw(gameTime);
        }
    }
}
