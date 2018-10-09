using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContextContainer {

    public IDialogueContext context;
    public IDialogueContext nextContext;

    public DialogueContextContainer(IDialogueContext context)
    {
        this.context = context;
        this.nextContext = null;
    }

    public DialogueContextContainer(IDialogueContext context, IDialogueContext next)
    {
        this.context = context;
        this.nextContext = next;
    }
}
