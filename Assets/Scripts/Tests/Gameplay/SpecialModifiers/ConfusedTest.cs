using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ConfusedTest
    {
        Confused modifier;
        SnakeSegment segment;

        [SetUp]
        public void SetUp() {
            modifier = new Confused();
            segment = new GameObject().AddComponent<SnakeSegment>();
            segment.gameObject.AddComponent<SpecialComponent>();
            // segment.Modifier = modifier;
        }

        [Test]
        public void InvertsDirection()
        {
            Vector3 direction = new Vector3(1, 2, 3);
            modifier.DirectionModifier(ref direction);
            Assert.That(direction, Is.EqualTo(new Vector3(-1,-2,-3)));
        }

        [UnityTest]
        public IEnumerator DeactivatesAfterTime()
        {
            // modifier.maxTime = .01f;
            Assert.That(segment.Modifier, Is.EqualTo(modifier));
            yield return new WaitForSeconds(.02f);
            Assert.That(segment.Modifier, Is.Null);
        }
    }
}
