namespace generic {
	using System;
	using System.Collections;

	public class ReverseComparer : IComparer  
	{
	   // Call CaseInsensitiveComparer.Compare with the parameters reversed.
	   public int Compare(Object x, Object y)  
	   {
		   return (new CaseInsensitiveComparer()).Compare(y, x );
	   }
	}
}