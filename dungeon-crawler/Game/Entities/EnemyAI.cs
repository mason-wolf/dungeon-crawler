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
        // Throttle frames to reduce latency when invoking path finder.
        int frameCount = 0;
        bool nearestEnemyFound = false;

        int pathFinderCount = 0;
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
                if (enemyDistance < 250 && enemy.State != Action.Dead || enemy.Aggroed && !nearestEnemyFound)
                {
                    if (!enemiesInRange.Contains(enemy) && enemy.Movable)
                    {
                        enemy.Distance = enemyDistance;
                        enemy.PathFinder = new PathFinder(gameTime, grid, enemiesInRange);
                        enemiesInRange.Add(enemy);
                    }
                }
                else
                {
                    // Remove enemy and reset path finding waypoints if out of range.
                //    enemy.PathFinder = null;
                    enemiesInRange.Remove(enemy);
                }

                enemy.Update(gameTime);
            }


          //  Sort enemies by distance.
            if (enemiesInRange.Count > 0 && !nearestEnemyFound)
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
                    enemy.PathFinder.MoveUnit(enemy, 0.06f, 1, gameTime);

                    if (enemy.PathFinder != null)
                    {
                        // Find the closest enemy and find path to player.
                        if (!nearestEnemyFound && enemy.Movable || nearestEnemy.Dead && pathFinderCount < 25)
                        {
                            Entity closestEnemy = enemyList
                            .Where(e => e.Movable)
                            .OrderBy(e => Vector2.Distance(e.Position, player.Position))
                            .FirstOrDefault();
                            closestEnemy.PathFinder = new PathFinder(gameTime, grid, enemiesInRange);
                            closestEnemy.PathFinder.FindPathToTarget(closestEnemy, player);
                            nearestEnemy = closestEnemy;
                            nearestEnemyFound = true;
                        }
                        else
                        {
                            if (nearestEnemyFound && nearestEnemy.PathFinder != null)
                            {
                                enemy.PathFinder.SetWayPoints(nearestEnemy.PathFinder.GetWayPoints());
                            }

                            if (nearestEnemy.PathFinder != null && 
                                nearestEnemy.PathFinder.GetWayPoints().Count == 0 && 
                                pathFinderCount < 25)
                            {
                                pathFinderCount++;
                                enemy.PathFinder = new PathFinder(gameTime, grid, enemiesInRange);
                                enemy.PathFinder.FindPathToTarget(enemy, player);
                            }
                        }
                    //    enemy.PathFinder.MoveUnit(enemy, 0.06f, 1, gameTime);
                        enemy.Attack(player);
                    }
                }
            }

            if (frameCount >= 30)
            {
                frameCount = 0;
                pathFinderCount = 0;
                nearestEnemyFound = false;
            }
        }
    }
}
