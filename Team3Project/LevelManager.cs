using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Enemy_Stuff;
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

        //Static manager objects that are updated and re-initialized for each new level
        static StageObjectManager stageObjectManager;

        /// <summary>
        /// Purpose: Sets up level and creates the stage object manager for the level
        /// Testing use: Can call specific methods for testing things out of the traditional way the game would be played
        /// </summary>
        public static void SetUpLevel(Texture2D meleeTexture)
        {
            Enemy enemy1;
            Rectangle enemy1Rect = new Rectangle(600, 600, enemyDefaults[EnemyTypes.Melee][Stats.Width], enemyDefaults[EnemyTypes.Melee][Stats.Height]);
            enemy1 = new MeleeEnemy(enemyDefaults[EnemyTypes.Melee][Stats.Health], enemyDefaults[EnemyTypes.Melee][Stats.MoveSpeed], enemy1Rect, enemyDefaults[EnemyTypes.Melee][Stats.AttackDelay], meleeTexture);
            AddEnemy(enemy1);

            //stageObjectManager = new StageObjectManager();
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
            enemyDefaults.Add(EnemyTypes.Ranged, new Dictionary<Stats, int>() { { Stats.Health, 100 }, { Stats.MoveSpeed, 3 }, { Stats.Height, 100 }, { Stats.Width, 50 }, { Stats.AttackDelay, 60 } });
        }

        /// <summary>
        /// Adds an enemy object to enemy list
        /// </summary>
        /// <param name="enemy"></param>
        public static void AddEnemy(Enemy enemy)
        {
            enemyList.Add(enemy);
        }

        /// <summary>
        /// Update runs once per Game1's update method
        /// </summary>
        public static void Update()
        {
            UpdateProjectiles();
            UpdateEnemies();
            //stageObjectManager.Update();
        }

        /// <summary>
        /// Runs through the list of projectiles, checking if they are still active and calling Update() for the ones that are.
        /// </summary>
        public static void UpdateProjectiles()
        {
            foreach (Projectile projectile in projectileList)
            {
                //If a projectile is no longer active, remove it from the list
                if (!projectile.Active)
                {
                    projectileList.Remove(projectile);
                }
                else
                {
                    projectile.Update();
                }
            }
        }

        /// <summary>
        /// Runs through the list of enemies, checking if they are still active and calling Update() for the ones that are.
        /// </summary>
        public static void UpdateEnemies()
        {
            foreach (Enemy enemy in enemyList)
            {
                if (!enemy.Active)
                {
                    enemyList.Remove(enemy);
                }
                else
                {
                    enemy.Update();
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch, spriteEffects);
            }
        }
    }
}
