using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: Enemy Manager
//Purpose: An instance of this class is created for each level by the level manager.
//         This class spawns and keeps track of the enemies that are still alive.

namespace Team3Project.Enemy_Stuff
{
    enum EnemyTypes
    {
        Melee,
        Ranged
    }

    internal class EnemyManager
    {
        //Fields
        //TEMPORARY HEALTH INT, ATTACK DELAY, PROJECTILE SPEED
        int enemyHealth;
        int attackDelay;
        int projectileSpeed;

        //List of enemies that spawn at the beginning of the level
        List<Enemy> enemyList;

        //Get-only property for enemy list, because StageObjectManager needs to see it
        public List<Enemy> Enemies
        {
            get { return enemyList; }
        }

        //Dictionary with values for enemy speed
        Dictionary<int, int> enemyMoveSpeed = new Dictionary<int, int>(){
            { 0, 3 },
            { 1, 2 },
        };

        //Dictionary with values for enemy collision height/width
        //***NOTE: update when sprites are added
        Dictionary<int, Vector2> enemySize = new Dictionary<int, Vector2>(){
            { 0, new Vector2(10, 20) },
            { 1, new Vector2(10, 20) },
        };

        //Constructor
        public EnemyManager(List<int[]> enemyData)
        {
            //Enemy data info:
            //[0] - Enemy type (0 = melee, 1 = ranged)
            //[1] - X position
            //[2] - Y position

            //Populate enemy list
            for (int i = 0; i < enemyData.Count; i++)
            {
                int enemyType = enemyData[i][0];
                Rectangle collisionBox = new Rectangle(enemyData[i][1], enemyData[i][2], (int)enemySize[enemyType].X, (int)enemySize[enemyType].Y);
                if (enemyType == 0)
                {
                    enemyList.Add(new MeleeEnemy(enemyHealth, enemyMoveSpeed[enemyType], collisionBox, attackDelay));//update this to include another dictionary
                }
                else if (enemyType == 1)
                {
                    enemyList.Add(new RangedEnemy(enemyHealth, enemyMoveSpeed[enemyType], collisionBox, attackDelay, projectileSpeed));
                }
            }
        }

        //Methods
        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
