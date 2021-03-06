using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArenaTests
    {

        private Arena arena;

        [SetUp]
        public void SetUp(){
            arena = new GameObject().AddComponent<Arena>();
            arena.Width = 40;
            arena.Height = 20;
        }
        
        [Test]
        public void KeepWithinBounds()
        {
            Transform transform = new GameObject().transform;
            transform.position = new Vector3(65, 35);
            Vector3 expectedPosition = new Vector3(25, 15);

            arena.KeepWithinBounds(ref transform);
            Assert.That(transform.position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void KeepWithinBoundsNegative()
        {
            Transform transform = new GameObject().transform;
            transform.position = new Vector3(-65, -35);
            Vector3 expectedPosition = new Vector3(15, 5);

            arena.KeepWithinBounds(ref transform);
            Assert.That(transform.position, Is.EqualTo(expectedPosition));
        }
    }
}
