using System;
using UnityEditor;
using UnityEngine;

public abstract class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    protected GUIStyle titleStyle;
    protected Rect titleRect;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode)
    {
        title = "Node";
        titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 14;
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public virtual void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public virtual void Draw()
    {
        titleRect = rect;
        titleRect.y = rect.y - 10;
        GUI.Label(titleRect, new GUIContent(title), titleStyle);
        GUI.Box(rect, "", style);
    }

    public virtual bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    protected virtual void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }

    public abstract bool ContainsConnection(ConnectionPoint point);
}