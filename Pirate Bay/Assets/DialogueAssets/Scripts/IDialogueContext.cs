using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IDialogueContext : ScriptableObject {

    public abstract IDialogueDisplayer Display(Speaker speaker, IIterable conversation);
}
