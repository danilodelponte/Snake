using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EnginePowerTest
    {
        EnginePower modifier;

        [SetUp]
        public void SetUp() {
            modifier = new EnginePower();
        }
        
        [Test]
        public void DecreasesMovementDeltaTime()
        {
            float startValue = 1f;
            float movingDelta = startValue;
            modifier.MovementModifier(ref movingDelta);
            Assert.That(movingDelta, Is.LessThan(startValue));
        }
    }
}
