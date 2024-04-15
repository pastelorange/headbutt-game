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

        // // Check if players are off screen
        // // The renderer is actually a SkinnedMeshRenderer and in a child object of the player object
        // SkinnedMeshRenderer renderer1 = player1.GetComponentInChildren<SkinnedMeshRenderer>();
        // SkinnedMeshRenderer renderer2 = player2.GetComponentInChildren<SkinnedMeshRenderer>();

        // // Check if each game object is visible
        // if (!IsVisibleFrom(renderer1, Camera.main))
        // {
        //     player1.GetComponent<PlayerController>().KnockOut();
        // }
        // if (!IsVisibleFrom(renderer2, Camera.main))
        // {
        //     player2.GetComponent<PlayerController>().KnockOut();
        // }

    }

    // Check if a game object is visible from a specific camera
    public bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}