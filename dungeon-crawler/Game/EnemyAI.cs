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
        private Grid movementGrid;
        private Entity player;
        private List<Entity> enemyList = new List<Entity>();
        private List<Entity> enemiesInRange = new List<Entity>();

        public void Clear()
        {
            if (enemyList != null)
            {
                enemyList.Clear();
            }
        }

        public EnemyAI(Grid movementGrid, List<Entity> enemyList, Entity player)
        {
            this.movementGrid = movementGrid;
            this.enemyList = enemyList;
            this.player = player;
        }
        public void Update(GameTime gameTime)
        {

            int enemyDeathCount = 0;

            foreach (Entity enemy in enemiesInRange)
            {
                if (enemy.Dead)
                {
                    enemyDeathCount++;
                }
                else
                {
                    enemy.PathFinder = new PathFinder(gameTime, movementGrid, enemiesInRange);
                    enemy.PathFinder.FindPathToTarget(enemy, player);
                    enemy.PathFinder.MoveUnit(enemy, 0.07f, 15, gameTime);
                    enemy.Attack(player);
                }
            }

            // Attack the player if an enemy is within range.
            foreach (Entity enemy in enemyList)
            {
                float enemyDistance = Vector2.Distance(player.Position, enemy.Position);

                if (enemyDistance < 90 && enemy.State != Action.Dead || enemy.Aggroed)
                {
                    // Keep a list to find paths of the nearest enemies.
                    if (!enemiesInRange.Contains(enemy))
                    {
                        enemiesInRange.Add(enemy);
                    }
                }
                else
                {
                    enemy.PathFinder = null;
                    enemiesInRange.Remove(enemy);
                }

                enemy.Update(gameTime);

            }
        }
    }
}
