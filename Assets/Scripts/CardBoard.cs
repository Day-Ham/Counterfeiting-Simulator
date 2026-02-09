using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBoard : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float changeDirectional = 1f;
    [SerializeField] private float minHeight = -2f;
    [SerializeField] private float maxHeight = 5f;

    private float currentDirectional = 1f;
    private float timeSinceLastChange = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("Cardboard test");
        currentDirectional = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        HandleMovementOfCard();
    }

    void HandleMovementOfCard()
    {
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= changeDirectional)
        {
            currentDirectional = Random.value > 0.5f ? 1f : -1f;
            timeSinceLastChange = 0f;
        }

        if (transform.position.y >= maxHeight && currentDirectional > 0)
        {
            currentDirectional = -1f;
        }
        else if (transform.position.y <= minHeight && currentDirectional < 0)
        {
            currentDirectional = 1f;
        }

        Vector3 movement = new Vector3(0, currentDirectional * moveSpeed * Time.deltaTime, 0);
        rb.MovePosition(rb.position + movement);
    }
}