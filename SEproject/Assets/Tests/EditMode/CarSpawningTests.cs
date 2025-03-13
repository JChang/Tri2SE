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

        GameObject carPrefab = Resources.Load<GameObject>("car");
        if (carPrefab == null)
        {
            Debug.LogError("Car prefab not found in Resources folder!");
        }
        else
        {
            Debug.Log("Car prefab found in Resources folder!");
        }
        gameManager.carPrefab = carPrefab;

        GameObject vanPrefab = Resources.Load<GameObject>("minivan");
        if (vanPrefab == null)
        {
            Debug.LogError("Van prefab not found in Resources folder!");
        }
        else
        {
            Debug.Log("Van prefab found in Resources folder!");
        }
        gameManager.vanPrefab = vanPrefab;

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

        GameObject car = new GameObject("car");
        car.AddComponent<CarBehavior>();
        gameManager.carPrefab = car;

        gameManager.SpawnCar();
        yield return null;

        GameObject[] spawnedCars = GameObject.FindGameObjectsWithTag("car");
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
        GameObject[] spawnedCars = GameObject.FindGameObjectsWithTag("car");
        GameObject[] spawnedVans = GameObject.FindGameObjectsWithTag("car");
        Assert.IsFalse(spawnedCars.Length + spawnedVans.Length == 1);
    }

    [Test]
    public void TestCalculateSpeed()
    {
        GameObject gameManagerObject = new GameObject();
        GameManagerScript gameManager = gameManagerObject.AddComponent<GameManagerScript>();
        gameManager.baseSpeed = 5f;
        gameManager.speedMultiplier = 0.1f;
        gameManager.maxSpeed = 20f;
        gameManager.addPoints(50f);

        float expectedSpeed = Mathf.Clamp(
            gameManager.baseSpeed + (gameManager.score * gameManager.speedMultiplier),
            gameManager.baseSpeed,
            gameManager.maxSpeed
        );

        float actualSpeed = gameManager.CalculateSpeed();

        Assert.AreEqual(expectedSpeed, actualSpeed, 0.01f, "Speed was not calculated correctly!");
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
        MeshRenderer renderer = car.AddComponent<MeshRenderer>();
        Color randomColor = gameManager.GetRandomColor();

        float red = randomColor.r;
        float green = randomColor.g;
        float blue = randomColor.b;

        Debug.Log($"Generated color - R: {red}, G: {green}, B: {blue}");

        Assert.AreNotEqual(0f, red, "Red component should not be 0!");
        Assert.AreNotEqual(0f, green, "Green component should not be 0!");
        Assert.AreNotEqual(0f, blue, "Blue component should not be 0!");

        Assert.AreNotEqual(Color.black, randomColor, "Generated color should not be black!");
    }
}
