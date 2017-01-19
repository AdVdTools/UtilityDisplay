using UnityEngine;

namespace AdVd {
	public enum StepType { 
		Tap,// v0: min length, v1: max length
		Delay,// v0: min length, v1: max length 
		Swipe// (v0, v1): main direction & distance
	}
	[System.Serializable]
	public class Step {
		public const float sqrt2 = 1.4142f;

		public StepType type;
		public float v0;
		public float v1;
	}

	[CreateAssetMenu(menuName="Input Sequence")]
	public class InputSequence : ScriptableObject {
		public Step[] steps;

		public float maxStepDelay = 0.25f;
		public float maxSwipeLength = 1f;
	}
}
