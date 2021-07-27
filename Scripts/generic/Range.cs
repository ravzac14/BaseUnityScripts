namespace generic {
	using System;

	/* Range of ints, inclusive of min and max */
	[Serializable]
	public class Range {
		public int max;
		public int min;
	
		public Range(int min, int max) {
			this.max = max;
			this.min = min;
		}
		
		public bool contains(int i) {
			return (min <= i) && (max >= i);
		}
	}
}