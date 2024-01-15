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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace AllInOne
{
    /// <summary>
    /// Represents the score display scene in the game, managing the rendering and loading of score data.
    /// </summary>
    internal class ScoreScene : GameScene
    {
        private Texture2D texBackground1;
        private Texture2D texBackground2;
        private Texture2D texTitle;
        private SpriteBatch sb;
        private SpriteFont hilightFont;
        private List<(string Name, double Score)> scores = new List<(string, double)>();
        Texture2D scoreSceneBackground;
        Rectangle backgroundSrcRect;
        Rectangle backgroundRect1;
        Rectangle backgroundRect2;
        Rectangle titleRect;
        Vector2 stage;
        private const String SCORE_DB_PATH = "GameScoreDb.txt";
        private const int MAX_RANK = 10;

        /// <summary>
        /// Initializes a new instance of the ScoreScene class.
        /// </summary>
        /// <param name="game">The main game object.</param>
        public ScoreScene(Game game) : base(game)
        {
            GameHandler g = (GameHandler)game;
            sb = g._spriteBatch;
            hilightFont = g.Content.Load<SpriteFont>("fonts/HilightFont");
            scoreSceneBackground = game.Content.Load<Texture2D>("images/snowball");

            // Initialize stage and background textures
            stage = new Vector2(g._graphics.PreferredBackBufferWidth, g._graphics.PreferredBackBufferHeight);
            texBackground1 = game.Content.Load<Texture2D>("images/snow1");
            backgroundRect1 = new Rectangle(0, 0, g._graphics.PreferredBackBufferWidth, g._graphics.PreferredBackBufferHeight);

            float wScale = 0.4f;
            float hScale = 0.8f;
            int imgWidth = (int)(g._graphics.PreferredBackBufferWidth * wScale);
            int imgHeight = (int)(g._graphics.PreferredBackBufferHeight * hScale);
            texBackground2 = game.Content.Load<Texture2D>("images/input_background");
            backgroundRect2 = new Rectangle((int)(stage.X / 2 - imgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), imgWidth, imgHeight);

            int titleImgWidth = (int)(g._graphics.PreferredBackBufferWidth * 0.2);
            int titleImgHeight = (int)(g._graphics.PreferredBackBufferHeight * 0.1);
            texTitle = game.Content.Load<Texture2D>("images/hi-score");
            titleRect = new Rectangle((int)(stage.X / 2 - titleImgWidth / 2), (int)(stage.Y / 2 - imgHeight / 2), titleImgWidth, titleImgHeight);

            // Load scores from file when the scene is created
            LoadFromFile(SCORE_DB_PATH);
        }

        /// <summary>
        /// Initializes the scene by clearing previous scores and loading new ones.
        /// </summary>
        public override void Initialize()
        {
            scores.Clear();
            LoadFromFile(SCORE_DB_PATH);
            base.Initialize();
        }

        /// <summary>
        /// Draws the score scene, including the background and the list of top scores.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            // Draw backgrounds and title
            sb.Draw(texBackground1, backgroundRect1, Color.White);
            sb.Draw(texBackground2, backgroundRect2, Color.White);
            sb.Draw(texTitle, titleRect, Color.White);

            // Display the top 10 scores
            Vector2 position = new Vector2(backgroundRect2.X + backgroundRect2.Width / 4, backgroundRect2.Y + backgroundRect2.Height / 6);
            for (int i = 0; i < scores.Count; i++)
            {
                if (i == MAX_RANK)
                {
                    break;
                }
                var (name, score) = scores[i];
                string scoreText = $"{i + 1}. {name} - {score}";
                sb.DrawString(hilightFont, scoreText, position + new Vector2(0, i * 30), Color.Black);
            }

            sb.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Loads the scores from a file.
        /// </summary>
        /// <param name="fileName">The name of the file containing the scores.</param>
        public void LoadFromFile(string fileName)
        {
            try
            {
                // Get the full path of the file
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string fileContent = reader.ReadToEnd();
                        ParseScores(fileContent);
                    }
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses the content of the score file and populates the scores list.
        /// </summary>
        /// <param name="content">The content of the score file.</param>
        private void ParseScores(string content)
        {
            string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && double.TryParse(parts[1], out double score))
                {
                    scores.Add((parts[0], score));
                }
            }

            scores = scores.OrderByDescending(s => s.Score).ToList();
        }
    }
}