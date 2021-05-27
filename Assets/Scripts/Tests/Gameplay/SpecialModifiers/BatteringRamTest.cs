using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BatteringRamTest
    {
        BatteringRam modifier;
        SnakeSegment segment;
        Collider other;

        [SetUp]
        public void SetUp() {
            segment = new GameObject().AddComponent<SnakeSegment>();
            segment.gameObject.AddComponent<SpecialComponent>();
            Snake snake = new GameObject().AddComponent<Snake>();
            segment.transform.SetParent(snake.transform);
            snake.Head = segment;

            GameObject otherObject = new GameObject();
            other = otherObject.AddComponent<SphereCollider>();

            modifier = new BatteringRam();
            segment.Modifier = modifier;
        }

        [Test]
        public void CollidesWithCollectables() {
            other.gameObject.AddComponent<Collectable>();
            bool collisionModified = modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            Assert.That(collisionModified, Is.False);
        }

        [Test]
        public void SnakeHeadCrossesSnakeSegment() {
            other.gameObject.AddComponent<SnakeSegment>();
            bool collisionModified = modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            Assert.That(collisionModified, Is.True);
        }

        [Test]
        public void WhenSegmentIsNotHeadDoNotModifyCollision() {
            segment.Snake.Head = null;
            other.gameObject.AddComponent<SnakeSegment>();
            bool collisionModified = modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            Assert.That(collisionModified, Is.False);
        }

        [Test]
        public void WhenCrossingModifyCollision() {
            other.gameObject.AddComponent<SnakeSegment>();
            modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            segment.Snake.Head = null;
            bool collisionModified = modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            Assert.That(collisionModified, Is.True);
        }

        [Test]
        public void WhenCrossingCollidesWithOtherPositions() {
            other.gameObject.AddComponent<SnakeSegment>();
            modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            other.transform.position += Vector3.one;
            bool collisionModified = modifier.CollisionModifier(segment, other.GetComponent<Collider>());
            Assert.That(collisionModified, Is.False);
        }
    }
}
