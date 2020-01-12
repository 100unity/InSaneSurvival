using UnityEngine;

namespace StateMachineBehaviours
{
    public class AnimationRandomizer : StateMachineBehaviour
    {
        [Tooltip("The number of attack animations in the sub-state machine")] [SerializeField]
        private int numberOfAnimations;

        [Tooltip("The name of the animator's animation ID parameter")] [SerializeField]
        private string idParameterName;

        private static readonly System.Random Random = new System.Random();
    
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) =>
            animator.SetInteger(idParameterName, Random.Next(numberOfAnimations));
    }
}