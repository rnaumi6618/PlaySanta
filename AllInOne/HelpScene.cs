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
using MonoGame.Framework.Utilities.Deflate;
using SharpDX.Direct3D9;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace AllInOne
{
    /// <summary>
    /// Represents the help scene in the game. 
    /// This scene displays instructions, gameplay tips, and control information to the player.
    /// </summary>
    public class HelpScene : GameScene
    {
        private Texture2D tex;
        private Texture2D back;
        private Texture2D title;
        private SpriteBatch sb;
        private SoundEffect helpSound;
        private SoundEffectInstance InstanceHelpSound;
        SpriteFont regularFont;
        GameHandler g;
        string help;
        private const float space = 2.5f;

        /// <summary>
        /// Initializes a new instance of the HelpScene class.
        /// </summary>
        /// <param name="game">The main game object reference.</param>

        public HelpScene(Game game) : base(game)
        {
            g = (GameHandler)game;
            sb = g._spriteBatch;
            regularFont = g.Content.Load<SpriteFont>("fonts/RegularFont");
            tex = game.Content.Load<Texture2D>("images/santaTara");
            back = game.Content.Load<Texture2D>("images/level_select_button");
            title = game.Content.Load<Texture2D>("images/instruction");
            helpSound = game.Content.Load<SoundEffect>("sounds/help");
            
            help = "Instructions:\n" +
            "Objective:\n" +
            "  - Help Santa collect as many gifts as possible while avoiding the Grinches and snow.\n" +
            "  - The more gifts you collect, the higher your score!\n" +
            " \n" +
            "Controls: \n" +
            "  - Use the right and left Arrow Keys to move Santa around.\n" +
            "  - Press up arrow key to jump over Grinches.\n" +
            "  - click mouse in ok or cancel button after game ends\n"+
            "  - Run through the gift to collect point" +
            " \n" +
            "Gameplay:\n" +
            "  - Avoid Grinches: Each time you collide with a Grinch, \n" +
            "    you lose one life and 5 points.\n" +
            "  - Lose all 3 lives, and it's game over!\n" +
            "  - Collect Gifts: Each gift you collect increases your score by 10 points.\n" +
            "  - SnowBalls: Watch out for snowballs! Getting hit by a snowball will reduce 3 points.\n" +
            " \n" +
            "Tips:\n" +
            "  - Stay alert! Grinches and snowballs can appear suddenly.\n" +
            "  - Timing is key: Time your jumps carefully to avoid Grinches and collect gifts.\n" +
            "Press Esc to return to the main menu.";
        }
        /// <summary>
        /// Stops the help scene sound.
        /// </summary>

        public void stop()
        {
            InstanceHelpSound?.Stop();
        }

        /// <summary>
        /// Starts playing the help scene sound if music is enabled.
        /// </summary>

        public void start()
        {
            if (g.IsMusicOn)
            {
                InstanceHelpSound = helpSound.CreateInstance();
                InstanceHelpSound.IsLooped = true;
                InstanceHelpSound.Play();
            }
        }


        /// <summary>
        /// Draws the help scene, including instructions, images, and background.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(Shared.stage.X / space, 20);
            Vector2 position1 = new Vector2(0, 0);
            Rectangle position2 = new Rectangle(0, 0, g._graphics.PreferredBackBufferWidth, g._graphics.PreferredBackBufferHeight);
            sb.Begin();
            sb.Draw(back, position2, Color.White);
            sb.Draw(title, position2, Color.White);
            sb.Draw(tex, position1, Color.White);
            sb.DrawString(regularFont, help, position, Color.Purple);
            sb.End();

            base.Draw(gameTime);
        }

       
    }
}
