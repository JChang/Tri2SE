using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarBehavior : MonoBehaviour
{
    [Header("Car AI")]
    public CarAI carAI = CarAI.Direct;
    public enum CarAI { Drunk, Direct, Intercept, Overshoot, Swerve, Blockade };

    [Header("Components")]
    NavMeshAgent enemy;
    GameObject player;
    public ParticleSystem explosion;
    public AudioClip explosionSound;
    private AudioSource audioSource;

    [Header("Swerve AI")]
    private float swerveDirection = 1f;
    private float swerveTimer = 0f;
    public float swerveInterval = 1f;
    public float swerveAmount = 5f;

    [Header("Blockade AI")]
    private bool isBlocking = false;
    public float blockadeDistance = 20f;
    public float reBlockadeDistance = 30f;

    [Header("References")]
    public GameManagerScript gameManager;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        carAI = (CarAI)Random.Range(0, System.Enum.GetValues(typeof(CarAI)).Length);
        gameManager = FindObjectOfType<GameManagerScript>();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (carAI == CarAI.Drunk)
        {
            carAI = (CarAI)Random.Range(1, System.Enum.GetValues(typeof(CarAI)).Length);
        }

        switch (carAI)
        {
            case CarAI.Direct:
                Direct();
                break;
            case CarAI.Intercept:
                Intercept();
                break;
            case CarAI.Overshoot:
                Overshoot();
                break;
            case CarAI.Swerve:
                Swerve();
                break;
            case CarAI.Blockade:
                Blockade();
                break;
        }
    }

    private void Direct()
    {
        enemy.SetDestination(player.transform.position);
    }

    private void Intercept()
    {
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 playerPosition = player.transform.position;
        Vector3 carPosition = transform.position;

        if (playerVelocity.magnitude > 0)
        {
            float timeToIntercept = Vector3.Distance(carPosition, playerPosition) / enemy.speed;
            Vector3 predictedPosition = playerPosition + playerVelocity * timeToIntercept;
            enemy.SetDestination(predictedPosition);
        }
        else
        {
            enemy.SetDestination(playerPosition);
        }
    }

    void Overshoot()
    {
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 playerPosition = player.transform.position;
        Vector3 carPosition = transform.position;

        if (playerVelocity.magnitude > 0)
        {
            Vector3 overshootDirection = playerVelocity.normalized;
            float overshootDistance = 10f;
            Vector3 overshootPosition = playerPosition + overshootDirection * overshootDistance;

            enemy.SetDestination(overshootPosition);
        }
        else
        {
            enemy.SetDestination(playerPosition);
        }
    }

    void Swerve()
    {
        swerveTimer += Time.deltaTime;

        if (swerveTimer >= swerveInterval)
        {
            swerveTimer = 0f;
            swerveDirection *= -1f;
        }

        Vector3 targetPosition = player.transform.position;
        Vector3 swerveOffset = transform.right * swerveDirection * swerveAmount;
        enemy.SetDestination(targetPosition + swerveOffset);
    }

    void Blockade()
    {
        if (!isBlocking)
        {
            Vector3 blockadePosition = player.transform.position + player.GetComponent<Rigidbody>().velocity.normalized * blockadeDistance;
            enemy.SetDestination(blockadePosition);
            isBlocking = true;
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer >= reBlockadeDistance)
            {
                isBlocking = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "car" || collision.gameObject.tag == "Obstacle")
        {
            Renderer carRenderer = GetComponentInChildren<Renderer>();
            if (carRenderer != null)
            {
                carRenderer.material.color = Color.black;
            }

            enemy.isStopped = true;
            enemy.velocity = Vector3.zero;

            audioSource.volume = 0.75f;
            audioSource.PlayOneShot(explosionSound);
            Instantiate(explosion, transform.position, Quaternion.identity);

            StartCoroutine(DestroyCarAfterDelay(3f));
        }
    }

    private IEnumerator DestroyCarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        if (enemy != null)
        {
            enemy.speed = speed;
        }
    }

    public float getSpeed()
    {
        return enemy.speed = gameManager.baseSpeed + (gameManager.score * gameManager.speedMultiplier);
    }
}