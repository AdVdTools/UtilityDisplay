using UnityEngine;
using UnityEngine.Events;

namespace AdVd {
	public class SequenceDetector : MonoBehaviour {
		enum StepState { Waiting, Running, Success, Failure }

		int currentStep = 0;
		float lastEventTime;
		Vector2 lastEventPosition;
		StepState stepState;

		public InputSequence sequence;

		public UnityEvent OnCorrectSequence;

		void Update() {
			if (sequence != null && sequence.steps != null && sequence.steps.Length > 0) {
				Step current = sequence.steps[currentStep];
				float currentTime = Time.unscaledTime;
				bool mouse = Input.GetMouseButton(0);
				Vector2 mousePosition = (Vector2)Input.mousePosition;

				switch (current.type) {
				case StepType.Tap:
					if (stepState == StepState.Waiting) {
						if (mouse) {
							lastEventTime = currentTime;
							stepState = StepState.Running;
						}
					}
					else {
						if (!mouse){
							if (currentTime < lastEventTime + current.v0 || currentTime > lastEventTime + current.v1) {//Failed
								stepState = StepState.Failure;
							}
							else {
								stepState = StepState.Success;
							}
							lastEventTime = currentTime;
						}
					}
					break;
				case StepType.Delay:
					if (stepState == StepState.Waiting) {
						if (!mouse) {
							lastEventTime = currentTime;
							stepState = StepState.Running;
						}
					}
					else {
						if (mouse){
							if (currentTime < lastEventTime + current.v0) {//Failed
								stepState = StepState.Failure;
							}
							else {
								stepState = StepState.Success;
							}
							lastEventTime = currentTime;
						}
						else if (currentTime > lastEventTime + current.v1) {
							stepState = StepState.Failure;
						}
					}
					break;
				case StepType.Swipe:
					if (stepState == StepState.Waiting) {
						if (mouse) {
							lastEventTime = currentTime;
							lastEventPosition = mousePosition;
							stepState = StepState.Running;
						}
					}
					else {
						Vector2 delta = mousePosition - lastEventPosition;
						float dist2 = delta.SqrMagnitude();
						float v2 = current.v0*current.v0 + current.v1*current.v1;
						float dot = delta.x*current.v0 + delta.y*current.v1;
						float cross = delta.x*current.v1 - delta.y*current.v0;

						if (currentTime > lastEventTime + sequence.maxSwipeLength) {
							stepState = StepState.Failure;
							lastEventTime = currentTime;
						}
						else if (dist2 > v2) {
							if (cross*cross > dot*dot || dot < 0) {//Failed
								stepState = StepState.Failure;
							}
							else if (!mouse) {//Success
								stepState = StepState.Success;
							}
							else {
								break;
							}
							lastEventTime = currentTime;
						}
					}
					break;
				}
//				Debug.Log(current.type.ToString()+"_"+stepState.ToString());
				if (stepState == StepState.Success) {
					currentStep++;
					stepState = StepState.Waiting;
				}
				else if (stepState == StepState.Failure) {
					currentStep = -1;
					if (!mouse) {
						stepState = StepState.Waiting;
					}
				}

				if (currentStep >= sequence.steps.Length) {
					currentStep = 0;
					OnCorrectSequence.Invoke();
				}
				else if (currentStep < 0) {
					currentStep = 0;
				}
				if (stepState == StepState.Waiting && currentTime > lastEventTime+sequence.maxStepDelay) {//Sequence stopped
					currentStep = 0;
				}
			}
		}
	}
}
