using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class UITest
{
    private GameManagerScript gameManager;
    private TitleMusicManager musicManager;
    private Camera mockCamera;
    private TextMeshProUGUI mockScoreText;
    private GameObject canvasObject;
    private GameObject eventSystemObject;

    [SetUp]
    public void SetUp()
    {
        canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        gameManager = new GameObject("GameManager").AddComponent<GameManagerScript>();
        musicManager = new GameObject("MusicManager").AddComponent<TitleMusicManager>();

        mockCamera = new GameObject("MainCamera").AddComponent<Camera>();

        GameObject textObject = new GameObject("ScoreText");
        textObject.transform.SetParent(canvas.transform);
        mockScoreText = textObject.AddComponent<TextMeshProUGUI>();

        gameManager.scoreText = mockScoreText;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManager.gameObject);
        Object.DestroyImmediate(musicManager.gameObject);
        Object.DestroyImmediate(mockCamera.gameObject);
        Object.DestroyImmediate(mockScoreText.gameObject);
        Object.DestroyImmediate(canvasObject);
        Object.DestroyImmediate(eventSystemObject);
    }

    [Test]
    public void TestCameraExists()
    {
        Assert.IsNotNull(gameManager.GetCameraTransform(), "Camera transform should be assigned in GameManager");
    }

    [UnityTest]
    public IEnumerator TestMusicPlaysOnStart()
    {
        musicManager.Start();
        yield return null; 

        Assert.IsTrue(musicManager.GetComponent<AudioSource>().isPlaying, "Title music should play at startup.");
    }

    [UnityTest]
    public IEnumerator TestMusicStopsOnGameOver()
    {
        musicManager.StopTitleMusic();
        yield return null;

        Assert.IsFalse(musicManager.GetComponent<AudioSource>().isPlaying, "Music should stop when game is over.");
    }

    [UnityTest]
    public IEnumerator TestScoreUpdatesCorrectly()
    {
        gameManager.AddPoints(10);
        yield return null; 

        Assert.AreEqual("Score: 10", mockScoreText.text, "Score UI should update correctly.");
    }
}
