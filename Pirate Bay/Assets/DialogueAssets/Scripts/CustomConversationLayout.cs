using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Conversation Layout", menuName = "Conversation Layouts/Custom Conversation Layout")]
public class CustomConversationLayout : ScriptableObject {

    public List<IDialogueContext> contexts = new List<IDialogueContext>();

    public int Length { get { return contexts.Count; } }
	
}
