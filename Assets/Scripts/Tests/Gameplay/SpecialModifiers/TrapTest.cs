using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrapTest
    {
        // A Test behaves as an ordinary method
        Trap modifier;
        SnakeSegment segment;

        [SetUp]
        public void SetUp() {
            modifier = new Trap();
            segment = new GameObject().AddComponent<SnakeSegment>();
            segment.gameObject.AddComponent<SpecialComponent>();
            // segment.Modifier = modifier;
        }

        [Test]
        public void SetsDirectionToZero()
        {
            Vector3 direction = new Vector3(1, 2, 3);
            modifier.DirectionModifier(ref direction);
            Assert.That(direction, Is.EqualTo(Vector3.zero));
        }

        [UnityTest]
        public IEnumerator DeactivatesAfterTime()
        {
            modifier.MaxTime = .01f;
            Assert.That(segment.Modifier, Is.EqualTo(modifier));
            yield return new WaitForSeconds(.02f);
            Assert.That(segment.Modifier, Is.Null);
        }
    }
}
