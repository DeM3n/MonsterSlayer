using UnityEngine;

public class TileParalax : MonoBehaviour

{
    public float speed = 2f;
    public float repeatWidth = 20f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
        }
    }
}