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

    // Used in auto draw.
    public Node parent;

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

    // Use this constructor if you dont want the connection node to use the rect of the node.
    public ConnectionPoint(Node node, Node parent, Vector2 offset, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.parent = parent;
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
        if (parent == null) return;

        rect.y = parent.rect.y + offset.y;
    
        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = parent.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = parent.rect.x + parent.rect.width - 8f;
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