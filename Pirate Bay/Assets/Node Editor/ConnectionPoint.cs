using System;
using UnityEngine;
using UnityEditor;

public enum ConnectionPointType { In, Out }

[System.Serializable]
public class ConnectionPoint
{
    public Node node;

    public Rect rect;

    public ConnectionPointType type;

    private GUIStyle style;

    private Action<ConnectionPoint> OnClickConnectionPoint;

    public Vector2 offset;

    public ConnectionPoint(Node node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        rect = new Rect(0, 0, 10f, 20f);

        style = new GUIStyle();
        style.border = new RectOffset(4, 4, 12, 12);

        switch (type)
        {
            case ConnectionPointType.In:
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                break;
            case ConnectionPointType.Out:
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                break;
        }

        this.OnClickConnectionPoint = OnClickConnectionPoint;
    }

    public ConnectionPoint(Node node, Vector2 offset, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.offset = offset;
        rect = new Rect(0, 0, 10f, 20f);

        style = new GUIStyle();
        style.border = new RectOffset(4, 4, 12, 12);

        switch (type)
        {
            case ConnectionPointType.In:
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                break;
            case ConnectionPointType.Out:
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                break;
        }

        this.OnClickConnectionPoint = OnClickConnectionPoint;
    }

    public void AutoDraw()
    {
        rect.y = node.rect.y + offset.y;
    
        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }

    }

    public void Draw()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}