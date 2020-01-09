using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpeechBubble : MonoBehaviour
    {
        [Tooltip("The speech bubble content to show")] [SerializeField]
        private GameObject speechBubbleContent;
        
        [Tooltip("The image component of the speech bubble")] [SerializeField]
        private Image image;

        [Tooltip("The amount of seconds the speech bubble is supposed to be displayed for")] [SerializeField]
        private float displayTime;

        public void ShowSpeechBubble(Sprite imageToDisplay)
        {
            image.sprite = imageToDisplay;
            speechBubbleContent.SetActive(true);
            Invoke(nameof(HideSpeechBubble), displayTime);
        }

        private void HideSpeechBubble() => speechBubbleContent.SetActive(false);
    }
}