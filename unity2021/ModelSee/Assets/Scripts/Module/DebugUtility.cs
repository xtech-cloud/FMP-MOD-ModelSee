using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUtility : MonoBehaviour
{
    public Camera renderCamera { get; set; }
    public Transform pivot { get; set; }

    public Text debugPosX;
    public Text debugPosY;
    public Text debugPosZ;
    public Text debugRotX;
    public Text debugRotY;
    public Text debugRotZ;
    public Text debugPivotX;
    public Text debugPivotY;
    public Text debugPivotZ;

    public float moveSpeed { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (null == renderCamera)
            return;

        if (Input.GetKey(KeyCode.W))
            renderCamera.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetKey(KeyCode.S))
            renderCamera.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.A))
            renderCamera.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetKey(KeyCode.D))
            renderCamera.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.Q))
            renderCamera.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        else if (Input.GetKey(KeyCode.E))
            renderCamera.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.I))
            pivot.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetKey(KeyCode.K))
            pivot.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.J))
            pivot.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetKey(KeyCode.L))
            pivot.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.U))
            pivot.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        else if (Input.GetKey(KeyCode.O))
            pivot.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);


        renderCamera.transform.LookAt(pivot);

        if (Input.GetKey(KeyCode.Alpha1))
        {
            resetPosX();
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            resetPosY();
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            resetPosZ();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            resetPivotX();
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            resetPivotY();
        }

        if (Input.GetKey(KeyCode.Alpha6))
        {
            resetPivotZ();
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            resetPosX();
            resetPosY();
            resetPosZ();
            resetPivotX();
            resetPivotY();
            resetPivotZ();
            renderCamera.transform.localRotation = Quaternion.identity;
        }

        debugPosX.text = string.Format("{0:F2}", renderCamera.transform.localPosition.x);
        debugPosY.text = string.Format("{0:F2}", renderCamera.transform.localPosition.y);
        debugPosZ.text = string.Format("{0:F2}", renderCamera.transform.localPosition.z);
        debugRotX.text = string.Format("{0:F2}", renderCamera.transform.localRotation.eulerAngles.x);
        debugRotY.text = string.Format("{0:F2}", renderCamera.transform.localRotation.eulerAngles.y);
        debugRotZ.text = string.Format("{0:F2}", renderCamera.transform.localRotation.eulerAngles.z);
        debugPivotX.text = string.Format("{0:F2}", pivot.localPosition.x);
        debugPivotY.text = string.Format("{0:F2}", pivot.localPosition.y);
        debugPivotZ.text = string.Format("{0:F2}", pivot.localPosition.z);
    }

    private void resetPosX()
    {
        Vector3 position = renderCamera.transform.localPosition;
        position.x = 0;
        renderCamera.transform.localPosition = position;
    }

    private void resetPosY()
    {
        Vector3 position = renderCamera.transform.localPosition;
        position.y = 0;
        renderCamera.transform.localPosition = position;
    }

    private void resetPosZ()
    {
        Vector3 position = renderCamera.transform.localPosition;
        position.z = 0;
        renderCamera.transform.localPosition = position;
    }
    private void resetPivotX()
    {
        Vector3 position = pivot.localPosition;
        position.x = 0;
        pivot.localPosition = position;
    }

    private void resetPivotY()
    {
        Vector3 position = pivot.localPosition;
        position.y = 0;
        pivot.localPosition = position;
    }

    private void resetPivotZ()
    {
        Vector3 position = pivot.localPosition;
        position.z = 0;
        pivot.localPosition = position;
    }
}
