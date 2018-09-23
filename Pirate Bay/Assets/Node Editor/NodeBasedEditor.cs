using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeBasedEditor : EditorWindow
{
    private EntryNode entryNode;

    private List<DialogueNode> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Node Based Editor");
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        GUIStyle tempStyle = new GUIStyle();
        tempStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;

        GUIStyle tempSelectedStyle = new GUIStyle();
        tempSelectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3 on.png") as Texture2D;

        entryNode = new EntryNode(new Vector2(10, 70), 50, 50, tempStyle, tempSelectedStyle, null, outPointStyle, OnClickOutPoint);
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();
        DrawButtons();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawButtons()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Save"))
        {
            SaveConversation();
        }
    }

    private void DrawNodes()
    {
        entryNode.Draw();

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        entryNode.ProcessEvents(e);

        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Scripted Dialogue"), false, () => OnClickAddScriptedDialogue(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Typed Dialogue"), false, () => OnClickAddTypedDialogue(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Options Dialogue"), false, () => OnClickAddOptionsDialogue(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        entryNode.Drag(delta);

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddScriptedDialogue(Vector2 mousePosition)
    {
        if(nodes == null)
        {
            nodes = new List<DialogueNode>();
        }

        nodes.Add(new ScriptedDialogueNode(mousePosition, 400, 50, nodeStyle, selectedNodeStyle, OnClickRemoveNode, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
    }

    private void OnClickAddTypedDialogue(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<DialogueNode>();
        }

        nodes.Add(new TypedDialogueNode(mousePosition, 125, 75, nodeStyle, selectedNodeStyle, OnClickRemoveNode, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
    }

    private void OnClickAddOptionsDialogue(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<DialogueNode>();
        }

        nodes.Add(new OptionsDialogueNode(mousePosition, 400, 75, nodeStyle, selectedNodeStyle, OnClickRemoveNode, inPointStyle, OnClickInPoint, outPointStyle, OnClickOutPoint, RemoveConnections));
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        RemoveConnections(node);

        nodes.Remove((DialogueNode)node);
    }

    private void RemoveConnections(Node node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (node.ContainsConnection(connections[i].inPoint) || node.ContainsConnection(connections[i].outPoint))
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void SaveConversation()
    {
        TreeList<IDialogueContext> dialogueTree = new TreeList<IDialogueContext>();
        DialogueNode currentNode = (DialogueNode)GetConnectedNode(entryNode);
        Queue<DialogueNode> nodesToVist = new Queue<DialogueNode>();
        List<IDialogueContext> contexts = new List<IDialogueContext>(nodes.Count);
        List<int> childCounts = new List<int>(nodes.Count);
        nodesToVist.Enqueue(currentNode);
        while (nodesToVist.Count > 0)
        {
            DialogueNode tmp = nodesToVist.Dequeue();
            VisitNode(tmp, dialogueTree, contexts, childCounts);
            if (tmp.HasInternalChildren())
            {
                Node[] children = tmp.GetInternalChildren();
                foreach(Node node in children)
                {
                    DialogueNode connectedNode = (DialogueNode)GetConnectedNode(node);
                    if(connectedNode != null)
                    {
                        nodesToVist.Enqueue(connectedNode);
                    }
                    continue;
                }
            }
            else
            {
                DialogueNode connectedNode = (DialogueNode)GetConnectedNode(tmp);
                if(connectedNode != null)
                {
                    nodesToVist.Enqueue(connectedNode);
                    continue;
                }
            }
        }

        ConversationLayout asset = CreateInstance<ConversationLayout>();
        asset.Init("Test Conversation!", contexts, childCounts);

        AssetDatabase.CreateAsset(asset, "Assets/ConversationLayouts/NewConversationLayout.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

    }

    private Node GetConnectedNode(Node currentNode)
    {
        foreach(Connection conn in connections)
        {
            if (conn.Contains(currentNode)){
                return conn.inPoint.node;
            }
        }

        return null;
    }

    private void VisitNode(DialogueNode node, TreeList<IDialogueContext> tree, List<IDialogueContext> contexts, List<int> childCounts)
    {
        IDialogueContext context = node.GetDialogueContext();
        int childCount = (node.HasInternalChildren() ? node.GetInternalChildCount() : 1);
        contexts.Add(context);
        childCounts.Add(childCount);
        tree.Insert(new TreeNode<IDialogueContext>(context, childCount));
    }
}
