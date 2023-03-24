using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Enemy_Stuff;
using Team3Project.Stage_Stuff;

//Name: Level Manager
//Purpose: Static Class that oversees more specific managers as well as the projectile list

namespace Team3Project
{
    internal static class LevelManager
    {
        //List of all projectiles on screen
        static List<Projectile> projectileList;

        static public List<Projectile> ProjectileList
        {
            get { return projectileList; }
        }

        //Static manager objects that are updated and re-initialized for each new level
        static EnemyManager enemyManager;
        static StageObjectManager stageObjectManager;

        /// <summary>
        /// Adds a new projectile to the global projectile list
        /// </summary>
        /// <param name="projectile"></param>
        public static void AddProjectile(Projectile projectile)
        {
            projectileList.Add(projectile);
        }

        public static void Update()
        {
            UpdateProjectiles();
            enemyManager.Update();
            stageObjectManager.Update();
        }

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
    }
}
