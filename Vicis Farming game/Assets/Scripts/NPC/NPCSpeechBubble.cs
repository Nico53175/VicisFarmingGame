using UnityEngine;

public class NPCSpeechBubble : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpeechBubbleData speechBubbleData;

    void Start()
    {
        spriteRenderer.sprite = null; // Initially, no speech bubble should be shown.
    }

    public void ShowRandomSpeechBubble()
    {
        // Randomly select a sprite from the sprites array.
        Sprite randomSprite = speechBubbleData.sprites[Random.Range(0, speechBubbleData.sprites.Length)];
        spriteRenderer.sprite = randomSprite;
    }

    public void HideSpeechBubble()
    {
        spriteRenderer.sprite = null;
    }
}
