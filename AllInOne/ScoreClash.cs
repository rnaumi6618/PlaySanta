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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Represents the score display component in the game.
    /// This component is responsible for drawing the current score on the screen.
    /// </summary>
    public class ScoreClash : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont scoreFont; // Font used for displaying the score value.
        private SpriteFont labelFont; // Font used for displaying the score label.

        /// <summary>
        /// Gets or sets the current score to be displayed.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Initializes a new instance of the ScoreClash class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        /// <param name="scoreFont">The font used for drawing the score value.</param>
        /// <param name="labelFont">The font used for drawing the score label.</param>
        public ScoreClash(Game game, SpriteBatch spriteBatch, SpriteFont scoreFont, SpriteFont labelFont)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.scoreFont = scoreFont;
            this.labelFont = labelFont;
        }

        /// <summary>
        /// Draws the score information on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Prepare the score label and value for drawing
            string scoreLabel = "Score:";
            string scoreValue = $"{Score}";
            Vector2 labelSize = labelFont.MeasureString(scoreLabel);
            Vector2 scoreSize = scoreFont.MeasureString(scoreValue);

            // Position the score in the top right corner of the screen
            Vector2 position = new Vector2(Game.GraphicsDevice.Viewport.Width - scoreSize.X - 10, 0);
            // Position the label to the left of the score value
            Vector2 labelPosition = new Vector2(position.X - labelSize.X - 5, 0);

            // Draw the score label and value
            spriteBatch.DrawString(labelFont, scoreLabel, labelPosition, Color.Purple);
            spriteBatch.DrawString(scoreFont, scoreValue, position, Color.Purple);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}