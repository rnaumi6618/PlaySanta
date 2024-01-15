/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using AllInOne;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;


namespace AllInOne
{
    /// <summary>
    /// Represents the primary action scene in the game.
    /// This scene handles the gameplay, including managing Santa, obstacles like Grinches and Snowballs, and collectibles like Gifts.
    /// </summary>
    public class ActionScene2 : GameScene
    {
        private ScoreClash scoreClash;
        private Santa santa;
        private SnowBall snowBall;
        private Background backgroundManager;
        private LifeDisplay lifeDisplay;
        private List<Grinch> grinchList=new List<Grinch>();
        private List<SnowBall> snowBallList = new List<SnowBall>();
		private List<Gift> giftList = new List<Gift>();
        private Grinch grinch1, grinch2, grinch3, grinch4, grinch;

		Texture2D texGift;
        private CollisionManagement collision;
        GameHandler gameHandler;
        private const int PLAYER_DEFAULT_POS_X = 100;
        private const int PLAYER_DEFAULT_POS_Y = 30;
        private const float PLAYER_RESIZING_RATIO = 0.4f;
        private const float OBSTACLE_RESIZING_RATIO = 0.3f;
        private const float GIFT_RESIZING_RATIO = 0.5f;
        private const int NUM_OF_STAGE = 100;
       // private const int SCORE_BOARD_POS_X = WINDOW_WIDTH - 130;
        private const int SCORE_BOARD_POS_Y = 10;
        private bool isActionOver = false;



        Texture2D texBackground;
        Texture2D texSanta;
        Texture2D texGrinch;
        Texture2D texSnowBall;
        Texture2D texScore;
        SpriteFont regularFont;
        SpriteFont hilightFont;
        SpriteFont scoreFont;
        SpriteFont labelFont;
        SoundEffect punchSound;
        SoundEffect giftSound;
        SoundEffect ballHitSound;

        Vector2 stage;
        public int CurrentScore { get; set; }

        public bool IsActionOver { get => isActionOver; set => isActionOver = value; }
        /// <summary>
        /// Initializes a new instance of the ActionScene1 class.
        /// </summary>
        /// <param name="game">Reference to the main game object.</param>

        public ActionScene2(Game game) : base(game)
        {
            gameHandler = (GameHandler)game;
            punchSound = game.Content.Load<SoundEffect>("sounds/punch");
            giftSound = game.Content.Load<SoundEffect>("sounds/gift");
            ballHitSound = game.Content.Load<SoundEffect>("sounds/ballDrop");
            // Background
            stage = new Vector2(gameHandler._graphics.PreferredBackBufferWidth, gameHandler._graphics.PreferredBackBufferHeight);
            texBackground = game.Content.Load<Texture2D>("images/snow1");
  
            //Santa
            texSanta = game.Content.Load<Texture2D>("images/santa2");

            //Score for Clash
            scoreFont = game.Content.Load<SpriteFont>("fonts/HilightFont");
            labelFont = game.Content.Load<SpriteFont>("fonts/HilightFont");

            //Flag if Game is over or not
            //isActionOver = false;

            //Grinch
            texGrinch = game.Content.Load<Texture2D>("images/g6");

            //Gift
            texGift = game.Content.Load<Texture2D>("images/gift1");
            
            //Snowball
            texSnowBall = game.Content.Load<Texture2D>("images/snowball");

            Initialize();
        }
        /// <summary>
        /// Registers game components such as Santa, obstacles, gifts, and the collision manager.
        /// </summary>
        public void registerComponent()
        {

            Rectangle srcRect = new Rectangle(0, 0, texBackground.Width, texBackground.Height);
            Vector2 pos = new Vector2(0, stage.Y - srcRect.Height);
            Vector2 speedBackground = new Vector2(1, 0);
            backgroundManager = new Background(gameHandler, gameHandler._spriteBatch, texBackground, pos, speedBackground, srcRect);
            this.Components.Add(backgroundManager);

            Vector2 position = new Vector2(Shared.stage.X / 6, Shared.stage.Y - texSanta.Height / 3 - PLAYER_DEFAULT_POS_Y);
            Vector2 speed = new Vector2(0, 0);
            santa = new Santa(gameHandler, gameHandler._spriteBatch, texSanta, position, speed, PLAYER_RESIZING_RATIO);
            this.Components.Add(santa);

         
            //Score for Clash
            SpriteFont scoreFont = gameHandler.Content.Load<SpriteFont>("fonts/HilightFont");
            SpriteFont labelFont = gameHandler.Content.Load<SpriteFont>("fonts/HilightFont");

            scoreClash = new ScoreClash(gameHandler, gameHandler._spriteBatch, scoreFont, labelFont);
            this.Components.Add(scoreClash);

            lifeDisplay = new LifeDisplay(gameHandler, gameHandler._spriteBatch, scoreFont, santa);
            this.Components.Add(lifeDisplay);


            //Grinch
            Vector2 speed1 = new Vector2(8, 0);

            // Calculate total width and positions
          
            int grinichPerStage = 2;
            // Calculate total width and positions
            float totalWidth = Shared.stage.X * NUM_OF_STAGE;
            //float distanceBetweenGrinches = totalWidth / NUM_OF_STAGE;
            float distanceBetweenGrinches = totalWidth / (grinichPerStage * NUM_OF_STAGE + 1);

            // Create and position Grinches
            for (int i = 0; i < NUM_OF_STAGE; i++)
            {
                // Calculate the X position for each Grinch
                // Position them in the middle of each stage
                float posX = santa.position.X + distanceBetweenGrinches * (i + 0.5f);

                // position the Grinches at the bottom of each stage
                float posY = Shared.stage.Y - texGrinch.Height/3; // Adjust Y position as needed

                // Create the Grinch object
                Grinch grinch = new Grinch(gameHandler, gameHandler._spriteBatch, texGrinch, new Vector2(posX, posY), speed1, OBSTACLE_RESIZING_RATIO, 10);

                // Add the Grinch to the scene
                this.Components.Add(grinch);
                grinchList.Add(grinch); //  keeping track of Grinches in a list
            }


            //Gift
            int giftsPerStage = 3;
            // Calculate total width and distance between gifts
            float distanceBetweenGifts = totalWidth / (giftsPerStage * NUM_OF_STAGE + 1);

            // Create and position gifts
            for (int i = 0; i < giftsPerStage * NUM_OF_STAGE; i++)
            {
                // Calculate the X position for each gift
                float posX = distanceBetweenGifts * (i + 1);

                // position the gifts at the bottom of each stage
                float posY = Shared.stage.Y - texGift.Height / 2;

                // Create the gift object 
                Gift gift = new Gift(gameHandler, gameHandler._spriteBatch, texGift, new Vector2(posX, posY), GIFT_RESIZING_RATIO, speed1);

                // Add the gift to the scene
                this.Components.Add(gift);
                giftList.Add(gift); //keeping track of gifts in a list
            }

    
            Vector2 upwardSpeed = new Vector2(0, -5); // Replace with desired upward speed.
            double snowballDelay = 4; // Initial delay for the first snowball.
            snowBall = new SnowBall(gameHandler, gameHandler._spriteBatch, texSnowBall, santa, upwardSpeed, stage, OBSTACLE_RESIZING_RATIO, snowballDelay);
            snowBallList.Add(snowBall);
            this.Components.Add(snowBall);

            if (gameHandler.IsMusicOn)
            {
                collision = new CollisionManagement(gameHandler, santa, grinchList, snowBallList, giftList, scoreClash, lifeDisplay, punchSound, giftSound, ballHitSound);
            }
            else
            {
                collision = new CollisionManagement(gameHandler, santa, grinchList, snowBallList, giftList, scoreClash, lifeDisplay, null, null, null);

            }
            this.Components.Add(collision);

        }
        /// <summary>
        /// Removes and disposes of all components from the scene.
        /// </summary>
        private void RemoveComponents()
        {

            if(backgroundManager!=null)
            {
                Components.Remove(backgroundManager);
                backgroundManager.Dispose(); //  resource cleanup
                backgroundManager = null;
            }


            if(scoreClash != null)
            {
                Components.Remove(scoreClash);
                scoreClash.Dispose(); //  resource cleanup
                scoreClash = null;
            }


            if (scoreClash != null)
            {
                Components.Remove(scoreClash);
                scoreClash.Dispose(); //  resource cleanup
                scoreClash = null;
            }

            if (santa!=null)
            {
                Components.Remove(santa);
                santa.Dispose(); //  resource cleanup
                santa = null;
            }

            //grindch
            foreach (Grinch grinch in grinchList) 
            { 
                if (grinch != null)
                {
                    Components.Remove(grinch);
                    grinch.Dispose(); //  resource cleanup
      
                }
            }
            grinchList.Clear();

            //gift
            foreach (Gift gift in giftList)
            {
                if (gift != null)
                {
                    Components.Remove(gift);
                    gift.Dispose(); //  resource cleanup
                   
                }
            }
            giftList.Clear();

            //snowBall
            foreach (SnowBall snowBall in snowBallList)
            {
                if (snowBall != null)
                {
                    Components.Remove(snowBall);
                    snowBall.Dispose(); //  resource cleanup
                }
            }
            snowBallList.Clear();

            if (lifeDisplay != null)
            {
                Components.Remove(lifeDisplay);
                lifeDisplay.Dispose(); //  resource cleanup
                lifeDisplay = null;
            }

            if (collision != null)
            {
                Components.Remove(collision);
                collision.Dispose(); //  resource cleanup
                collision = null;
            }

            this.Components.Clear();
        }
        /// <summary>
        /// Initializes the action scene, setting up components and game state.
        /// </summary>
        public override void Initialize()
        {

            RemoveComponents();

            registerComponent();

            //Flag if Game is over or not
            isActionOver = false;

            base.Initialize();
        }
        /// <summary>
        /// Updates the action scene, handling game state changes like Santa's life status.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
		{
			if(santa.getSantalife() == 0)
			{
			    isActionOver = true;
                CurrentScore = scoreClash.Score;
            }

            base.Update(gameTime);
        }
    }
}
