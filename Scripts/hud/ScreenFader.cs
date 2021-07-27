namespace hud {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// TODO(zack): Decide if this is a singleton
	public class ScreenFader : MonoBehaviour {

		Animator animator;
		bool isFading = false;

		void Start () {
			animator = GetComponent<Animator>();
		}

		public IEnumerator FadeToClear() {
			isFading = true;
			animator.SetTrigger("fadeIn");
            while (isFading) yield return null;
		}

		public IEnumerator FadeToBlack() {
			isFading = true;
			animator.SetTrigger("fadeOut");
            while (isFading) yield return null;
		}

		void AnimationComplete() {
			isFading = false;
			animator.ResetTrigger("fadeIn");
			animator.ResetTrigger("fadeOut");
		}
	}
}
