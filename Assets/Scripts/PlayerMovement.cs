using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
   
    private float moveInput;

    void Start()
    {
      
    }

    void Update()
    {
       
        moveInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);

      
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(moveInput, 1f, 1f);
        }
    }

   
}