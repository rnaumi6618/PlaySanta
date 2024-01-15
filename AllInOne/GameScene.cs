/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// An abstract base class for different game scenes.
    /// This class provides common functionalities like showing, hiding, and updating components within a game scene.
    /// </summary>
    public abstract class GameScene : DrawableGameComponent
    {
        public const int LEVEL_NONE = 0;
        public const int LEVEL_INTERMEDIATE = 1;
        public const int LEVEL_PRIMARY = 2;
        public List<GameComponent> Components { get; set; }
        
        /// <summary>
        /// Hides the game scene, making it invisible and inactive.
        /// </summary>

        public virtual void hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }
        /// <summary>
        /// Shows the game scene, making it visible and active.
        /// </summary>
        public virtual void show()
        {
            this.Enabled = true;
            this.Visible = true;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the GameScene class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>

        protected GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            hide();
        }
        /// <summary>
        /// Updates the game scene, including all its components.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in Components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Draws the game scene and all its drawable components.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent)
                {
                    DrawableGameComponent comp = (DrawableGameComponent)item;
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }
       
    }
}
