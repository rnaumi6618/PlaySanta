/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Manages collision detection and responses between different game objects such as Santa, Grinches, Snowballs, and Gifts.
    /// This class is responsible for updating the game state based on collisions.
    /// </summary>
    public class CollisionManagement : GameComponent
    {
        public ScoreClash scoreClash;
        public LifeDisplay lifeDisplay;
        private Santa santa;
        private List<Grinch> grinchs;
        private List<SnowBall> snowBalls;
        private List<Gift> gifts;
        private SoundEffect punchSound;
        private SoundEffect giftSound;
        private SoundEffect ballHitSound;
        //private int currentScore;


        /// <summary>
        /// Initializes a new instance of the CollisionManagement class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>
        /// <param name="santa">The Santa object in the game.</param>
        /// <param name="grinchs">List of Grinch objects in the game.</param>
        /// <param name="snowBalls">List of SnowBall objects in the game.</param>
        /// <param name="gifts">List of Gift objects in the game.</param>
        /// <param name="scoreClash">Score display component.</param>
        /// <param name="lifeDisplay">Life display component.</param>
        /// <param name="punchSound">Sound effect for collisions with Grinches.</param>
        /// <param name="giftSound">Sound effect for collecting Gifts.</param>
        /// <param name="ballHitSound">Sound effect for collisions with Snowballs.</param>

        public CollisionManagement(Game game, Santa santa, List<Grinch> grinchs, List<SnowBall> snowBalls, List<Gift> gifts,ScoreClash scoreClash,LifeDisplay lifeDisplay,SoundEffect punchSound,
        SoundEffect giftSound,SoundEffect ballHitSound) : base(game)
        {
            this.santa = santa;
            this.grinchs = grinchs;
            this.snowBalls = snowBalls;
            this.gifts = gifts;
            this.scoreClash = scoreClash;
            this.lifeDisplay = lifeDisplay;
            this.punchSound = punchSound;
            this.giftSound = giftSound;
            this.ballHitSound = ballHitSound;
        }

        /// <summary>
        /// Updates the collision management, checking and handling collisions between game objects.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            Rectangle santaRect = santa.getBound();

            foreach (Grinch grinch in grinchs)
            {

                if (grinch.HasCollided) continue; // Skip if already collided

                Rectangle grinchRect = grinch.getBound();
                if (grinchRect.Intersects(santaRect))
                {
                    if(punchSound != null)
                    {
                        punchSound.Play();
                    }

                    scoreClash.Score -= 5;
                    grinch.Collide(); // Handle the collision
                    santa.LoseLife(); // Santa loses a life on collision
                    grinch.Visible = false;
                }
            }

           

            foreach (SnowBall snowBall in snowBalls)
            {

                Rectangle snowBallRect = snowBall.getBound();
                if (!snowBall.Visible) continue;

                if (snowBallRect.Intersects(santaRect))
                {  
                    if(ballHitSound != null)
                    {
                        ballHitSound.Play();
                    }
              
                    scoreClash.Score -= 3;
                    snowBall.Visible = false;
                }
            }
            foreach (Gift gift in gifts)
            {
                Rectangle giftRect = gift.getBound();
                if (!gift.Visible) continue;
                if (giftRect.Intersects(santaRect))
                {
                    if(giftSound != null)
                    {
                        giftSound.Play();
                    }
                  
                    scoreClash.Score += 10;
                    gift.Visible = false;
                    // santa.IsAlive = false;
                }
            }

            base.Update(gameTime);
        }
    }
}
