using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalCam : MonoBehaviour {

    public Transform player = null;
    public Transform portalEntry = null;
    public Transform portalExit = null;
    private Camera cam = null;
    private PortalView pv = null;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();

        pv = portalEntry.GetComponent<PortalView>();
    }
    
    void Update () {
        #region Get the relative position of the player from the entrance portal
        Vector3 oldPlayerScale = player.localScale;
        Transform parent = player.parent;
        //set entrance portal as parent of player to get relative positions
        player.SetParent(portalEntry);

        //get relative positions of player from portal entrance
        float x = player.localPosition.x;
        float y = player.localPosition.y;
        float z = player.localPosition.z;

        //place player back in the hierarchy were it belongs
        player.SetParent(parent);
        //ensure that the scale of player doesn't change
        player.localScale = oldPlayerScale;
        #endregion

        #region Set the position of the camera relative to the exit portal
        Vector3 oldTransformScale = transform.localScale;
        parent = transform.parent;
        transform.SetParent(portalExit);

        //set the position of the camera on the back side of the exit portal
        transform.localPosition = new Vector3(-x, y, -z);
        //keep the orientation of the camera perpendicular to the exit portal
        transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        
        //get the true relative positions of the camera from the exit portal
        x *= transform.lossyScale.x;
        y *= transform.lossyScale.y;
        z *= transform.lossyScale.z;

        transform.SetParent(parent);
        //ensure that the scale of camera does not changing parents back and forth
        transform.localScale = oldTransformScale;

        #endregion

        #region Set the camera fov such that it fully captures the exit portal (and more)
        //calculate the width and height of the rectangle centered in the camera view and fully contains the portal exit
        float width = 2.0f * Mathf.Abs(x) + transform.lossyScale.x;
        float height = 2.0f * Mathf.Abs(y) + transform.lossyScale.y;

        //|x| + 0.5 is the horizontal distance from the border of the portal
        //|z| is the perpendicular distance from the portal
        //float angleh = 2 * Mathf.Atan((0.5f * width) / Mathf.Abs(z));
        //|y| + 0.5 is the vertical distance from the border of the portal
        //|z| is the perpendicular distance from the portal
        float anglev = 2 * Mathf.Atan((0.5f * height) / Mathf.Abs(z));

        //set the aspect ratio of the camera
        //cam.aspect = Mathf.Tan(0.5f * angleh) / Mathf.Tan(0.5f * anglev);
        cam.aspect = width / height;
        //set the fov of the camera
        cam.fieldOfView = anglev * 180.0f / Mathf.PI;
        #endregion

        #region Only use part of the render texture that captures the exit portal
        //|x| + 0.5 is half the width of the whole rectangle
        //|y| + 0.5 is half the height of the whole rectangle
        pv.UpdateUV(-x, -y, width, height);
        #endregion
        
    }
}
