using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HeadBombTest
    {
        HeadBomb modifier;
        Snake snake;
        SnakeSegment segment;
        GameObject other;

        [SetUp]
        public void SetUp() {
            // snake = new GameObject().AddComponent<Snake>();
            // snake.Init();
            // segment = snake.Head;

            // GameObject otherObject = new GameObject();
            // other = otherObject.AddComponent<SphereCollider>();

            // GameplayController controller = new GameObject().AddComponent<GameplayController>();
            // controller.enabled = false;
            // controller.CreateArena();
            // controller.GameMode = new GameplayMode();
            // modifier = new HeadBomb();
            // segment.Modifier = modifier;
            // modifier.Activate(controller);
        }

        [Test]
        public void WhenCollidesWithCollectableRemovesModifier() {
            Collectable collectable = other.gameObject.AddComponent<Collectable>();
            collectable.Modifier = new EnginePower();
            bool collisionModified = modifier.CollisionModifier(segment, other);
            Assert.That(collisionModified, Is.True);
            Assert.That(collectable.Modifier, Is.Null);
            Assert.That(snake.transform.childCount, Is.EqualTo(4));
            Assert.That(snake.Head.Modifier, Is.Null);
        }

        [Test]
        public void WhenSegmentIsNotHeadDoNotModifyCollision() {
            segment.Snake.Head = null;
            other.gameObject.AddComponent<SnakeSegment>();
            bool collisionModified = modifier.CollisionModifier(segment, other);
            Assert.That(collisionModified, Is.False);
        }

        [Test]
        public void WhenCollidesKillsOtherSnake() {
            Snake otherSnake = new GameObject().AddComponent<Snake>();
            // otherSnake.Init();
            SnakeSegment otherSegment = otherSnake.Head.NextSegment;
            bool collisionModified = modifier.CollisionModifier(segment, otherSegment.gameObject);
            Assert.That(collisionModified, Is.True);
            Assert.That(otherSnake.isActiveAndEnabled, Is.False);
        }
    }
}
