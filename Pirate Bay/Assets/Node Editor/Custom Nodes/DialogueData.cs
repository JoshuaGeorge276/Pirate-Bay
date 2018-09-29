using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface DialogueData {

    void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint);

    void Draw();

    IDialogueContext GetDialogueContext();

    bool ContainsConnection(ConnectionPoint point);

    void DrawConnections();

}
