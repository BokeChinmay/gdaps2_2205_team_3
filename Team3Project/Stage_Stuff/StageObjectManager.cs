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

namespace Team3Project.Stage_Stuff
{
    internal class StageObjectManager
    {
        // enum for use with assigning tile types on level generation
        enum TileTypes
        {
            empty,
            blocked
        }

        // General field declarations
        private StreamReader reader;
        private Random rng;

        // Organizing fields
        private List<StageObject> obstructiveStageObjects;
        private Dictionary<string, TileTypes[,]> levelLayouts;
        private TileTypes[,] currentLayout;
        
        // Buffer fields
        private Texture2D bufferTexture;
        private VisualBarrier leftBuffer;
        private VisualBarrier rightBuffer;

        // Tile fields
        private Texture2D blockedTileTexture;

        // Fields for the other bounds of the play area
        private Texture2D backWallTexture;
        private VisualBarrier backWall;
        private HiddenStageObject bottomBounds;

        // Fields for interactive stage elements
        private Texture2D elevatorClosed;
        private Texture2D elevatorOpen;
        private Elevator elevator;

        // Get-only property for obstructive stage objects
        public List<StageObject> ObstructiveStageObjects
        {
            get { return obstructiveStageObjects; }
        }

        // Get-only property for the elevator
        public Elevator Elevator
        {
            get { return elevator; }
        }

        // Default constructor
        public StageObjectManager()
        {
            rng = new Random();
            
            obstructiveStageObjects = new List<StageObject>();
            levelLayouts = new Dictionary<string, TileTypes[,]>();

            bottomBounds = new HiddenStageObject(1500, 200, 0, 901);
            obstructiveStageObjects.Add(bottomBounds);
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

            elevator = new Elevator(elevatorOpen, elevatorClosed);

            // Loading the blocked tile texture
            blockedTileTexture = content.Load<Texture2D>("BlockedTile");

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
        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach(StageObject s in obstructiveStageObjects)
            {
                s.Draw(_spriteBatch, SpriteEffects.None);
            }

            elevator.Draw(_spriteBatch, SpriteEffects.None);
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
            foreach (Enemy e in enemies) 
            { 
                CheckBlockedSides(e);
            }
            foreach (Projectile p in LevelManager.ProjectileList)
            {
                CheckBlockedSides(p);
            }
            CheckBlockedSides(player);

            elevator.PlayerEnters(player);
        }

        /// <summary>
        /// Generates a new level, selecting a layout from those pulled from the file
        /// </summary>
        public void GenerateLevel()
        {
            // Clearing all of the current tiles to make way for new ones
            foreach(StageObject so in obstructiveStageObjects)
            {
                if (so is BlockedTile)
                {
                    obstructiveStageObjects.Remove(so);
                }
            }

            // Choosing a new layout at random
            int layoutChoice = rng.Next(0, 4);

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

            // Creating tiles and adding them to the list of stage objects
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (currentLayout[j, i] == TileTypes.blocked)
                    {
                        obstructiveStageObjects.Add(new BlockedTile((180 + 114 * j), (100 + 100 * i), blockedTileTexture));
                    }
                }
            }
        }
    }
}
