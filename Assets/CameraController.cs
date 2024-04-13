using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2f;

        // Set the camera's position to the midpoint between the two players. Maintain the default height (y) and z position.
        cam.transform.position = new Vector3(midpoint.x, cam.transform.position.y, cam.transform.position.z);

        // Adjust FOV and lerp to smooth cam movement depending on distance between players.
        float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        cam.fieldOfView = Mathf.Lerp(40, 70, distance / 10f);
    }
}