using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SequenceDetector : MonoBehaviour {
	enum Step { ShortTap, LongTap }
	[SerializeField]
	Step[] switchSequence;
	int currentStep = 0;
	float lastEventTime;
	bool stepRunning;

	[SerializeField]
	float maxStepDelay = 0.25f;
	[SerializeField]
	float longTapLength = 0.25f;

	[SerializeField]
	UnityEvent OnCorrectSequence;

	void Update() {
		if (switchSequence != null && switchSequence.Length > 0) {
			Step current = switchSequence[currentStep];
			float currentTime = Time.unscaledTime;
			bool mouse = Input.GetMouseButton(0);
			switch (current) {
			case Step.ShortTap:
				if (!stepRunning) {
					if (mouse) {
						lastEventTime = currentTime;
						stepRunning = true;
					}
				}
				else {
					if (!mouse){
						if (currentTime > lastEventTime + longTapLength) {//Failed
							currentStep = -1;
						}
						else {
							currentStep++;
						}
						lastEventTime = currentTime;
						stepRunning = false;
					}
				}
				break;
			case Step.LongTap:
				if (!stepRunning) {
					if (mouse) {
						lastEventTime = currentTime;
						stepRunning = true;
					}
				}
				else {
					if (!mouse){
						if (currentTime < lastEventTime + longTapLength) {//Failed
							currentStep = -1;
						}
						else {
							currentStep++;
						}
						lastEventTime = currentTime;
						stepRunning = false;
					}
				}
				break;
			}
			if (currentStep >= switchSequence.Length) {
				currentStep = 0;
				OnCorrectSequence.Invoke();
			}
			else if (currentStep < 0) {
				currentStep = 0;
			}
			if (!stepRunning && currentTime > lastEventTime+maxStepDelay) {//Sequence stopped
				currentStep = 0;
			}
		}
	}
}
