using System.Collections;
using UnityEngine;

public abstract class NPCBasicInteraction : MonoBehaviour
{
    public NPCSpeechBubble speechBubble;

    public virtual void NPCInteraction()
    {
        Debug.Log("Interaction with " + gameObject.name);
        StartCoroutine(ShowAndHideSpeechBubble());
    }

    protected IEnumerator ShowAndHideSpeechBubble()
    {
        // Show the speech bubble
        speechBubble.ShowRandomSpeechBubble();

        // Wait for a random amount of time between 2 and 4 seconds
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        // Hide the speech bubble
        speechBubble.HideSpeechBubble();
    }
}
