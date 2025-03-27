using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;

    // Variables to store previous button states
    private float previousPataValue;
    private float previousPonValue;
    private float previousDonValue;
    private float previousChakaValue;

    private void Awake()
    {
        // Initialize the Input Action class
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the action map
        inputActions.KamiponControls.Enable();
    }

    private void OnDisable()
    {
        // Disable the action map
        inputActions.KamiponControls.Disable();
    }

    private void Update()
    {
        // Update previous states after checking the current states
        previousPataValue = inputActions.KamiponControls.Pata.ReadValue<float>();
        previousPonValue = inputActions.KamiponControls.Pon.ReadValue<float>();
        previousDonValue = inputActions.KamiponControls.Don.ReadValue<float>();
        previousChakaValue = inputActions.KamiponControls.Chaka.ReadValue<float>();
    }

    public bool IsPataPressed()
    {
        float currentPataValue = inputActions.KamiponControls.Pata.ReadValue<float>();
        return previousPataValue == 0 && currentPataValue > 0; // True only when pressed
    }

    public bool IsPonPressed()
    {
        float currentPonValue = inputActions.KamiponControls.Pon.ReadValue<float>();
        return previousPonValue == 0 && currentPonValue > 0; // True only when pressed
    }

    public bool IsDonPressed()
    {
        float currentDonValue = inputActions.KamiponControls.Don.ReadValue<float>();
        return previousDonValue == 0 && currentDonValue > 0; // True only when pressed
    }

    public bool IsChakaPressed()
    {
        float currentChakaValue = inputActions.KamiponControls.Chaka.ReadValue<float>();
        return previousChakaValue == 0 && currentChakaValue > 0; // True only when pressed
    }
}
