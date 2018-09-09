using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conversation : IIterable {

    public abstract void Next();

    public abstract void ProceedToNextSpeaker();

}
