using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class CarSpawningTests
{
    private GameManagerScript gameManager;

    [SetUp]
    public void SetUp()
    {
        GameObject gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManagerScript>();
        gameManager.carPrefab = new GameObject("CarPrefab");
        gameManager.vanPrefab = new GameObject("VanPrefab");
        gameManager.deer = new GameObject();
        gameManager.scoreText = new GameObject().AddComponent<TextMeshProUGUI>();
        gameManager.gameOverPanel = new GameObject();
        gameManager.spawnInterval = 0.1f;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(gameManager.gameObject);
    }

    [UnityTest]
    public IEnumerator TestCarSpawnPosition()
    {
        gameManager.minSpawnRadius = 5f;
        gameManager.maxSpawnRadius = 10f;
        gameManager.deer.transform.position = Vector3.zero;

        GameObject car = new GameObject("CarPrefab");
        car.AddComponent<MeshRenderer>();
        car.AddComponent<CarBehavior>();
        gameManager.carPrefab = car;

        gameManager.SpawnCar();
        yield return null;

        GameObject[] spawnedCars = GameObject.FindGameObjectsWithTag("Car");
        Assert.AreEqual(1, spawnedCars.Length);

        Vector3 spawnPosition = spawnedCars[0].transform.position;
        Assert.IsTrue(spawnPosition.magnitude >= gameManager.minSpawnRadius);
        Assert.IsTrue(spawnPosition.magnitude <= gameManager.maxSpawnRadius);
    }


    [UnityTest]
    public IEnumerator TestRandomCarTypeSpawned()
    {
        gameManager.spawnInterval = 0.1f;
        gameManager.deer.transform.position = Vector3.zero;
        gameManager.SpawnCar();
        yield return null;
        GameObject[] spawnedCars = GameObject.FindGameObjectsWithTag("Car");
        GameObject[] spawnedVans = GameObject.FindGameObjectsWithTag("Van");
        Assert.IsTrue(spawnedCars.Length + spawnedVans.Length == 1);
    }

    [Test]
    public void TestCarSpeed()
    {
        GameObject carObject = new GameObject();
        carObject.AddComponent<CarBehavior>();
        NavMeshAgent navMeshAgent = carObject.AddComponent<NavMeshAgent>();
        GameManagerScript gameManager = new GameObject().AddComponent<GameManagerScript>();
        gameManager.baseSpeed = 5f;
        gameManager.speedMultiplier = 0.1f;
        gameManager.AddPoints(50f);

        CarBehavior carBehavior = carObject.GetComponent<CarBehavior>();
        float expectedSpeed = gameManager.baseSpeed + (gameManager.score * gameManager.speedMultiplier);
        carBehavior.SetSpeed(expectedSpeed);

        navMeshAgent.speed = carBehavior.getSpeed();
        Assert.AreEqual(expectedSpeed, navMeshAgent.speed, 0.01f);
    }


    [Test]
    public void TestScoreIncrease()
    {
        gameManager.score = 0f;
        gameManager.scoreIncreaseRate = 5f;
        gameManager.score += gameManager.scoreIncreaseRate * 2f;
        Assert.Greater(gameManager.score, 0f);
    }

    [Test]
    public void TestApplyRandomColor()
    {
        GameObject car = new GameObject();
        car.AddComponent<MeshRenderer>();

        gameManager.ApplyRandomColor(car);

        Renderer carRenderer = car.GetComponentInChildren<Renderer>();
        Assert.IsNotNull(carRenderer);
        Assert.AreNotEqual(carRenderer.material.color, Color.black);
    }

}
