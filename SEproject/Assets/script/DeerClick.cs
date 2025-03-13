using UnityEngine;

public class DeerClick : MonoBehaviour
{
    public AudioClip[] clickSounds;
    public float[] soundWeights;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnMouseDown()
    {
        if (clickSounds.Length > 0 && soundWeights.Length == clickSounds.Length)
        {
            int selectedIndex = GetWeightedRandomIndex(soundWeights);
            audioSource.clip = clickSounds[selectedIndex];
            audioSource.Play();
        }
    }

    int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return 0;
    }
}
