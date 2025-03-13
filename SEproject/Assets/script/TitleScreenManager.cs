using UnityEngine;
using TMPro;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private string originalText;

    void Start()
    {
        originalText = titleText.text;
    }

    public void ShowSettingsMessage()
    {
        titleText.text = "lmao you thought";
        StartCoroutine(RevertTextAfterDelay(3f));
    }

    private IEnumerator RevertTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        titleText.text = originalText;
    }
}
