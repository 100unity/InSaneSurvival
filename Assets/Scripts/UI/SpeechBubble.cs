using Entity.Player;
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

        [Tooltip("The player controller to get the camera distance range from")] [SerializeField]
        private PlayerController playerController;
        
        [Tooltip("The translation factor for moving the speech bubble on camera distance changes")] [SerializeField]
        private float translationFactor;

        private void Start() => UpdatePosition(playerController.CameraDistance);

        private void OnEnable() => PlayerController.OnCameraDistanceChange += UpdatePosition;

        private void OnDisable() => PlayerController.OnCameraDistanceChange -= UpdatePosition;

        /// <summary>
        /// Displays a speech bubble with the specified image in it above the player's head
        /// </summary>
        /// <param name="imageToDisplay">The image to display in the speech bubble</param>
        public void ShowSpeechBubble(Sprite imageToDisplay)
        {
            // Setup image and display speech bubble for displayTime
            image.sprite = imageToDisplay;
            speechBubbleContent.SetActive(true);
            Invoke(nameof(HideSpeechBubble), displayTime);
        }

        /// <summary>
        /// Hides the speech bubble
        /// </summary>
        private void HideSpeechBubble() => speechBubbleContent.SetActive(false);

        /// <summary>
        /// Updates the position of the speech bubble depending on the camera distance to the player
        /// </summary>
        private void UpdatePosition(float cameraDistance)
        {
            float normalizedCameraDistance = (cameraDistance - playerController.CameraDistanceRange.min) /
                                             (playerController.CameraDistanceRange.max - playerController.CameraDistanceRange.min);
            Vector3 position = speechBubbleContent.transform.localPosition;
            position.y = normalizedCameraDistance * translationFactor;
            speechBubbleContent.transform.localPosition = position;
        }
    }
}