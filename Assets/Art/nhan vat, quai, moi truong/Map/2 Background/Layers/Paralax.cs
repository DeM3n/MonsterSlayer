// using UnityEngine;

// public class Paralax : MonoBehaviour
// {
//     public float scrollSpeed = 1f;  // Giá trị dương để di chuyển sang phải
//     private float width;
//     void Start()
//     {
//         Vector3 camPos = Camera.main.transform.position;

//     // Đặt 2 background nối nhau từ trái sang phải theo camera
//     transform.GetChild(0).position = new Vector3(camPos.x, camPos.y, 0);
//     transform.GetChild(1).position = new Vector3(camPos.x + width, camPos.y, 0);
//            SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         if (sr != null)
//             width = sr.bounds.size.x;
//         else
//             width = 10f;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//           transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);

//         // Khi layer ra khỏi màn hình bên phải thì reset về trái
//         if (transform.position.x >= width)
//         {
//             transform.position -= new Vector3(2 * width, 0, 0);
//         }
//     }
// }
// using UnityEngine;

// public class Paralax : MonoBehaviour
// {
//     public float scrollSpeed = 2f;

//     private Transform[] layers = new Transform[2];
//     private float width;
//     private Transform camTransform;

//     void Start()
//     {
//      if (transform.childCount < 2)
//     {
//         Debug.LogError("Paralax requires 2 child objects!");
//         return;
//     }

//     layers[0] = transform.GetChild(0);
//     layers[1] = transform.GetChild(1);

//     SpriteRenderer sr = layers[0].GetComponent<SpriteRenderer>();
//     if (sr == null)
//     {
//         Debug.LogError("SpriteRenderer missing on layer 0");
//         width = 10f; // fallback
//     }
//     else
//     {
//         width = sr.bounds.size.x;
//     }

//     camTransform = Camera.main?.transform;
//     if (camTransform == null)
//     {
//         Debug.LogError("No Camera with tag 'MainCamera' found!");
//     }
//     }

//     void Update()
//     {
//         foreach (Transform layer in layers)
//         {
//             layer.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
//         }

//         // Nếu ảnh đầu tiên đi hết khỏi màn hình bên phải → đưa về sau ảnh thứ 2
//         if (layers[0].position.x - width > camTransform.position.x)
//         {
//             layers[0].position = new Vector3(
//                 layers[1].position.x - width,
//                 layers[0].position.y,
//                 layers[0].position.z
//             );

//             SwapLayers();
//         }
//     }

//     void SwapLayers()
//     {
//         Transform temp = layers[0];
//         layers[0] = layers[1];
//         layers[1] = temp;
//     }
// }
