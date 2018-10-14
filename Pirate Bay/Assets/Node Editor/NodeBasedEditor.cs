using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class NodeBasedEditor : EditorWindow
{
    private EntryNode entryNode;

    private List<ConnectionNode> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;
    private GUIStyle titleStyle;

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
        titleStyle = GUIStyle.none;
        titleStyle.normal.background = null;
        titleStyle.active.background = null;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.fontSize = 32;
        titleStyle.fontStyle = FontStyle.Bold;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        GUIStyle tempStyle = new GUIStyle();
        tempStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;

        GUIStyle tempSelectedStyle = new GUIStyle();
        tempSelectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3 on.png") as Texture2D;

        entryNode = new EntryNode(new Vector2(10, 70), 50, 50, tempStyle, tempSelectedStyle, null, OnClickConnectionPoint);
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();
        DrawOverlay();

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

    string convoTitle = "New Conversation";

    private void DrawOverlay()
    {
        GUILayout.Space(20);
        convoTitle = GUILayout.TextField(convoTitle, titleStyle);

        if (GUI.Button(new Rect(Screen.width - 225, Screen.height - 100, 200, 50), "Save"))
        {
            SaveDialogueTree();
        }

        if(GUI.Button(new Rect(Screen.width - 450, Screen.height - 100, 200, 50), "Load"))
        {
            LoadDialogueTree();
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
            nodes = new List<ConnectionNode>();
        }

        nodes.Add(CreateScriptedDialogueNode(mousePosition));
    }

    private void OnClickAddTypedDialogue(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<ConnectionNode>();
        }

        nodes.Add(CreateTypedDialogueNode(mousePosition));
    }

    private void OnClickAddOptionsDialogue(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<ConnectionNode>();
        }

        nodes.Add(CreateOptionsDialogueNode(mousePosition));
    }



    public DialogueNode<ScriptedDialogueNode> CreateScriptedDialogueNode(Vector2 mousePosition)
    {
        return new DialogueNode<ScriptedDialogueNode>(mousePosition, 300, 150, nodeStyle, selectedNodeStyle, OnClickRemoveNode, OnClickConnectionPoint, RemoveConnections);
    }

    public DialogueNode<TypedDialogueNode> CreateTypedDialogueNode(Vector2 mousePosition)
    {
        return new DialogueNode<TypedDialogueNode>(mousePosition, 250, 75, nodeStyle, selectedNodeStyle, OnClickRemoveNode, OnClickConnectionPoint, RemoveConnections);
    }

    public DialogueNode<OptionsDialogueNode> CreateOptionsDialogueNode(Vector2 mousePosition)
    {
        return new DialogueNode<OptionsDialogueNode>(mousePosition, 400, 475, nodeStyle, selectedNodeStyle, OnClickRemoveNode, OnClickConnectionPoint, RemoveConnections);
    }

    private void OnClickConnectionPoint(ConnectionPoint point)
    {
        switch (point.type)
        {
            case ConnectionPointType.In:
                selectedInPoint = point;

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
                break;
            case ConnectionPointType.Out:
                selectedOutPoint = point;

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
                break;
        }
        
    }

    private void OnClickRemoveNode(Node node)
    {
        RemoveConnections((ConnectionNode)node);

        nodes.Remove((ConnectionNode)node);
    }

    private void RemoveConnections(ConnectionNode node)
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

    private void SaveDialogueTree()
    {
        var path = EditorUtility.SaveFilePanel("Save Dialogue Tree", Application.dataPath, convoTitle, "json");

        SerializedNodeEditor serializedData = new SerializedNodeEditor();

        TreeList<IDialogueContext> dialogueTree = new TreeList<IDialogueContext>();
        ConnectionNode currentNode = (ConnectionNode)GetConnectedNode(entryNode);
        Queue<ConnectionNode> nodesToVist = new Queue<ConnectionNode>();
        List<IDialogueContext> contexts = new List<IDialogueContext>(nodes.Count);
        List<int> childCounts = new List<int>(nodes.Count);
        nodesToVist.Enqueue(currentNode);
        while (nodesToVist.Count > 0)
        {
            DialogueNode tmp = (DialogueNode)nodesToVist.Dequeue();
            int childCount = 0;

            if(tmp.GetType() == typeof(DialogueNode<OptionsDialogueNode>))
            {
                DialogueNode<OptionsDialogueNode> castNode = (DialogueNode<OptionsDialogueNode>)tmp;
                ConnectionNode[] children = castNode.Data.optionNodes.ToArray();
                childCount = children.Length;
                for(int i = 0; i < childCount; i++)
                {
                    ConnectionNode connectedNode = (ConnectionNode)GetConnectedNode(children[i]);
                    if (connectedNode != null)
                    {
                        nodesToVist.Enqueue(connectedNode);
                    }
                    else
                    {
                        Vector2 pos = castNode.rect.position;
                        DialogueNode<TypedDialogueNode> endNode = CreateTypedDialogueNode(new Vector2(pos.x + 500, pos.y + (i * 125.0f)));
                        endNode.Data.type = DialogueTypes.Type.Goodbye;
                        nodesToVist.Enqueue(endNode);
                    }
                }
            }
            else
            {
                ConnectionNode connectedNode = (ConnectionNode)GetConnectedNode(tmp);
                if(connectedNode != null)
                {
                    childCount = 1;
                    nodesToVist.Enqueue(connectedNode);
                }
                else
                {
                    childCount = 0;
                }
            }

            childCounts.Add(childCount);
            VisitNode(tmp.GetData(), childCount, dialogueTree, contexts);
            serializedData.Insert(tmp.GetData().GetTitle(), tmp);
        }

        ConversationLayout asset = CreateInstance<ConversationLayout>();
        asset.Init(convoTitle, contexts, childCounts);

        AssetDatabase.CreateAsset(asset, "Assets/ConversationLayouts/" + convoTitle + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        string json = JsonUtility.ToJson(serializedData);

        StreamWriter writer = new StreamWriter(path);
        try
        {
            writer.Write(json);
        }
        finally
        {
            writer.Close();
        }

    }

    private void LoadDialogueTree()
    {
        var path = EditorUtility.OpenFilePanel("Load Dialogue Tree", Application.dataPath, "json");

        if (nodes != null)
        {
            nodes.Clear();
        }
        else
        {
            nodes = new List<ConnectionNode>();
        }

        if(connections != null)
        {
            connections.Clear();
        }
        else
        {
            connections = new List<Connection>();
        }

        StreamReader reader = new StreamReader(path);
        string json = "";
        try
        {
            json = reader.ReadToEnd();
        }
        finally
        {
            reader.Close();
        }

        SerializedNodeEditor serializedData = JsonUtility.FromJson<SerializedNodeEditor>(json);

        NodeEditorDeserializer deserializer = new NodeEditorDeserializer(serializedData, this);

        ConnectionNode prevNode = entryNode;

        Queue<ConnectionNode> childrenNodes = new Queue<ConnectionNode>();

        for(int i = 0; i < deserializer.count; i++)
        {
            ConnectionNode node;
            bool isChildNode = false;
            if(childrenNodes.Count > 0)
            {
                selectedOutPoint = childrenNodes.Dequeue().GetOutPoint();
                isChildNode = true;
            }
            else
            {
                selectedOutPoint = prevNode.GetOutPoint();
            }

            node = deserializer.GetNode(i, childrenNodes);
            nodes.Add(node);

            selectedInPoint = node.GetInPoint();
            CreateConnection();

            if (!isChildNode)
            {
                prevNode = node;
            }
        }

        Debug.Log("Loading Done");
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

    private void VisitNode(DialogueData data, int childCount, TreeList<IDialogueContext> tree, List<IDialogueContext> contexts)
    {
        IDialogueContext context = data.GetDialogueContext();
        contexts.Add(context);
        tree.Insert(new TreeNode<IDialogueContext>(context, childCount));
    }

    public class NodeEditorDeserializer
    {
        public int count;

        private Queue<SerializedScriptedNode> scriptedNodes;
        private Queue<SerializedTypedNode> typedNodes;
        private Queue<SerializedOptionsNode> optionNodes;
        private NodeBasedEditor editor;

        public NodeEditorDeserializer(SerializedNodeEditor data, NodeBasedEditor editor)
        {
            scriptedNodes = new Queue<SerializedScriptedNode>(data.scriptedNodes);
            typedNodes = new Queue<SerializedTypedNode>(data.typedNodes);
            optionNodes = new Queue<SerializedOptionsNode>(data.optionNodes);
            this.editor = editor;

            count = data.count;
        }

        public ConnectionNode GetNode(int index, Queue<ConnectionNode> children)
        {
            bool inScripted = scriptedNodes.Count > 0;
            bool inTyped = typedNodes.Count > 0;
            bool inOptions = optionNodes.Count > 0;

            if (inScripted)
            {
                int order = scriptedNodes.Peek().order;
                if (order == index)
                {
                    SerializedScriptedNode data = scriptedNodes.Dequeue();
                    DialogueNode<ScriptedDialogueNode> node = editor.CreateScriptedDialogueNode(data.position);
                    node.Data.sentences = data.sentences;
                    return node;
                }else if(index > order)
                {
                    inScripted = false;
                }
            }

            if (inTyped)
            {
                int order = typedNodes.Peek().order;
                if(order == index)
                {
                    SerializedTypedNode data = typedNodes.Dequeue();
                    DialogueNode<TypedDialogueNode> node = editor.CreateTypedDialogueNode(data.position);
                    node.Data.type = data.type;
                    return node;
                }else if(index > order)
                {
                    inTyped = false;
                }
            }

            if (inOptions)
            {
                int order = optionNodes.Peek().order;
                if(order == index)
                {
                    SerializedOptionsNode data = optionNodes.Dequeue();
                    DialogueNode<OptionsDialogueNode> node = editor.CreateOptionsDialogueNode(data.position);
                    List<SerializedOptionNode> optionsData = data.optionNodes;
                    for(int i = 0; i < data.optionsCount; i++)
                    {
                        node.Data.AddOption();
                        node.Data.optionNodes[i].text = optionsData[i].sentence;
                        node.Data.optionNodes[i].proceedToNextSpeaker = optionsData[i].goToNextSpeaker;
                        children.Enqueue(node.Data.optionNodes[i]);
                    }

                    return node;
                }else if(index > order)
                {
                    inOptions = false;
                }
            }

            return null;
        }
    }


    [System.Serializable]
    public class SerializedNodeEditor
    {
        int order;
        public int count;

        public List<SerializedScriptedNode> scriptedNodes;
        public List<SerializedTypedNode> typedNodes;
        public List<SerializedOptionsNode> optionNodes;

        public SerializedNodeEditor()
        {
            scriptedNodes = new List<SerializedScriptedNode>();
            typedNodes = new List<SerializedTypedNode>();
            optionNodes = new List<SerializedOptionsNode>();

            order = count = 0;
        }

        public void Insert(string title, DialogueNode node)
        {
            switch (title)
            {
                case "Scripted Dialogue":
                    Insert((DialogueNode<ScriptedDialogueNode>)node);
                    break;
                case "Typed Dialogue":
                    Insert((DialogueNode<TypedDialogueNode>)node);
                    break;
                case "Options Dialogue":
                    Insert((DialogueNode<OptionsDialogueNode>)node);
                    break;
            }
        }

        private void Insert(DialogueNode<ScriptedDialogueNode> node)
        {
            scriptedNodes.Add(new SerializedScriptedNode(order++, node.Position, node.Data.sentences));
            count++;
        }

        private void Insert(DialogueNode<TypedDialogueNode> node)
        {
            typedNodes.Add(new SerializedTypedNode(order++, node.Position, node.Data.type));
            count++;
        }

        private void Insert(DialogueNode<OptionsDialogueNode> node)
        {
            List<SerializedOptionNode> options = new List<SerializedOptionNode>();
            foreach (OptionNode option in node.Data.optionNodes)
            {
                options.Add(new SerializedOptionNode(option.text, option.proceedToNextSpeaker));
            }
            optionNodes.Add(new SerializedOptionsNode(order++, node.Position, options, options.Count));
            count++;
        }
    }


    [System.Serializable]
    public struct SerializedScriptedNode
    {
        public Vector2 position;
        public int order;
        public List<string> sentences;

        public SerializedScriptedNode(int order, Vector2 pos, List<string> sentences)
        {
            this.order = order;
            this.position = pos;
            this.sentences = sentences;
        }
    }

    [System.Serializable]
    public struct SerializedTypedNode
    {
        public Vector2 position;
        public int order;
        public DialogueTypes.Type type;

        public SerializedTypedNode(int order, Vector2 pos, DialogueTypes.Type type)
        {
            this.order = order;
            this.position = pos;
            this.type = type;
        }
    }

    [System.Serializable]
    public struct SerializedOptionsNode
    {
        public Vector2 position;
        public int order;
        public List<SerializedOptionNode> optionNodes;
        public int optionsCount;

        public SerializedOptionsNode(int order, Vector2 pos, List<SerializedOptionNode> optionNodes, int optionsCount)
        {
            this.order = order;
            this.position = pos;
            this.optionNodes = optionNodes;
            this.optionsCount = optionsCount;
        }
    }

    [System.Serializable]
    public struct SerializedOptionNode
    {
        public string sentence;
        public bool goToNextSpeaker;


        public SerializedOptionNode(string sentence, bool goToNextSpeaker)
        {
            this.sentence = sentence;
            this.goToNextSpeaker = goToNextSpeaker;
        }
    }

}
