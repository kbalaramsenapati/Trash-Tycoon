using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    public Camera camera;


    #region PanTouch
    public bool StartPanTouch;
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    // Define the boundaries for panning
    public float panLimitLeft = -10f;
    public float panLimitRight = 10f;
    public float panLimitBottom = -10f;
    public float panLimitTop = 10f;
    public float panLimitForward = -10;
    public float panLimitBackward = 10;
    #endregion

    public Transform TargetHMCharacter;
    public Transform TopView;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartPanTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += direction;

                // Clamp the camera position within the panning limits
                Camera.main.transform.position = new Vector3(
                    Mathf.Clamp(Camera.main.transform.position.x, panLimitLeft, panLimitRight),
                    Mathf.Clamp(Camera.main.transform.position.y, panLimitBottom, panLimitTop),
                    Mathf.Clamp(Camera.main.transform.position.z, panLimitForward, panLimitBackward)
                );
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    public void OrthographicCameraLocateOn(int num,int TempZoom)
    {
        //camera.orthographicSize = 50;
        LeanTween.value(camera.orthographicSize, TempZoom, 1).setOnUpdate(orthographicSizeCamera);
        switch (num)
        {
            case 0:
                LeanTween.move(camera.gameObject, new Vector3(-80, 75, 65), 1f);
                //camera.transform.position = new Vector3(-80, 75, 65);
                break;
            case 1:
                LeanTween.move(camera.gameObject, new Vector3(0, 65, 100), 1);
                //camera.transform.position = new Vector3(0, 65, 100);
                break;
            case 2:
                LeanTween.move(camera.gameObject, new Vector3(0, 100, 50), 1);
                //camera.transform.position = new Vector3(0, 100, 50);
                break;

        }
    }
    public void orthographicSizeCamera(float v)
    {
        camera.orthographicSize= v;
    }
    public void OnCameraLocatToNPC()
    {
        camera.orthographic = false;
        camera.fieldOfView = 60;

        LeanTween.move(camera.gameObject, TargetHMCharacter, 1);
        LeanTween.rotate(camera.gameObject, TargetHMCharacter.eulerAngles, 1);
    }
    public async void GotoTopView()
    {
        StartPanTouch = true;
        camera.orthographic = true;

        LeanTween.move(camera.gameObject, TopView, 1);
        LeanTween.rotate(camera.gameObject, TopView.eulerAngles, 1);
        //await Task.Delay(1 * 1000);
        //OrthographicCameraLocateOn(0, 50);
    }
}
