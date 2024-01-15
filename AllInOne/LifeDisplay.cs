/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents a display component for showing Santa's lives in the game.
    /// It displays the current level and the number of lives remaining.
    /// </summary>
    public class LifeDisplay : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Santa santa;
        GameHandler gameHandler;

        /// <summary>
        /// Initializes a new instance of the LifeDisplay class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        /// <param name="font">The font used for displaying text.</param>
        /// <param name="santa">Reference to the Santa object to access life information.</param>

        public LifeDisplay(Game game, SpriteBatch spriteBatch, SpriteFont font, Santa santa)
            : base(game)
        {
            gameHandler = (GameHandler)game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.santa = santa;
        }

        /// <summary>
        /// Draws the current level and number of lives on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            string curLevel = "Level- " + gameHandler.SelectedLevel.ToString() + "  ";
            string lifeText = $"    Life- {santa.lives}";
            Vector2 position = new Vector2(0,0); // Adjust the position as needed

            spriteBatch.DrawString(font, curLevel+lifeText, position, Color.Purple);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
