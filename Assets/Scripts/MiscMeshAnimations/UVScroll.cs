using UnityEngine;

public class UVScroll : MonoBehaviour
{
    public string childName = "Baner"; // Name of the child object
    public string materialName = "eye"; // Name of the material
    public float uvXOffset = -0.35f; // Speed to move the UV in the X direction

    private Material targetMaterial; // Cached reference to the material

    void Start()
    {
        // Find the child object
        Transform childTransform = transform.Find(childName);
        if (childTransform == null)
        {
            Debug.LogError($"Child object '{childName}' not found!");
            return;
        }

        // Get the Renderer of the child
        Renderer childRenderer = childTransform.GetComponent<Renderer>();
        if (childRenderer == null)
        {
            Debug.LogError($"Renderer not found on child object '{childName}'!");
            return;
        }

        // Access the material
        foreach (var material in childRenderer.materials)
        {
            if (material.name.Contains(materialName))
            {
                targetMaterial = material;
                break;
            }
        }

        if (targetMaterial == null)
        {
            Debug.LogError($"Material '{materialName}' not found on child object '{childName}'!");
        }
    }

    void FixedUpdate()
    {
        // Ensure the material is found
        if (targetMaterial == null)
            return;

        // Modify the UV offset dynamically
        Vector2 currentOffset = targetMaterial.mainTextureOffset;
        currentOffset.x = uvXOffset; // Update the X offset with speed
        targetMaterial.mainTextureOffset = currentOffset;
    }
}
