using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject bodyPrefab;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float steerSpeed = 150f;  // Increased for smoother turning
    [SerializeField] private int bodyPartDistance = 5; // Adjusted to a lower value for better spacing
    [SerializeField] private float bodySpeed = 10f;

    private List<GameObject> bodyPartList = new List<GameObject>();
    private List<Vector3> positionHistoryList = new List<Vector3>();

    void Start()
    {
        // Initialize the snake with body parts
        for (int i = 0; i < 5; i++)
        {
            GrowBody();
        }


    }

    void Update()
    {
        MoveForward();
        SteerSnake();

        if (Input.GetKeyDown(KeyCode.E))
        {
            GrowBody();
        }

        // Insert the current position at the front of the history list
        positionHistoryList.Insert(0, (transform.position - (Vector3.up * 0.15f))); // y offset for body
       // positionHistoryList.Insert(0, transform.position);

        // Move each body part to follow the head
        for (int i = 0; i < bodyPartList.Count; i++)
        {
            Vector3 point = positionHistoryList[Mathf.Min(i * bodyPartDistance, positionHistoryList.Count - 1)];
            GameObject body = bodyPartList[i];

            // Smoothly move the body part towards the target point
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * Time.deltaTime * bodySpeed;

            // Make the body part face the direction it's moving
            body.transform.LookAt(point);
        }

        //  Limit the size of the position history to prevent memory overflow
        int maxHistorySize = bodyPartList.Count * bodyPartDistance;
        if (positionHistoryList.Count > maxHistorySize)
        {
            positionHistoryList.RemoveAt(positionHistoryList.Count - 1);
        }
    }

    private void SteerSnake()
    {
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);
    }

    private void MoveForward()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void GrowBody()
    {
        GameObject body = Instantiate(bodyPrefab);
        // Place the new body part at the last position of the snake or on the head if it's the first one
        //  body.transform.position = bodyPartList.Count == 0 ? transform.position : bodyPartList[bodyPartList.Count - 1].transform.position;
        bodyPartList.Add(body);
    }
}
