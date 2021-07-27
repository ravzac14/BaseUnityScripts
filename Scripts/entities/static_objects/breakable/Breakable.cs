namespace static_objects.breakable {
	using generic;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Assertions;

	public class Breakable : MonoBehaviour {

		Animator animator;
	
		public int maxHealth;
		public int[] breakThresholds;
		public int damageStub;

		private int currentHealth;
		private Option<int> maybeLastBreakThresholdIndex = new None<int>();
		
		private string animationTriggerName(int breakThresholdIndex) {
			string breakThresholdAnimNamePrefix = "breakThreshold";
			Assert.IsTrue(breakThresholds.Length >= breakThresholdIndex + 1, "Breakable.animationTriggerName called with index out of bounds " + breakThresholdIndex);
			if (breakThresholdIndex + 1 == breakThresholds.Length) {
				return breakThresholdAnimNamePrefix + "Last";
			} else {
				return breakThresholdAnimNamePrefix + (breakThresholdIndex + 1);
			}
		}
		
		private void validateBreakThresholds() {
			Assert.IsTrue(breakThresholds.Length > 0, "Breakable.breakThresholds must not be empty.");
		}
				
		private Option<int> maybeBreakThresholdIndexFor(int newCurrentHealth) {
			if (Array.Exists(breakThresholds, bt => bt >= newCurrentHealth))
				new Some<int>(Array.FindLastIndex(breakThresholds, bt => bt >= newCurrentHealth));
			return new None<int>();
		}

		void Start() {
			/* Validations */
			validateBreakThresholds();
			
			/* Inits */
			animator = GetComponent<Animator>();
			Array.Sort(breakThresholds, new ReverseComparer());
			currentHealth = maxHealth;
		}

		void OnCollision2D(Collision collision) {
			// TODO(zack): Find the magic damage number from the given collider and remove this hardcode dmg value (and above public field)
			currentHealth -= damageStub;
		
			/* Maybe update the animation with the appropriate trigger
			 * NOTE(zack): This assumes that the Animator is configured to work with the amount of breakThresholds provided */
			Option<int> maybeIndex = maybeBreakThresholdIndexFor(currentHealth);
			if (maybeIndex.exists(bt => bt != maybeLastBreakThresholdIndex.getOrElse(-1))) {
				maybeLastBreakThresholdIndex = maybeIndex;
				animator.SetTrigger(animationTriggerName(maybeIndex.get()));
			}
		}
	}
}
