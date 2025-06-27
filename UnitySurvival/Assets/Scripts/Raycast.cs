using UnityEngine;
using UnityEngine.InputSystem;

public class Raycast : MonoBehaviour
{

    [Header("Raycast Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionMask;

    [Header("Input")]
    public InputActionReference interactionKey;

    private void OnEnable()
    {
        if (interactionKey != null)
            interactionKey.action.performed += OnInteractPerformed;

    }

    private void OnDisable()
    {
        if (interactionKey != null)
            interactionKey.action.performed -= OnInteractPerformed;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {

        Ray rayCast = new Ray(transform.position, transform.forward);
        RaycastHit rayCastHit;

        if (Physics.Raycast(rayCast, out rayCastHit, interactionDistance, interactionMask))
        {
            InteractableObject isInteractable = rayCastHit.collider.GetComponent<InteractableObject>();
            if (isInteractable != null)
                isInteractable.OnInteract();

        }

    }
}