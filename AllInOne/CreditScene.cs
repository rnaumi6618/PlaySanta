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
using static System.Net.Mime.MediaTypeNames;

namespace AllInOne
{
    /// <summary>
    /// Represents the credit scene in the game.
    /// This scene displays credits for the game, including the developer names and acknowledgments.
    /// </summary>
    internal class CreditScene : GameScene
    {
        
        private SpriteBatch sb;
        SpriteFont font;
        private Texture2D back;
        private Texture2D cuteSanta;
        private Texture2D board;
        private float space = 3f;
        private float x = 2.3f;
        private float y = 5f;
        GameHandler g;

        /// <summary>
        /// Initializes a new instance of the CreditScene class.
        /// </summary>
        /// <param name="game">The main game object reference.</param>

        public CreditScene(Game game) : base(game)
        {
            g = (GameHandler)game;
            sb = g._spriteBatch;
            font = g.Content.Load<SpriteFont>("fonts/HilightFont");
            back = game.Content.Load<Texture2D>("images/snow1");
            cuteSanta = game.Content.Load<Texture2D>("images/cuteSanta2");
            board = game.Content.Load<Texture2D>("images/help");
        }
        /// <summary>
        /// Draws the credit scene, including credits text and related images.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            string gameTitle = "Game Title:\n  PlaySanta\n";
            string producers = "\nDeveloper:\n -Eunheui Jo\n -Rafia Naumi\n";
            string thanks = "\nSpecial Thanks:\n-Professor Sabbir Ahmed";


            Vector2 position = new Vector2(Shared.stage.X /x, Shared.stage.Y/y);
            Rectangle position1 = new Rectangle(0, 0, g._graphics.PreferredBackBufferWidth, g._graphics.PreferredBackBufferHeight);
            Vector2 position2 = new Vector2(Shared.stage.X /space, 0);
            Vector2 position3 = new Vector2(Shared.stage.X /space, Shared.stage.Y / 2);
            sb.Draw(back, position1, Color.White);
            sb.Draw(board, position2, Color.White);
            sb.Draw(cuteSanta, position3, Color.White);

            sb.DrawString(font, gameTitle + producers + thanks, position, Color.Purple);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
