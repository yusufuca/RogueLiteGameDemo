using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;

    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.forward);
    }

}
