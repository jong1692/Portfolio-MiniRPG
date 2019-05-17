using UnityEngine;
using System.Collections;


public class GUIManager : MonoBehaviour
{

    public RenderTexture MiniMapTexture;
    public Material MiniMapMaterial;
    private float offset;

    void Awake()
    {
        offset = 10;

    }

    void OnGUI()
    {

        Rect Map_Rectangle = new Rect(Screen.width - (0.2f * Screen.width), 0.05f * Screen.height,
            0.18f * Screen.width, 0.28f * Screen.height);

        if (Event.current.type == EventType.Repaint)
        {
            Graphics.DrawTexture(Map_Rectangle, MiniMapTexture, MiniMapMaterial);
        }
    }
}
