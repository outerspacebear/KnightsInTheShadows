using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScroller : MonoBehaviour
{
    public void MoveToTarget()
    {
        movementTarget = targetTransform.position;
        isMoving = true;
        Debug.Log("Scrolling UI " + gameObject.name + " to target " + movementTarget.ToString());
    }

    public void MoveToOriginalPosition()
    {
        movementTarget = originalPosition;
        isMoving = true;
        Debug.Log("Scrolling UI " + gameObject.name + " to original position " + movementTarget.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = gameObject.transform.position;
        if(targetTransform == null)
        {
            Debug.LogError("No target assigned to UIScroller! Cannot function!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position
                , movementTarget, movementSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, movementTarget) < 0.005f)
            {
                gameObject.transform.position = movementTarget;
                isMoving = false;
            }
        }
    }

    [SerializeField]
    Transform targetTransform;
    Vector3 originalPosition;
    bool isMoving = false;
    Vector3 movementTarget;
    [SerializeField]
    float movementSpeed = 2.0f;
}
