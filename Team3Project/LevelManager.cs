using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Enemy_Stuff;
using Team3Project.Player_Stuff;
using Team3Project.Stage_Stuff;

//Name: Level Manager
//Purpose: Static Class that oversees more specific managers as well as the projectile and enemy lists

namespace Team3Project
{
    enum EnemyTypes
    {
        Melee,
        Ranged
    }

    enum Stats
    {
        Health,
        MoveSpeed,
        Height,
        Width,
        AttackDelay,
        ProjectileSpeed
    }

    internal static class LevelManager
    {
        //List of all projectiles on screen
        static List<Projectile> projectileList;

        static public List<Projectile> ProjectileList
        {
            get { return projectileList; }
        }

        //List of enemies that are currently active
        static List<Enemy> enemyList;

        static public List<Enemy> EnemyList
        {
            get { return enemyList; }
        }

        //Dictionary for enemy default stats
        static Dictionary<EnemyTypes, Dictionary<Stats, int>> enemyDefaults;

        // Field and property allowing other classes to see whether enemies are present
        static bool enemiesPresent;

        static public bool EnemiesPresent
        {
            get { return enemiesPresent; }
        }

        //Static manager objects that are updated and re-initialized for each new level
        static StageObjectManager stageObjectManager;

        //Enemy textures
        static Texture2D projectileTexture;
        static Texture2D meleeTexture;
        static Texture2D rangedTexture;

        /// <summary>
        /// Purpose: Sets up level and creates the stage object manager for the level
        /// Testing use: Can call specific methods for testing things out of the traditional way the game would be played
        /// </summary>
        public static void SetUpTextures(Texture2D meleeTexture, Texture2D rangedTexture, Texture2D pTexture)
        {
            projectileTexture = pTexture;
            LevelManager.meleeTexture = meleeTexture;
            LevelManager.rangedTexture = rangedTexture;
        }

        /// <summary>
        /// Adds a new projectile to the global projectile list
        /// </summary>
        /// <param name="projectile"></param>
        public static void AddProjectile(Projectile projectile)
        {
            projectileList.Add(projectile);
        }

        /// <summary>
        /// Sets up the enemy defaults list, which is used when creating enemies
        /// </summary>
        public static void Initialize()
        {
            enemyList = new List<Enemy>();
            projectileList = new List<Projectile>();

            //Sets up the dictionary
            enemyDefaults = new Dictionary<EnemyTypes, Dictionary<Stats, int>>();
            enemyDefaults.Add(EnemyTypes.Melee, new Dictionary<Stats, int>() { { Stats.Health, 100 }, { Stats.MoveSpeed, 5 }, { Stats.Height, 50 }, { Stats.Width, 50 }, { Stats.AttackDelay, 30 } });
            enemyDefaults.Add(EnemyTypes.Ranged, new Dictionary<Stats, int>() { { Stats.Health, 100 }, { Stats.MoveSpeed, 3 }, { Stats.Height, 100 }, { Stats.Width, 50 }, { Stats.AttackDelay, 360 }, { Stats.ProjectileSpeed, 10 } });
        }

        /// <summary>
        /// Adds an enemy object to enemy list
        /// </summary>
        /// <param name="enemy"></param>
        public static void AddEnemy(Enemy enemy)
        {
            enemyList.Add(enemy);
            enemiesPresent = true;
        }

        /// <summary>
        /// Update runs once per Game1's update method
        /// Updating enemies must be done BEFORE projectiles due to projectiles becoming inactive when they contact enemies
        /// </summary>
        public static void Update(Player player, GameTime gameTime)
        {
            UpdateEnemies(player.Collision, gameTime);
            UpdateProjectiles(player);

            enemiesPresent = false;

            foreach (Enemy enemy in enemyList) 
            { 
                if (enemy.Collision.Intersects(player.Collision) && enemy.Active)
                {
                    player.TakeDamage(1);
                }

                if (enemy.Active)
                {
                    enemiesPresent = true;
                }
            }
        }

        /// <summary>
        /// Runs through the list of projectiles, checking if they are still active and calling Update() for the ones that are.
        /// </summary>
        public static void UpdateProjectiles(Player player)
        {
            for (int i = 0; i < projectileList.Count; i++)
            {
                //Deactivate projectile if it is in contact with the player
                if (projectileList[i].Collision.Intersects(player.Collision) && !projectileList[i].Friendly)
                {
                    projectileList[i].Active = false;
                    player.TakeDamage(projectileList[i].Damage);
                }
                
                //If a projectile is no longer active, remove it from the list
                if (!projectileList[i].Active)
                {
                    projectileList.Remove(projectileList[i]);
                }
                else
                {
                    projectileList[i].Update();
                }
            }
        }

        /// <summary>
        /// Runs through the list of enemies, checking if they are still active and calling Update() for the ones that are.
        /// </summary>
        public static void UpdateEnemies(Rectangle playerCollision, GameTime gameTime)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].Active)
                {
                    enemyList.Remove(enemyList[i]);
                    i--;
                }
                else
                {
                    enemyList[i].Update(playerCollision, projectileList, gameTime);
                }
            }
        }

        /// <summary>
        /// Calls draw method for enemies and projectiles
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public static void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch, spriteEffects);
            }
            foreach (Projectile projectile in projectileList)
            {
                projectile.Draw(spriteBatch, spriteEffects);
            }
        }

        /// <summary>
        /// Generates enemies based on the current level
        /// </summary>
        /// <param name="obstructiveObjects">The list of obstructive stage objects. Enemies can't be spawned on top of these</param>
        /// <param name="level">Current level</param>
        public static void LoadNewLevel(List<StageObject> obstructiveObjects, int level)
        {
            enemyList.Clear();
            projectileList.Clear();

            //Load new enemies randomly using free spaces in the top 2/3 of the screen
            //This funtion will increase the number of enemies for later levels
            int numEnemies = (int) Math.Ceiling(Math.Sqrt(level));

            Random rand = new Random();

            for (int i = 0; i < numEnemies; i++)
            {
                EnemyTypes enemyType = (EnemyTypes)rand.Next(2);
                Rectangle newCollision = new Rectangle(0, 0, enemyDefaults[enemyType][Stats.Width], enemyDefaults[enemyType][Stats.Height]);
                bool pass = false;
                do
                {
                    //Randomize a new possible spawning location in the top 1/3 of the room
                    newCollision.X = rand.Next(200, 1300);
                    newCollision.Y = rand.Next(200, 400);

                    //Loop through the obstructive object list to see if the new location is valid
                    int count = 0;
                    foreach (StageObject stageObject in obstructiveObjects)
                    {
                        if (newCollision.Intersects(stageObject.Dimensions))
                        {
                            count++;
                        }
                    }

                    //If no intersection is found, break the loop
                    if (count == 0)
                    {
                        pass = true;
                    }
                    //Else, restart the loop
                    else
                    {
                        count = 0;
                    }
                } while (pass == false);

                switch (enemyType)
                {
                    case EnemyTypes.Melee:
                        enemyList.Add(
                            new MeleeEnemy(
                                enemyDefaults[enemyType][Stats.Health],
                                enemyDefaults[enemyType][Stats.MoveSpeed],
                                newCollision,
                                enemyDefaults[enemyType][Stats.AttackDelay],
                                meleeTexture
                                )
                            );
                        break;
                    case EnemyTypes.Ranged:
                        enemyList.Add(
                            new RangedEnemy(
                                enemyDefaults[enemyType][Stats.Health],
                                enemyDefaults[enemyType][Stats.MoveSpeed],
                                newCollision,
                                enemyDefaults[enemyType][Stats.AttackDelay],
                                enemyDefaults[enemyType][Stats.ProjectileSpeed],
                                rangedTexture,
                                projectileTexture
                                )
                            );
                        break;
                }
            }
        }
    }
}
