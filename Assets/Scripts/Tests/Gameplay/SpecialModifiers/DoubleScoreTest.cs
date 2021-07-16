using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DoubleScoreTest
    {
        DoubleScore modifier;

        [SetUp]
        public void SetUp() {
            modifier = new DoubleScore();
        }
        
        [Test]
        public void DoublesScore()
        {
            int startingScoreGain = 1;
            int scoreGain = startingScoreGain;
            modifier.ScoreGainModifier(ref scoreGain);
            Assert.That(scoreGain, Is.EqualTo(startingScoreGain * 2));
        }
    }
}
