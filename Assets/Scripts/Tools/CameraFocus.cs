using UnityEngine;
using System.Collections;

public class CameraFocus : MonoBehaviour
{
    public Transform targetObject; // The target object to focus on
    public float focusSpeed = 2.0f; // Speed of the focus transition
    public Vector3 offset = new Vector3(0, 2, -5); // Offset from the target object

    private Camera mainCamera;
    public bool isFocusing = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFocusing)
        {
            StartCoroutine(FocusOnTarget());
        }
    }

    IEnumerator FocusOnTarget()
    {
        isFocusing = true;

        Vector3 initialPosition = mainCamera.transform.position;
        Quaternion initialRotation = mainCamera.transform.rotation;

        Vector3 targetPosition = targetObject.position + offset;
        Quaternion targetRotation = Quaternion.LookRotation(targetObject.position - mainCamera.transform.position);

        float elapsedTime = 0.0f;

        while (elapsedTime < focusSpeed)
        {
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / focusSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / focusSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;

        isFocusing = false;

    }
}
