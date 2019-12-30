using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PortalView : MonoBehaviour {
    private Mesh quad = null;

	// Use this for initialization
	void Start () {
        quad = GetComponent<MeshFilter>().mesh;
	}

    /* Changes the UV of quad such that only a corner of the complete view is shown.
     * This corner has center (x, y) and is part of a rectangle with dimensions (w, h).
     * The complete rectangle has center (0, 0).
    */
    public void UpdateUV(float x, float y, float w, float h)
    {
        float left = 0.0f;
        float right = 1.0f;
        float bottom = 0.0f;
        float up = 1.0f;
        
        if (x > 0)
        {
            left = (2 * x) / w;
            right = 1.0f;
        }
        else if (x < 0)
        {
            left = 0.0f;
            right = (2 * x) / w + 1.0f;
        }

        
        if (y > 0)
        {
            bottom = (2 * y) / h;
            up = 1.0f;
        }
        else if(y < 0)
        {
            bottom = 0.0f;
            up = (2 * y) / h + 1.0f; ;
        }

        Vector2[] uv = quad.uv;
        uv[0].x = left;
        uv[0].y = bottom;
        uv[1].x = right;
        uv[1].y = up;
        uv[2].x = right;
        uv[2].y = bottom;
        uv[3].x = left;
        uv[3].y = up;

        quad.SetUVs(0,new List<Vector2>(uv));
    }
    
}
