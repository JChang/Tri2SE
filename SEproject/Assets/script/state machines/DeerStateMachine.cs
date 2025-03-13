using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerStateMachine : MonoBehaviour
{
    private IDeerState deerState;

    public Rigidbody rb;

    public Transform t;

    public bool IsGrounded { get; set; } = true;

    private Animator animator;
    private GameManagerScript gameManager;

    [Header("Audio")]
    public AudioClip[] keyPressSounds;
    public float[] soundWeights;
    public AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        deerState = new DeerWalk(this);
        animator = transform.Find("Deer_001").GetComponent<Animator>();
        gameManager = new GameManagerScript();
    }
    public void setState(IDeerState d)
    {
        deerState = d;
    }

    void Update()
    {
        // Debug.Log(rb.velocity.magnitude);
        animator.SetFloat("speed", rb.velocity.magnitude);
        if (Input.GetKey(KeyCode.W)) deerState.handleForward();
        if (Input.GetKey(KeyCode.A)) deerState.handleLeft();
        if (Input.GetKey(KeyCode.D)) deerState.handleRight();
        if (Input.GetKey(KeyCode.S)) deerState.handleBack();
        if (Input.GetKey(KeyCode.Space)) deerState.handleSpace();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            deerState.handleShift();
            animator.SetBool("shift", true);
        }
        else
        {
            animator.SetBool("shift", false);
        }
        // this.transform.rotation = gameManager.camRotation;
        deerState.handleGravity();
        deerState.advanceState();

        if (Input.GetKeyDown(KeyCode.R)) { PlayRandomSound(); }
    }

    void OnCollisionEnter(Collision c)
    {
        IsGrounded = true;
    }

    void PlayRandomSound()
    {
        if (keyPressSounds.Length > 0 && soundWeights.Length == keyPressSounds.Length)
        {
            int selectedIndex = GetWeightedRandomIndex(soundWeights);
            audioSource.clip = keyPressSounds[selectedIndex];
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
