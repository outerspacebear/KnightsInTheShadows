using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CharacterFollowerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CharacterSelectedEvent.Get().AddListener(FocusOnCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            float distanceToMove = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);
            if(Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
        else if(isRotating)
        {
            float distanceToMove = rotationSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, targetPosition, distanceToMove);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isRotating = false;
            }
            transform.LookAt(pointInFocus);
        }

        //Begin rotating
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            RotateAroundPointInFocus();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            RotateAroundPointInFocus(true);
        }

        //Begin panning
        else if(Input.GetKey(KeyCode.W))
        {
            PanCamera(Direction.FORWARD);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PanCamera(Direction.LEFT);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PanCamera(Direction.BACK);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            PanCamera(Direction.RIGHT);
        }

        //Focus on character
        else if(Input.GetKeyDown(KeyCode.F))
        {
            if(characterInFocus)
            {
                FocusOnCharacter(characterInFocus);
            }
        }
    }

    void FocusOnCharacter(CCharacter character)
    {
        characterInFocus = character;
        pointInFocus = character.transform.position;
        targetPosition = GetPositionForFocusPointWithAngle(characterInFocus.transform.position
            , anglesInClockwiseOrder[currentAngleIndex]);
        isMoving = true;
    }

    void RotateAroundPointInFocus(bool counterClockwise = false)
    {
        if(pointInFocus == null)
        {
            return;
        }

        UpdateCurrentAngleForRotation(counterClockwise);
        targetPosition = GetPositionForFocusPointWithAngle(pointInFocus, anglesInClockwiseOrder[currentAngleIndex]);
        isRotating = true;
    }

    void UpdateCurrentAngleForRotation(bool counterClockwise = false)
    {
        if(!counterClockwise)
        {
            currentAngleIndex = (currentAngleIndex + 1) % anglesInClockwiseOrder.Length;
        }
        else
        {
            currentAngleIndex = (currentAngleIndex - 1 + anglesInClockwiseOrder.Length) % anglesInClockwiseOrder.Length;
        }
    }

    void PanCamera(Direction direction)
    {
        //Move towards the 1st quadrant
        if(direction == Direction.FORWARD && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_WEST
            || direction == Direction.RIGHT && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_EAST
            || direction == Direction.BACK && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_EAST
            || direction == Direction.LEFT && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_WEST)
        {
            pointInFocus.x += 1;
            pointInFocus.z += 1;
            targetPosition = GetPositionForFocusPointWithAngle(pointInFocus, anglesInClockwiseOrder[currentAngleIndex]);
            isMoving = true;
        }

        //Move towards the 2nd quadrant
        if (direction == Direction.FORWARD && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_EAST
            || direction == Direction.RIGHT && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_EAST
            || direction == Direction.BACK && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_WEST
            || direction == Direction.LEFT && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_WEST)
        {
            pointInFocus.x -= 1;
            pointInFocus.z += 1;
            targetPosition = GetPositionForFocusPointWithAngle(pointInFocus, anglesInClockwiseOrder[currentAngleIndex]);
            isMoving = true;
        }

        //Move towards the 3rd quadrant
        if (direction == Direction.FORWARD && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_EAST
            || direction == Direction.RIGHT && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_WEST
            || direction == Direction.BACK && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_WEST
            || direction == Direction.LEFT && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_EAST)
        {
            pointInFocus.x -= 1;
            pointInFocus.z -= 1;
            targetPosition = GetPositionForFocusPointWithAngle(pointInFocus, anglesInClockwiseOrder[currentAngleIndex]);
            isMoving = true;
        }

        //Move towards the 4th quadrant
        if (direction == Direction.FORWARD && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_WEST
            || direction == Direction.RIGHT && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_WEST
            || direction == Direction.BACK && anglesInClockwiseOrder[currentAngleIndex] == Angle.SOUTH_EAST
            || direction == Direction.LEFT && anglesInClockwiseOrder[currentAngleIndex] == Angle.NORTH_EAST)
        {
            pointInFocus.x += 1;
            pointInFocus.z -= 1;
            targetPosition = GetPositionForFocusPointWithAngle(pointInFocus, anglesInClockwiseOrder[currentAngleIndex]);
            isMoving = true;
        }
    }

    Vector3 GetPositionForFocusPointWithAngle(Vector3 focusPoint, Angle angle)
    {
        Vector3 position = new Vector3();
        position.y = focusPoint.y + verticalSpacingFromCharacter;

        switch(angle)
        {
            case Angle.SOUTH_WEST:
                position.x = focusPoint.x - horizontalSpacingFromCharacter;
                position.z = focusPoint.z - horizontalSpacingFromCharacter;
                break;
            case Angle.SOUTH_EAST:
                position.x = focusPoint.x + horizontalSpacingFromCharacter;
                position.z = focusPoint.z - horizontalSpacingFromCharacter;
                break;
            case Angle.NORTH_EAST:
                position.x = focusPoint.x + horizontalSpacingFromCharacter;
                position.z = focusPoint.z + horizontalSpacingFromCharacter;
                break;
            case Angle.NORTH_WEST:
                position.x = focusPoint.x - horizontalSpacingFromCharacter;
                position.z = focusPoint.z + horizontalSpacingFromCharacter;
                break;
        }

        return position;
    }

    enum Angle
    { SOUTH_WEST, SOUTH_EAST, NORTH_WEST, NORTH_EAST }
    Angle[] anglesInClockwiseOrder = { Angle.SOUTH_WEST, Angle.NORTH_WEST, Angle.NORTH_EAST, Angle.SOUTH_EAST };
    int currentAngleIndex = 0;

    enum Direction
    { FORWARD, BACK, LEFT, RIGHT }

    [SerializeField]
    float horizontalSpacingFromCharacter = 3.0f;
    [SerializeField]
    float verticalSpacingFromCharacter = 4.0f;

    [SerializeField]
    float movementSpeed = 1.0f;
    [SerializeField]
    float rotationSpeed = 1.0f;

    CCharacter characterInFocus;
    Vector3 pointInFocus;

    bool isMoving = false;
    bool isRotating = false;
    Vector3 targetPosition;
}
