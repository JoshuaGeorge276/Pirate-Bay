using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface DialogueData {

    void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionNode> OnRemoveConnections);

    void Draw();

    IDialogueContext GetDialogueContext();

    bool ContainsConnection(ConnectionPoint point);

    void DrawConnections();

    string GetTitle();

    int GetChildCount();

    ConnectionPoint GetInPoint();

    ConnectionPoint GetOutPoint();

}
