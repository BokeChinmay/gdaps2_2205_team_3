using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Stage_Stuff
{
    internal class StageObjectManager
    {
        // enum for use with assigning tile types on level generation
        enum TileTypes
        {
            empty
        }

        // Field declarations
        private StreamReader reader;

        private List<StageObject> obstructiveStageObjects;
        private Dictionary<string, TileTypes[,]> levelLayouts;
        
        private Texture2D bufferTexture;
        private VisualBuffer leftBuffer;
        private VisualBuffer rightBuffer;

        // Default constructor
        public StageObjectManager()
        {
            obstructiveStageObjects = new List<StageObject>();
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

            leftBuffer = new VisualBuffer
                (0, bufferTexture);
            obstructiveStageObjects.Add(leftBuffer);

            rightBuffer = new VisualBuffer
                (_graphics.PreferredBackBufferWidth - bufferTexture.Width, bufferTexture);
            obstructiveStageObjects.Add(rightBuffer);

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
        }

        /// <summary>
        /// Drawing all of the stage objects
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            leftBuffer.Draw(_spriteBatch, SpriteEffects.None);
            rightBuffer.Draw(_spriteBatch, SpriteEffects.None);
        }

        /// <summary>
        /// Checks if an entity is blocked in any direction by a stage object
        /// </summary>
        /// <param name="entity"> the entity whose surroundings are being checked </param>
        public void CheckBlockedSides(Entity entity)
        {
            foreach (StageObject obj in obstructiveStageObjects)
            {
                // Checking if the entity is blocked from above
                if (obj.IsObstruction &&
                    entity.Collision.Top <= obj.BottomEdge + 5 &&
                    entity.Collision.Bottom >= obj.BottomEdge + entity.Collision.Height &&
                    (entity.Collision.Right <= obj.RightEdge &&
                    entity.Collision.Right >= obj.LeftEdge ||
                    entity.Collision.Left <= obj.RightEdge &&
                    entity.Collision.Left >= obj.LeftEdge))
                {
                    entity.TopBlocked = true;
                }
                else
                {
                    entity.TopBlocked = false;
                }

                // Checking if the entity is blocked from below
                if (obj.IsObstruction &&
                    entity.Collision.Bottom >= obj.TopEdge - 5 &&
                    entity.Collision.Top <= obj.TopEdge - entity.Collision.Height &&
                    (entity.Collision.Right <= obj.RightEdge &&
                    entity.Collision.Right >= obj.LeftEdge ||
                    entity.Collision.Left <= obj.RightEdge &&
                    entity.Collision.Left >= obj.LeftEdge))
                {
                    entity.BottomBlocked = true;
                }
                else
                {
                    entity.BottomBlocked = false;
                }

                // Checking if the entity is blocked from the left
                if (obj.IsObstruction &&
                    entity.Collision.Left <= obj.RightEdge + 5 &&
                    entity.Collision.Right >= obj.RightEdge + entity.Collision.Width &&
                    (entity.Collision.Bottom <= obj.BottomEdge &&
                    entity.Collision.Bottom >= obj.TopEdge ||
                    entity.Collision.Top <= obj.BottomEdge &&
                    entity.Collision.Top >= obj.TopEdge))
                {
                    entity.LeftBlocked = true;
                }
                else
                {
                    entity.LeftBlocked = false;
                }

                // Checking if the entity is blocked from the right
                if (obj.IsObstruction &&
                    entity.Collision.Right >= obj.LeftEdge + 5 &&
                    entity.Collision.Left <= obj.LeftEdge + entity.Collision.Width &&
                    (entity.Collision.Bottom <= obj.BottomEdge &&
                    entity.Collision.Bottom >= obj.TopEdge ||
                    entity.Collision.Top <= obj.BottomEdge &&
                    entity.Collision.Top >= obj.TopEdge))
                {
                    entity.RightBlocked = true;
                }
                else
                {
                    entity.RightBlocked = false;
                }
            }
        }
    }
}
