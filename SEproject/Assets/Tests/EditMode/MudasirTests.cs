// Mudasir Ali

using NUnit.Framework;
using UnityEngine;
using NSubstitute;

using System.Collections;

[TestFixture]
public class MudasirTests
{
    private GameManagerScript gameManager;
    private GameObject gameManagerObject;

    [SetUp]
    public void SetUp()
    {
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManagerScript>();
        //
        gameManager.carPrefab = new GameObject();
        gameManager.vanPrefab = new GameObject();
        gameManager.deer = new GameObject();
        gameManager.scoreText = Substitute.For<TMPro.TextMeshProUGUI>();
        gameManager.gameOverScore = Substitute.For<TMPro.TextMeshProUGUI>();
        gameManager.audioSource = Substitute.For<AudioSource>();
        gameManager.laughSounds = new AudioClip[1];
        gameManager.laughSounds[0] = AudioClip.Create("TestClip", 44100, 1, 44100, false);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManagerObject);
    }

    [Test]
    public void CalculateSpeed_BaseSpeed()
    {
        gameManager.baseSpeed = 5f;
        gameManager.maxSpeed = 20f;
        gameManager.speedMultiplier = 0.1f;
        gameManager.score = 0f;

        float speed = gameManager.CalculateSpeed();

        Assert.AreEqual(5f, speed);
    }

    [Test]
    public void CalculateSpeed_MaxSpeed()
    {
        gameManager.baseSpeed = 5f;
        gameManager.maxSpeed = 20f;
        gameManager.speedMultiplier = 0.1f;
        gameManager.score = 200f;

        float speed = gameManager.CalculateSpeed();

        Assert.AreEqual(20f, speed);
    }

    [Test]
    public void CalculateSpeed_IntermediateSpeed()
    {
        gameManager.baseSpeed = 5f;
        gameManager.maxSpeed = 20f;
        gameManager.speedMultiplier = 0.1f;
        gameManager.score = 50f;

        float speed = gameManager.CalculateSpeed();

        Assert.AreEqual(10f, speed);
    }

    [Test]
    public void AddPoints_IncreasesScore()
    {
        gameManager.score = 0f;
        gameManager.AddPoints(10f);

        Assert.AreEqual(10f, gameManager.score);
    }

    [Test]
    public void AddPoints_MultipleIncreases()
    {
        gameManager.score = 0f;
        gameManager.AddPoints(10f);
        gameManager.AddPoints(5f);

        Assert.AreEqual(15f, gameManager.score);
    }
}
