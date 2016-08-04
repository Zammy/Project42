using UnityEngine;
using System.Collections;

public static class CameraExt 
{
     public static Bounds CalcOrthographicBounds(this Camera camera)
     {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        var pos = camera.transform.position;
        pos.z = 0f;
        Bounds bounds = new Bounds( pos, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
     }
}
