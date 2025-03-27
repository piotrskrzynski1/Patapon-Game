using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject hatapon;
    private Vector3 hataponPos;
    public Vector3 offset;
    void Start()
    {
        if (hatapon == null)
        {
            Debug.LogError("Hatapon nie ustawiony");
        }
        else
        {
            hataponPos = hatapon.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hataponPos = hatapon.transform.position;
        transform.position = Vector3.Lerp(transform.position,hataponPos + offset,Time.deltaTime);
    }
}
