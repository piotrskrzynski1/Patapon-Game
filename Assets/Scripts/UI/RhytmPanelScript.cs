using UnityEngine;
using UnityEngine.UI;
public class RhytmPanelScript : MonoBehaviour
{
    private Image imageComponent;
    public float FadeBoost = 1.0f;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (imageComponent != null)
        {
            Color currentColor = imageComponent.color;

            currentColor.a -= Time.deltaTime * FadeBoost;

            currentColor.a = Mathf.Clamp01(currentColor.a);

            imageComponent.color = currentColor;
        }
    }

}
