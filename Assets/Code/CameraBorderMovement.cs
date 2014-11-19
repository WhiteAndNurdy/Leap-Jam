using UnityEngine;
using System.Collections;

public class CameraBorderMovement : MonoBehaviour 
{
    public float horizontalBorderTriggerPercentage = 0.1f;
    public float verticalBorderTriggerPercentage = 0.1f;

    public float maxSpeed = 10.0f;

    //These are in world space
    public float maxLeftCameraPos;
    public float maxRightCameraPos;
    public float maxTopCameraPos;
    public float maxBottomCameraPos;

	void Update () 
    {
        var mouseX = Input.mousePosition.x;
        var mouseY = Input.mousePosition.y;
        float speedMultiplier = 0.0f;

        Vector3 rightVec = new Vector3(1,0,0);
        Vector3 upVec = new Vector3(0,0,1);

        //Right
        float rightBorderPos = Screen.width * (1 - horizontalBorderTriggerPercentage);
        if (mouseX >= rightBorderPos)
        {
            speedMultiplier = (mouseX - rightBorderPos) / (Screen.width - rightBorderPos);
            if (transform.position.x < maxRightCameraPos)
            {
                transform.position += rightVec * maxSpeed * speedMultiplier * Time.deltaTime;
            }
        }
        //Left
        else if (mouseX <= Screen.width * horizontalBorderTriggerPercentage)
        {
            speedMultiplier = 1 - (mouseX / (Screen.width * horizontalBorderTriggerPercentage));
            if (transform.position.x > maxLeftCameraPos)
            {
                transform.position -= rightVec * maxSpeed * speedMultiplier * Time.deltaTime;
            }
        }

        //Bottom
        float bottomBorderPos = Screen.height * (1 - verticalBorderTriggerPercentage);
        if (mouseY >= bottomBorderPos)
        {
            speedMultiplier = (mouseY - bottomBorderPos) / (Screen.height - bottomBorderPos);
            if (transform.position.z < maxTopCameraPos)
            {
                transform.position += upVec * maxSpeed * speedMultiplier * Time.deltaTime;
            }
        }
        //Up
        else if (mouseY <= Screen.height * verticalBorderTriggerPercentage)
        {
            speedMultiplier = 1 - (mouseY  / (Screen.height * verticalBorderTriggerPercentage));
            if (transform.position.z > maxBottomCameraPos)
            {
                transform.position -= upVec * maxSpeed * speedMultiplier * Time.deltaTime;
            }
        }
	}
}
