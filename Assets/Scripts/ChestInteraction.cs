using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false;
    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    void Start()
    {
        animator = GetComponent<Animator>();
         animator.SetBool("isOpen", false);
    }

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
               Debug.Log("Đang cố mở rương - keyCount hiện tại: " + playerInventory.keyCount);
         if (playerInventory != null && playerInventory.UseKey())
            {
                animator.SetBool("isOpen", true);
                isOpened = true;
                Debug.Log("Rương đã mở bằng chìa!");
            }
            else
            {
                Debug.Log("Bạn không có đủ chìa để mở rương!");
            }
        }     
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Va chạm với: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerInventory>();
            playerInRange = true;

            if (playerInventory != null)
                Debug.Log("Đã tìm thấy PlayerInventory");
            else
                Debug.Log("Không có PlayerInventory");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}
