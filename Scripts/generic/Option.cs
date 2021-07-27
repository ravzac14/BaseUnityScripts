namespace generic {
	using System;
	using System.Collections.Generic;
	
	[Serializable]
	public abstract class Option<T> {
		public abstract T get();
		public abstract T getOrElse(T other);
		public abstract bool isDefined();
		public abstract bool isEmpty();
		public abstract bool exists(Func<T, bool> p);
		public abstract Option<S> map<S>(Func<T, S> f);
		public abstract Option<S> flatMap<S>(Func<T, Option<S>> f);
	}
	
	[Serializable]
	public sealed class None<T>: Option<T> {
		public None() {}
	
		public override T get() { throw new System.NotSupportedException("get called on None"); }
		public override T getOrElse(T other) { return other; }
		public override bool isDefined() { return false; }
		public override bool isEmpty() { return true; }
		public override bool exists(Func<T, bool> p) { return false; }
		public override Option<S> map<S>(Func<T, S> f) { return new None<S>(); }
		public override Option<S> flatMap<S>(Func<T, Option<S>> f) { return new None<S>(); }
	}
	
	[Serializable]
	public sealed class Some<T>: Option<T> {
		private T value;
		
		public Some(T value) { this.value = value; }
		
		public override T get() { return value; }
		public override T getOrElse(T other) { return this.get();}
		public override bool isDefined() { return true; }
		public override bool isEmpty() { return false; }
		public override bool exists(Func<T, bool> p) { return p(value); }
		public override Option<S> map<S>(Func<T, S> f) { return new Some<S>(f(value)); }
		public override Option<S> flatMap<S>(Func<T, Option<S>> f) { return f(value); }
	}
}