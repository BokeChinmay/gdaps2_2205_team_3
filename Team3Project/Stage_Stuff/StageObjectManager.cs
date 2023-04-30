using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Enemy_Stuff;
using Team3Project.Player_Stuff;

//Name: StageObjectManager
//Purpose: Manager class that loads levels and sets them up on screen. Also sets every entity's side blocked fields.

namespace Team3Project.Stage_Stuff
{
    // enum for use with assigning tile types on level generation
    enum TileTypes
    {
        empty,
        blocked,
        blocked2
    }

    internal class StageObjectManager
    {
        // General field declarations
        private StreamReader reader;
        private Random rng;

        // Organizing fields
        private List<StageObject> obstructiveStageObjects;
        private List<Rectangle> emptyTiles;
        private Dictionary<string, TileTypes[,]> levelLayouts;
        private TileTypes[,] currentLayout;
        private int currentLevel;
        
        // Buffer fields
        private Texture2D bufferTexture;
        private VisualBarrier leftBuffer;
        private VisualBarrier rightBuffer;

        // Tile fields
        private Texture2D blockedTileTopTexture;
        private Texture2D blockedTileBottomTexture;
        private Texture2D blockedTileBasicTexture;
        private Texture2D emptyTileTexture;

        // Fields for the other bounds of the play area
        private Texture2D backWallTexture;
        private VisualBarrier backWall;
        private HiddenStageObject bottomBounds;

        // Fields for interactive stage elements
        private Texture2D elevatorClosed;
        private Texture2D elevatorOpen;
        private Elevator elevator;

        // Fields for UI elements
        private SpriteFont font;
        private Texture2D fullHP;
        private Texture2D emptyHP;
        private Texture2D brokenHP;
        private Texture2D fullLife;
        private Texture2D emptyLife;
        private HealthDisplay healthDisplay;
        private Texture2D levelLabel;
        private Texture2D scoreLabel;
        private Texture2D healthLabel;
        private Texture2D livesLabel;
        private ScoreDisplay levelDisplay;
        private ScoreDisplay scoreDisplay;
        private Texture2D numberSheet;
        private Rectangle[] uiNumbers;

        // Get-only property for obstructive stage objects
        public List<StageObject> ObstructiveStageObjects
        {
            get { return obstructiveStageObjects; }
        }

        // Get-only property for the list of empty tiles
        public List<Rectangle> EmptyTiles
        { 
            get { return emptyTiles; } 
        }

        // Get-only property for the elevator
        public Elevator Elevator
        {
            get { return elevator; }
        }

        // Property for the current level
        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        // Default constructor
        public StageObjectManager()
        {
            rng = new Random();
            
            obstructiveStageObjects = new List<StageObject>();
            emptyTiles = new List<Rectangle>();
            levelLayouts = new Dictionary<string, TileTypes[,]>();
            currentLevel = 1;

            bottomBounds = new HiddenStageObject(1500, 200, 0, 901);
            obstructiveStageObjects.Add(bottomBounds);

            levelDisplay = new ScoreDisplay(true);
            scoreDisplay = new ScoreDisplay(false);

            uiNumbers = new Rectangle[10];
            uiNumbers[0] = new Rectangle(4, 4, 20, 24);
            uiNumbers[1] = new Rectangle(32, 4, 20, 24);
            uiNumbers[2] = new Rectangle(60, 4, 20, 24);
            uiNumbers[3] = new Rectangle(88, 4, 20, 24);
            uiNumbers[4] = new Rectangle(116, 4, 20, 24);
            uiNumbers[5] = new Rectangle(4, 36, 20, 24);
            uiNumbers[6] = new Rectangle(32, 36, 20, 24);
            uiNumbers[7] = new Rectangle(60, 36, 20, 24);
            uiNumbers[8] = new Rectangle(88, 36, 20, 24);
            uiNumbers[9] = new Rectangle(116, 36, 20, 24);
        }

        /// <summary>
        /// Loading content for all of the stage objects
        /// </summary>
        /// <param name="content"></param>
        /// <param name="_graphics"></param>
        public void LoadContent(ContentManager content, GraphicsDeviceManager _graphics)
        {
            // Loading and initializing buffers
            // Buffers are 180 pixels wide and 900 pixels tall
            bufferTexture = content.Load<Texture2D>("Buffer");

            leftBuffer = new VisualBarrier
                (0, bufferTexture, false);
            obstructiveStageObjects.Add(leftBuffer);

            rightBuffer = new VisualBarrier
                (_graphics.PreferredBackBufferWidth - bufferTexture.Width, bufferTexture, false);
            obstructiveStageObjects.Add(rightBuffer);

            // Loading and initializing the back wall
            // This wall is 1140 pixels wide and 100 pixels tall
            backWallTexture = content.Load<Texture2D>("BackWall");

            backWall = new VisualBarrier
                (leftBuffer.RightEdge, backWallTexture, true);
            obstructiveStageObjects.Add(backWall);

            // Loading the and initializaing the elevator
            elevatorOpen = content.Load<Texture2D>("Elevator_Open");
            elevatorClosed = content.Load<Texture2D>("Elevator_Closed");

            elevator = new Elevator(elevatorOpen, elevatorClosed, this);

            // Loading and initializing the UI
            font = content.Load<SpriteFont>("MenuFont");
            fullHP = content.Load<Texture2D>("Full_HP_v2");
            emptyHP = content.Load<Texture2D>("Empty_HP_v2");
            brokenHP = content.Load<Texture2D>("Broken_HP_v2");
            fullLife = content.Load<Texture2D>("Full_Life_v2");
            emptyLife = content.Load<Texture2D>("Empty_Life_v2");

            healthLabel = content.Load<Texture2D>("UI_HP");
            livesLabel = content.Load<Texture2D>("UI_Lives");
            healthDisplay = new HealthDisplay(font, fullHP, emptyHP, brokenHP, fullLife, 
                emptyLife, healthLabel, livesLabel);

            levelLabel = content.Load<Texture2D>("UI_Level");
            scoreLabel = content.Load<Texture2D>("UI_Score");
            numberSheet = content.Load<Texture2D>("UI_Numbers");

            // Loading the blocked tile textures
            blockedTileTopTexture = content.Load<Texture2D>("Blocked_Tile_Top");
            blockedTileBottomTexture = content.Load<Texture2D>("Blocked_Tile_Bottom(v2)");
            blockedTileBasicTexture = content.Load<Texture2D>("BlockedTile");

            // Loading the empty tile texture
            emptyTileTexture = content.Load<Texture2D>("emptyTile");

            // The tiles in the tile map will be 114 pixels wide by 100 pixels tall,
            // for a grid that is 10 tiles wide and 8 tiles tall
            try
            {
                reader = new StreamReader("..\\..\\..\\..\\data_files\\LevelLayouts.txt");

                while (reader.Peek() != -1)
                {
                    string layoutName = reader.ReadLine();
                    TileTypes[,] layoutTiles = new TileTypes[10, 8];

                    for (int i = 0; i < 8; i++)
                    {
                        string[] currentLine = reader.ReadLine().Split('-');

                        for (int j = 0; j < 10; j++)
                        {
                            if (currentLine[j] == "0")
                            {
                                layoutTiles[j, i] = TileTypes.empty;
                            }
                            else if (currentLine[j] == "X")
                            {
                                layoutTiles[j, i] = TileTypes.blocked;
                            }
                            else if (currentLine[j] == "V")
                            {
                                layoutTiles[j, i] = TileTypes.blocked2;
                            }
                        }
                    }
                    levelLayouts.Add(layoutName, layoutTiles);
                }

                reader.Close();
            }
            catch
            {
                levelLayouts.Add("Empty Layout", new TileTypes[10, 8]);
            }

            elevator.NewLevel += GenerateLevel;
        }

        /// <summary>
        /// Drawing all of the stage objects
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw1(SpriteBatch _spriteBatch)
        {
            foreach (Rectangle emptyTile in emptyTiles)
            {
                _spriteBatch.Draw(emptyTileTexture, emptyTile, Color.White);
            }

            foreach (StageObject s in obstructiveStageObjects)
            {
                if (s is BlockedTile)
                {
                    BlockedTile currentTile = (BlockedTile)s;
                    
                    currentTile.DrawBottom(_spriteBatch, SpriteEffects.None);
                }
                else
                {
                    s.Draw(_spriteBatch, SpriteEffects.None);
                }
            }

            elevator.Draw(_spriteBatch, SpriteEffects.None);

            healthDisplay.Draw(_spriteBatch, SpriteEffects.None);
            levelDisplay.Draw(_spriteBatch, SpriteEffects.None, this, levelLabel);
            scoreDisplay.Draw(_spriteBatch, SpriteEffects.None, this, scoreLabel);
        }

        /// <summary>
        /// Draws a top layer for blocked tiles over everything else
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw2(SpriteBatch _spriteBatch)
        {
            foreach(StageObject s in obstructiveStageObjects)
            {
                if (s is BlockedTile)
                {
                    BlockedTile currentTile = (BlockedTile)s;

                    if (!currentTile.Basic)
                    {
                        currentTile.DrawTop(_spriteBatch, SpriteEffects.None);
                    }
                    else
                    {
                        currentTile.Draw(_spriteBatch, SpriteEffects.None);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a number at a specified location
        /// </summary>
        /// <param name="number"> the number to be drawn </param>
        /// <param name="location"> where to draw the number </param>
        /// <param name="spriteBatch"></param>
        public void DrawUINumber(int number, Vector2 location, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(numberSheet, location, uiNumbers[number], Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Checks if an entity is blocked in any direction by a stage object
        /// </summary>
        /// <param name="entity"> the entity whose surroundings are being checked </param>
        public void CheckBlockedSides(Entity entity)
        {
            entity.RightBlocked = false;
            entity.LeftBlocked = false;
            entity.TopBlocked = false;
            entity.BottomBlocked = false;
            
            foreach (StageObject obj in obstructiveStageObjects)
            {
                // Checking if the entity is blocked from above
                if ((obj.IsObstruction &&
                    entity.Collision.Top <= obj.BottomEdge + 5 &&
                    entity.Collision.Bottom >= obj.BottomEdge + entity.Collision.Height &&
                    (entity.Collision.Right <= obj.RightEdge &&
                    entity.Collision.Right >= obj.LeftEdge ||
                    entity.Collision.Left <= obj.RightEdge &&
                    entity.Collision.Left >= obj.LeftEdge))
                    || entity.TopBlocked)
                {
                    entity.TopBlocked = true;
                }
                else
                {
                    entity.TopBlocked = false;
                }

                // Checking if the entity is blocked from below
                if ((obj.IsObstruction &&
                    entity.Collision.Bottom >= obj.TopEdge - 5 &&
                    entity.Collision.Top <= obj.TopEdge - entity.Collision.Height &&
                    (entity.Collision.Right <= obj.RightEdge &&
                    entity.Collision.Right >= obj.LeftEdge ||
                    entity.Collision.Left <= obj.RightEdge &&
                    entity.Collision.Left >= obj.LeftEdge))
                    || entity.BottomBlocked)
                {
                    entity.BottomBlocked = true;
                }
                else
                {
                    entity.BottomBlocked = false;
                }

                // Checking if the entity is blocked from the left
                if ((obj.IsObstruction &&
                    entity.Collision.Left <= obj.RightEdge + 5 &&
                    entity.Collision.Right >= obj.RightEdge + entity.Collision.Width &&
                    (entity.Collision.Bottom <= obj.BottomEdge &&
                    entity.Collision.Bottom >= obj.TopEdge ||
                    entity.Collision.Top <= obj.BottomEdge &&
                    entity.Collision.Top >= obj.TopEdge))
                    || entity.LeftBlocked)
                {
                    entity.LeftBlocked = true;
                }
                else
                {
                    entity.LeftBlocked = false;
                }

                // Checking if the entity is blocked from the right
                if ((obj.IsObstruction &&
                    entity.Collision.Right >= obj.LeftEdge - 5 &&
                    entity.Collision.Left <= obj.LeftEdge + entity.Collision.Width &&
                    (entity.Collision.Bottom <= obj.BottomEdge &&
                    entity.Collision.Bottom >= obj.TopEdge ||
                    entity.Collision.Top <= obj.BottomEdge &&
                    entity.Collision.Top >= obj.TopEdge))
                    || entity.RightBlocked)
                {
                    entity.RightBlocked = true;
                }
                else
                {
                    entity.RightBlocked = false;
                }
            }
        }

        /// <summary>
        /// Updates per frame
        /// </summary>
        public void Update(List<Enemy> enemies, Player player)
        {
            //Update blocked sides for all enemies
            foreach (Enemy e in enemies) 
            { 
                CheckBlockedSides(e);
            }
            //Check if projectiles intersect obstructive stage objects
            foreach (Projectile p in LevelManager.ProjectileList)
            {
                foreach(StageObject so in ObstructiveStageObjects)
                {
                    if (p.Collision.Intersects(so.Dimensions))
                    {
                        p.Active = false;
                    }
                }
            }

            //Update elevator based on whether or not there are still enemies on screen
            if (!LevelManager.EnemiesPresent)
            {
                elevator.IsOpen = true;
            }
            else
            {
                elevator.IsOpen = false;
            }

            //Update player's blocked sides
            CheckBlockedSides(player);

            //Update elevator's collision with the player
            elevator.PlayerEnters(player);

            //Update health and score displays
            healthDisplay.Health = player.Health;
            healthDisplay.Lives = player.Lives;
            levelDisplay.Update(player);
            scoreDisplay.Update(player);
        }

        /// <summary>
        /// Generates a new level, selecting a layout from those pulled from the file
        /// </summary>
        public void GenerateLevel(int level)
        {
            currentLevel = level;

            // Clearing all of the current tiles to make way for new ones
            for(int i = 0; i < obstructiveStageObjects.Count; i++)
            {
                if (obstructiveStageObjects[i] is BlockedTile)
                {
                    obstructiveStageObjects.Remove(obstructiveStageObjects[i]);
                    i--;
                }
            }

            emptyTiles.Clear();

            // Choosing a new layout
            int layoutChoice;

            if (currentLevel % 10 != 0)
            {
                layoutChoice = rng.Next(1, 5);
            }
            else
            {
                layoutChoice = 0;
            }

            // Selecting the layout corresponding to the choice made
            if (layoutChoice == 0) 
            {
                currentLayout = levelLayouts["Empty Layout"];
            }
            else if (layoutChoice == 1)
            {
                currentLayout = levelLayouts["Columns"];
            }
            else if (layoutChoice == 2)
            {
                currentLayout = levelLayouts["Alleys"];
            }
            else if (layoutChoice == 3)
            {
                currentLayout = levelLayouts["Lanes"];
            }
            else if (layoutChoice == 4)
            {
                currentLayout = levelLayouts["Scatter"];
            }

            // Creating tiles and adding them to the list of stage objects
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (currentLayout[j, i] == TileTypes.blocked)
                    {
                        obstructiveStageObjects.Add(new BlockedTile((180 + 114 * j), (100 + 100 * i),
                            blockedTileTopTexture, blockedTileBottomTexture, blockedTileBasicTexture, false));
                    }
                    else if (currentLayout[j, i] == TileTypes.blocked2)
                    {
                        obstructiveStageObjects.Add(new BlockedTile((180 + 114 * j), (100 + 100 * i),
                            blockedTileTopTexture, blockedTileBottomTexture, blockedTileBasicTexture, true));
                    }
                    else if ((currentLayout[j, i] == TileTypes.empty))
                    {
                        emptyTiles.Add(new Rectangle((180 + 114 * j), (100 + 100 * i), 114, 100));
                    }
                }
            }
        }
    }
}
