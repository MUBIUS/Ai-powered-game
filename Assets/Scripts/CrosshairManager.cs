using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage;  // Reference to the crosshair UI image
    public Color defaultColor = Color.white;
    public Color enemyDetectedColor = Color.red;

    public float detectionRange = 100f;
    public LayerMask enemyLayer;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        crosshairImage.color = defaultColor;  // Set crosshair to default color at start
    }

    private void Update()
    {
        UpdateCrosshairColor();
    }

    private void UpdateCrosshairColor()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionRange, enemyLayer))
        {
            // If the crosshair is pointed at an enemy, change the color to red
            crosshairImage.color = enemyDetectedColor;
        }
        else
        {
            // Otherwise, reset to the default color
            crosshairImage.color = defaultColor;
        }
    }
}
