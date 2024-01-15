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
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// The main game handler class that manages different scenes and the overall game state.
    /// </summary>
    public class GameHandler : Game
    {
        public enum Level
        {
            None,
            Intermediate,
            Primary
        }


        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Texture2D tex;

        //declare all scenes
        private StartScene startScene;
        private HelpScene helpScene;
        private ActionScene1 actionSceneLevel1;
        private ActionScene2 actionSceneLevel2;
        private CreditScene creditScene;
        private ScoreScene scoreScene;
        private GameEndScene gameEndScene;
        private LevelSelectionScene levelSelectionScene;
        private MusicSettingScene musicSettingScene;
        private Level selectedLevel;
        private bool isMusicOn;
        private bool isSettingChanged;
        private int clickDownTime = 200;
        private bool isStartGameClickOndown = false;
        private DateTime lastClickTime = DateTime.MinValue;
        public StartScene StartScene { get => startScene; set => startScene = value; }
        public HelpScene HelpScene { get => helpScene; set => helpScene = value; }
        public ActionScene1 ActionSceneLevel1 { get => actionSceneLevel1; set => actionSceneLevel1 = value; }
        public ActionScene2 ActionSceneLevel2 { get => actionSceneLevel2; set => actionSceneLevel2 = value; }
        internal CreditScene CreditScene { get => creditScene; set => creditScene = value; }
        internal ScoreScene ScoreScene { get => scoreScene; set => scoreScene = value; }
        internal GameEndScene InputNameScene { get => gameEndScene; set => gameEndScene = value; }
        internal LevelSelectionScene LevelSelectionScene { get => levelSelectionScene; set => levelSelectionScene = value; }
        internal MusicSettingScene MusicSettingScene { get => musicSettingScene; set => musicSettingScene = value; }

        public int SelectedLevel { get => (int)selectedLevel; set => selectedLevel = (Level)value; }
        public bool IsMusicOn { get => isMusicOn; set => isMusicOn = value; }
        public bool IsSettingChanged { get => isSettingChanged; set => isSettingChanged = value; }


        /// <summary>
        /// Initializes a new instance of the GameHandler class, setting up the game environment.
        /// </summary>
        public GameHandler()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 700;
            selectedLevel = Level.None;
            IsMusicOn = true;
            IsSettingChanged = false;
        }
        /// <summary>
        /// Initializes various components and scenes of the game.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }
        /// <summary>
        /// Loads content for the game, such as textures, fonts, and sounds.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //instantiate all scenes here
            startScene = new StartScene(this);
            this.Components.Add(startScene);

            levelSelectionScene = new LevelSelectionScene(this);
            this.Components.Add(levelSelectionScene);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

            actionSceneLevel1 = new ActionScene1(this);
            this.Components.Add(actionSceneLevel1);

            actionSceneLevel2 = new ActionScene2(this);
            this.Components.Add(actionSceneLevel2);

            creditScene = new CreditScene(this);
            this.Components.Add(creditScene);

            scoreScene = new ScoreScene(this);
            this.Components.Add(scoreScene);


            gameEndScene = new GameEndScene(this);
            this.Components.Add(gameEndScene);

            musicSettingScene = new MusicSettingScene(this);
            this.Components.Add(musicSettingScene);

            //make startscene active
            startScene.show();
            selectedLevel = Level.None;
        }

        /// <summary>
        /// Updates the game state, including the active scene and handling scene transitions.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>


        protected override void Update(GameTime gameTime)
        {

            // Select scene on the menu
            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();
            if (startScene.Enabled)
            {
                    selectedIndex = startScene.Menu.SelectedIndex;
                    if ((selectedIndex == 0 && ks.IsKeyDown(Keys.Enter)))
                    {
                        if (!isStartGameClickOndown)
                        {
                           lastClickTime = DateTime.Now;
                           isStartGameClickOndown = true;
                        }
       
                    }
                    else if(isStartGameClickOndown)
                    {
                        Debug.Print((DateTime.Now - lastClickTime).TotalMilliseconds.ToString());
                        // Check if the cooldown period has elapsed
                        if (isStartGameClickOndown && (DateTime.Now - lastClickTime).TotalMilliseconds >= clickDownTime)
                        {
                            isStartGameClickOndown = false;
                            startScene.hide();
                            levelSelectionScene.show();
                        }
                    }
                    else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter))
                    {
                        startScene.hide();
                        startScene.notPlay();
                        helpScene.show();
                        helpScene.start();
                    }
                    else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter))
                    {
                        startScene.hide();
                        scoreScene.show();

                    }
                    else if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter))
                    {
                        startScene.hide();
                        creditScene.show();
                    }
                    else if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter))
                    {
                        startScene.hide();
                        musicSettingScene.show();
                    }
                    else if (selectedIndex == 5 && ks.IsKeyDown(Keys.Enter))
                    {
                        Exit();
                    }
                    
            

        }

            if (levelSelectionScene.Enabled)
            {
                if (selectedLevel == Level.Intermediate)
                {
                    levelSelectionScene.hide();
                    actionSceneLevel1.show();

                }
                else if (selectedLevel == Level.Primary)
                {
                    levelSelectionScene.hide();
                    actionSceneLevel2.show();
                }
            }

            if (musicSettingScene.Enabled)
            {
                if (IsSettingChanged)
                {
                    //musicSettingScene.hide();
                    startScene.Initialize();
                    IsSettingChanged = false;
                }

            }

            if (actionSceneLevel1.Enabled)
            {

                MouseState ms = Mouse.GetState();

                if (ks.IsKeyDown(Keys.Escape))
                {
                    actionSceneLevel1.hide();
                    startScene.show();
                    selectedLevel = Level.None;
                }

                //when Santa is not alive, popup the input name Scene
                if (actionSceneLevel1.IsActionOver)
                {
                    actionSceneLevel1.Enabled = false;
                    gameEndScene.show();
                }
                else
                {
                    gameEndScene.hide();
                }
            }
            else if (actionSceneLevel2.Enabled)
            {
                MouseState ms = Mouse.GetState();

                if (ks.IsKeyDown(Keys.Escape))
                {
                    actionSceneLevel2.hide();
                    startScene.show();
                    selectedLevel = Level.None;
                }

                //when Santa is not alive, popup the input name Scene
                if (actionSceneLevel2.IsActionOver)
                {
                    actionSceneLevel2.Enabled = false;
                    gameEndScene.show();
                }
                else
                {
                    gameEndScene.hide();
                }
            }


            if (helpScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    helpScene.stop();
                    helpScene.hide();
                    startScene.show();
                }
            }
            if (creditScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    creditScene.hide();
                    startScene.show();
                }
            }
            if (scoreScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    scoreScene.hide();
                    startScene.show();
                }
            }

            //same way other scenes
            Debug.Print(this.Components.Count.ToString());
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the current active scene of the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}