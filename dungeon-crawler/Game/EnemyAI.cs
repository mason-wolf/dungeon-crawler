using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using RoyT.AStar;

namespace DungeonCrawler.Engine
{
    public class EnemyAI : IUpdate
    {
        // Map collision data
        private Grid grid;
        private Entity player;
        // List of all enemies
        private List<Entity> enemyList = new List<Entity>();
        // List of enemies within range
        private List<Entity> enemiesInRange = new List<Entity>();
        // Keep track of the nearest enemy to store pathfinding data
        // to s  hare with other entities.
        Entity nearestEnemy = null;
        float nearestEnemyDistance = 0;
        // Throttle frames to reduce latency when invoking path finder.
        int frameCount = 0;
        bool nearestEnemyFound = false;

        /// <summary>
        /// Clears list of enemies.
        /// </summary>
        public void Clear()
        {
            if (enemyList != null)
            {
                enemyList.Clear();
            }
        }

        /// <summary>
        /// Creates an instance of Enemy AI.
        /// </summary>
        /// <param name="grid">Map collision data</param>
        /// <param name="enemyList">List of enemies</param>
        /// <param name="player">Player object</param>
        public EnemyAI(Grid grid, List<Entity> enemyList, Entity player)
        {
            this.grid = grid;
            this.enemyList = enemyList;
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            int enemyDeathCount = 0;
            frameCount++;
            // Keep track of enemies within range.
            foreach (Entity enemy in enemyList)
            {
                // Calculate enemy distances within an appropriate range.
                float enemyDistance = Vector2.Distance(player.Position, enemy.Position);
                if (enemyDistance < 200 && enemy.State != Action.Dead || enemy.Aggroed)
                {
                    if (!enemiesInRange.Contains(enemy))
                    {
                        enemiesInRange.Add(enemy);
                    }
                }
                else
                {
                    // Remove enemy and reset path finding waypoints if out of range.
                    enemy.PathFinder = null;
                    enemiesInRange.Remove(enemy);
                }

                enemy.Update(gameTime);
            }

            // Throttle the amount of path finder instances.
            if (frameCount < 10)
            {
                foreach (Entity enemy in enemiesInRange)
                {
                    if(!enemy.Dead && enemy.PathFinder == null)
                    {
                        enemy.PathFinder = new PathFinder(gameTime, grid, enemiesInRange) ;
                    }
                }
            }

            // Sort enemies by distance.
            if (enemiesInRange.Count > 0)
            {
                enemiesInRange.Sort((e1, e2) => e1.Distance.CompareTo(e2.Distance));
            }

            foreach (Entity enemy in enemiesInRange)
            {
                if (enemy.Dead)
                {
                    enemyDeathCount++;
                }
                else
                {
                    if (enemy.PathFinder != null)
                    {
                        // Find the closest enemy and find path to player.
                        if (!nearestEnemyFound || nearestEnemy.Dead)
                        {
                            enemy.PathFinder = new PathFinder(gameTime, grid, enemiesInRange);
                            enemy.PathFinder.FindPathToTarget(enemy, player);
                            nearestEnemy = enemy;
                            nearestEnemyFound = true;
                        }
                        else
                        {
                            if (nearestEnemy != null)
                            {
                                nearestEnemyDistance = Vector2.Distance(nearestEnemy.Position, player.Position);
                                // If nearest enemy is still pursuing, share waypoints to other enemies.
                                if (nearestEnemyDistance > 15 && nearestEnemy.PathFinder != null)
                                {
                                    enemy.PathFinder.SetWayPoints(nearestEnemy.PathFinder.GetWayPoints());
                                }
                                else
                                {
                                    // Otherwise find their own path.
                                    enemy.PathFinder.FindPathToTarget(enemy, player);
                                   
                                }
                            }
                            enemy.PathFinder.MoveUnit(enemy, 0.06f, 1, gameTime);
                            enemy.Attack(player);
                        }
                    }
                }
            }

            if (frameCount >= 30)
            {
                frameCount = 0;
                nearestEnemyFound = false;
            }
        }
    }
}
