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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents a menu component in the game. 
    /// This component handles the display and interaction of a menu, including navigation and selection.
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {

        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;

        private List<string> menuItems;
        public int SelectedIndex { get; set; }
        private Vector2 position;
        private Color regularColor = Color.White;
        private Color hilightColor = Color.Red;
        private KeyboardState oldState;
        private SoundEffect buttonPressSound;
        /// <summary>
        /// Initializes a new instance of the MenuComponent class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        /// <param name="regularFont">Font for regular menu items.</param>
        /// <param name="hilightFont">Font for the highlighted menu item.</param>
        /// <param name="menus">Array of menu item names.</param>
        /// <param name="buttonPressed">Sound effect to play when a menu item is selected.</param>

        public MenuComponent(Game game, SpriteBatch sb,
            SpriteFont regularFont, SpriteFont hilightFont,
            string[] menus, SoundEffect buttonPressed) : base(game)
        {
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            menuItems = menus.ToList();
            position = new Vector2(Shared.stage.X / 3, Shared.stage.Y / 2);
            if (buttonPressed != null)
            {
                this.buttonPressSound = buttonPressed;
            }

        }

        /// <summary>
        /// Updates the menu component, handling navigation and selection of menu items.
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
        /// Draws the menu items on the screen.
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
                int padding = 10;
                Rectangle bgRect = new Rectangle((int)tempPos.X - padding / 2, (int)tempPos.Y, (int)size.X + padding, (int)size.Y);

                // Draw the rectangle
                Color stringColor = SelectedIndex == i ? Color.LightSkyBlue : Color.Transparent;
                sb.Draw(rectTexture, bgRect, stringColor);

                // Draw the text
                sb.DrawString(font, menuItems[i], tempPos, SelectedIndex == i ? hilightColor : regularColor);
                tempPos.Y += font.LineSpacing;
            }

            sb.End();
            rectTexture.Dispose(); // Dispose of the texture

            base.Draw(gameTime);
        }
    }
}
