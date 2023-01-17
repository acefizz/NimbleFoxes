using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    // To use you have to set the start position and end position. Then place the moving platform where the start is and that's it.
    [SerializeField] float speed = 0.5f;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float changeDirectionDelay;

    private Transform destinationTarget;
    private Transform sourceTarget;

    private float startTime;

    private float journeyLength;

    bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        sourceTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.time;
        journeyLength = Vector3.Distance(sourceTarget.position, destinationTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(sourceTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(DirectionDelay());
            }
        }

    }
    void ChangeDirection()
    {
        if (sourceTarget == endPoint && destinationTarget == startPoint)
        {
            sourceTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            sourceTarget = endPoint;
            destinationTarget = startPoint;
        }
    }
    IEnumerator DirectionDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDirection();
        startTime = Time.time;
        journeyLength = Vector3.Distance(sourceTarget.position, destinationTarget.position);
        isWaiting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }

    }
}