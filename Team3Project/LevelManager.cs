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

        static List<Healthbar> healthbarList;

        //Dictionary for enemy default stats
        static Dictionary<EnemyTypes, Dictionary<Stats, int>> enemyDefaults;

        // Field and property allowing other classes to see whether enemies are present
        static bool enemiesPresent;

        static public bool EnemiesPresent
        {
            get { return enemiesPresent; }
        }

        static List<Item> itemList;
        static bool itemDropped;
        //Item dictionary - for displaying the items that have been picked up throughout the run
        static Dictionary<ItemType, int> itemDictionary;
        static public Dictionary<ItemType, int> ItemDictionary
        {
            get { return itemDictionary; }
        }

        //Random number generator
        static Random rng;

        //Static manager objects that are updated and re-initialized for each new level
        static StageObjectManager stageObjectManager;

        //Enemy textures
        static Texture2D projectileTexture;
        static Texture2D meleeTexture;
        static Texture2D rangedTexture;

        //Item textures
        static Texture2D damageUp;
        static Texture2D speedUp;
        static Texture2D extraLife;
        static Texture2D healthPickup;

        //Health bar textures
        static Texture2D smallHBBack;
        static Texture2D smallHBFront;
        static Texture2D largeHBBack;
        static Texture2D largeHBFront;

        /// <summary>
        /// Purpose: Sets up level and creates the stage object manager for the level
        /// Testing use: Can call specific methods for testing things out of the traditional way the game would be played
        /// </summary>
        public static void SetUpTextures(Texture2D meleeTexture, Texture2D rangedTexture, Texture2D pTexture,
            Texture2D damageUp, Texture2D speedUp, Texture2D extraLife, Texture2D healthPickup,
            Texture2D smallHBBack, Texture2D smallHBFront, Texture2D largeHBBack, Texture2D largeHBFront)
        {
            projectileTexture = pTexture;
            LevelManager.meleeTexture = meleeTexture;
            LevelManager.rangedTexture = rangedTexture;

            LevelManager.damageUp = damageUp;
            LevelManager.speedUp = speedUp;
            LevelManager.extraLife = extraLife;
            LevelManager.healthPickup = healthPickup;

            LevelManager.smallHBBack = smallHBBack;
            LevelManager.smallHBFront = smallHBFront;
            LevelManager.largeHBBack = largeHBBack;
            LevelManager.largeHBFront = largeHBFront;
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
            healthbarList = new List<Healthbar>();
            itemList = new List<Item>();
            itemDropped = false;
            rng = new Random();
            itemDictionary = new Dictionary<ItemType, int>();

            //Sets up the dictionary
            enemyDefaults = new Dictionary<EnemyTypes, Dictionary<Stats, int>>();
            enemyDefaults.Add(EnemyTypes.Melee, new Dictionary<Stats, int>() { { Stats.Health, 50 }, { Stats.MoveSpeed, 5 }, { Stats.Height, 50 }, { Stats.Width, 50 }, { Stats.AttackDelay, 30 } });
            enemyDefaults.Add(EnemyTypes.Ranged, new Dictionary<Stats, int>() { { Stats.Health, 50 }, { Stats.MoveSpeed, 3 }, { Stats.Height, 75 }, { Stats.Width, 50 }, { Stats.AttackDelay, 360 }, { Stats.ProjectileSpeed, 10 } });
        }

        /// <summary>
        /// Adds an enemy object to enemy list
        /// </summary>
        /// <param name="enemy"></param>
        public static void AddEnemy(Enemy enemy)
        {
            enemyList.Add(enemy);
            healthbarList.Add(new Healthbar(enemy, smallHBBack, smallHBFront, largeHBBack, largeHBFront));
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

            foreach(Item item in itemList)
            {
                item.Update(player);
            }

            enemiesPresent = false;

            foreach (Enemy enemy in enemyList) 
            {
                if (enemy.Collision.Intersects(player.Collision) && enemy.Health >= 0)
                {
                    player.TakeDamage(1);
                }

                if (enemy.Active)
                {
                    enemiesPresent = true;
                }
            }

            foreach (Healthbar healthbar in healthbarList)
            {
                healthbar.Update();
            }

            if (!enemiesPresent && !itemDropped)
            {
                bool lifeCanDrop = false;
                bool healthCanDrop = false;

                if (player.Lives < 3)
                {
                    lifeCanDrop = true;
                }

                if (player.Health < player.MaxHealth)
                {
                    healthCanDrop = true;
                }

                DropItem(new Vector2(740, 450), lifeCanDrop, healthCanDrop, player.MoveSpeed);
                itemDropped = true;
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
                    player.TakeDamage(projectileList[i].Damage);
                    projectileList[i].Active = false;
                }

                foreach (Enemy enemy in enemyList)
                {
                    if (projectileList[i].Collision.Intersects(enemy.Collision) && projectileList[i].Friendly && enemy.Health > 0)
                    {
                        enemy.TakeDamage(projectileList[i]);
                        projectileList[i].Active = false;
                    }
                }
                
                //If a projectile is no longer active, remove it from the listw
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
                    healthbarList.Remove(healthbarList[i]);
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
        public static void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, SpriteFont font)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch, spriteEffects);
            }
            foreach (Projectile projectile in projectileList)
            {
                projectile.Draw(spriteBatch, spriteEffects);
                //FOR TESTING: spriteBatch.DrawString(font, "!!!", new Vector2(projectile.Collision.X, projectile.Collision.Y), Color.White);
            }
            foreach (Item item in itemList)
            {
                item.Draw(spriteBatch, spriteEffects);
            }
            foreach (Healthbar healthbar in healthbarList)
            {
                healthbar.Draw(spriteBatch, spriteEffects);
            }
        }

        /// <summary>
        /// Generates enemies based on the current level
        /// </summary>
        /// <param name="obstructiveObjects">The list of obstructive stage objects. Enemies can't be spawned on top of these</param>
        /// <param name="level">Current level</param>
        public static void LoadNewLevel(List<StageObject> obstructiveObjects, int level)
        {
            if (level % 10 == 0)
            {
                LoadBossLevel(obstructiveObjects, level);
            }
            else
            {
                enemyList.Clear();
                healthbarList.Clear();
                projectileList.Clear();
                double healthMultiplier = 1 + (0.1 * level);
                itemList.Clear();
                itemDropped = false;

                    //Load new enemies randomly using free spaces in the top 2/3 of the screen
                    //This funtion will increase the number of enemies for later levels
                    int numEnemies = (int)Math.Ceiling(Math.Sqrt(level));

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
                            Enemy newMeleeEnemy = new MeleeEnemy(
                                    (int)(enemyDefaults[enemyType][Stats.Health] * healthMultiplier),
                                    enemyDefaults[enemyType][Stats.MoveSpeed],
                                    newCollision,
                                    enemyDefaults[enemyType][Stats.AttackDelay],
                                    meleeTexture);
                            enemyList.Add(newMeleeEnemy);
                            healthbarList.Add(new Healthbar(newMeleeEnemy, smallHBBack, smallHBFront, largeHBBack, largeHBFront));
                            
                            break;
                        case EnemyTypes.Ranged:
                            Enemy newRangedEnemy = new RangedEnemy(
                                    (int)(enemyDefaults[enemyType][Stats.Health] * healthMultiplier),
                                    enemyDefaults[enemyType][Stats.MoveSpeed],
                                    newCollision,
                                    enemyDefaults[enemyType][Stats.AttackDelay],
                                    enemyDefaults[enemyType][Stats.ProjectileSpeed],
                                    rangedTexture,
                                    projectileTexture);
                            enemyList.Add(newRangedEnemy);
                            healthbarList.Add(new Healthbar(newRangedEnemy, smallHBBack, smallHBFront, largeHBBack, largeHBFront));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Every 10 levels, a boss room appears. This room contains a large version of the melee enemy as a boss with a few more melee enemies
        /// as minions.
        /// </summary>
        /// <param name="obstructiveObjects">List of obstructive stage objects</param>
        /// <param name="level">Player's current level</param>
        public static void LoadBossLevel(List<StageObject> obstructiveObjects, int level)
        {
            healthbarList.Clear();

            //Create boss enemy
            BossEnemy bossEnemy = new BossEnemy(
                300 + (100 * ((level / 10) - 1)),
                enemyDefaults[EnemyTypes.Melee][Stats.MoveSpeed] / 2,
                new Rectangle(500, 400, enemyDefaults[EnemyTypes.Melee][Stats.Width] * 3, enemyDefaults[EnemyTypes.Melee][Stats.Height] * 3),
                enemyDefaults[EnemyTypes.Melee][Stats.AttackDelay],
                meleeTexture
                );
            enemyList.Add(bossEnemy);
            healthbarList.Add(new Healthbar(bossEnemy, smallHBBack, smallHBFront, largeHBBack, largeHBFront));

            //Create minions - start with 2 and add 1 each consecutive time
            int numEnemies = 2 + ((level / 10) - 1);
            Random rand = new Random();
            for (int i = 0; i < numEnemies; i++)
            {
                EnemyTypes enemyType = EnemyTypes.Melee;
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

                double healthMultiplier = 1 + (0.1 * level);
                //Add enemy to list
                Enemy newMeleeEnemy = new MeleeEnemy(
                                    (int)(enemyDefaults[enemyType][Stats.Health] * healthMultiplier),
                                    enemyDefaults[enemyType][Stats.MoveSpeed],
                                    newCollision,
                                    enemyDefaults[enemyType][Stats.AttackDelay],
                                    meleeTexture
                                    );

                enemyList.Add(newMeleeEnemy);
                healthbarList.Add(new Healthbar(newMeleeEnemy, smallHBBack, smallHBFront, largeHBBack, largeHBFront));
            }
        }

        public static void DropItem(Vector2 location, bool missingLives, bool missingHealth, int playerSpeed)
        {
            Item newItem;
            int itemChoice;

            if (missingLives)
            {
                itemChoice = rng.Next(0, 100);
            }
            else
            {
                itemChoice = rng.Next(0, 90);
            }

            if (itemChoice < 40 || (itemChoice < 70 && !missingHealth))
            {
                newItem = new Item(1, 0,
                                new Rectangle((int)location.X, (int)location.Y, 60, 76),
                                damageUp, ItemType.DamageBoost);
            }
            else if (itemChoice < 70 || (itemChoice < 90 && playerSpeed > 9 && missingHealth))
            {
                newItem = new Item(1, 0,
                                new Rectangle((int)location.X, (int)location.Y, 66, 66),
                                healthPickup, ItemType.HealthPickup);
            }
            else if (itemChoice < 90)
            {
                newItem = new Item(1, 0,
                                new Rectangle((int)location.X, (int)location.Y, 64, 28),
                                speedUp, ItemType.SpeedBoost);
            }
            else
            {
                newItem = new Item(1, 0,
                                 new Rectangle((int)location.X, (int)location.Y, 64, 64),
                                 extraLife, ItemType.LifePickup);
            }

            newItem.Active = true;

            itemList.Add(newItem);
        }

        /// <summary>
        /// Adds an item of a given type to the dictionary
        /// </summary>
        /// <param name="type">Type of item added to list</param>
        public static void AddItem(ItemType type)
        {
            if (itemDictionary.ContainsKey(type))
            {
                itemDictionary[type] += 1;
            }
            else
            {
                itemDictionary.Add(type, 1);
            }
        }

        public static void DrawItemDictionary(SpriteBatch spriteBatch, SpriteEffects spriteEffects, Texture2D damageUpTexture, Texture2D speedUpTexture, SpriteFont font)
        {
            int drawY = 130;
            int drawX = 1350;
            foreach (ItemType type in itemDictionary.Keys)
            {
                if (type == ItemType.SpeedBoost)
                {
                    spriteBatch.Draw(speedUpTexture, new Rectangle(drawX, drawY + 50, 100, 50), Color.White);
                    spriteBatch.DrawString(font, String.Format("x" + itemDictionary[type]), new Vector2(drawX + 110, drawY + 50), Color.White);
                    drawY += 150;
                }
                else if (type == ItemType.DamageBoost)
                {
                    spriteBatch.Draw(damageUpTexture, new Rectangle(drawX, drawY, 100, 120), Color.White);
                    spriteBatch.DrawString(font, String.Format("x" + itemDictionary[type]), new Vector2(drawX + 110, drawY + 50), Color.White);
                    drawY += 150;
                }
            }
        }
    }
}
