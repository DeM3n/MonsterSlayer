using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBound = -10f;
    public float rightBound = 10f;

    private bool isMoving = true;
    private PlayerMovement player;

    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.canMove = false;
        }

        Vector3 startPos = transform.position;
        startPos.x = rightBound;
        transform.position = startPos;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x <= leftBound)
            {
                transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
                isMoving = false;

                if (player != null)
                {
                    player.canMove = true;
                }
            }
        }
    }
}