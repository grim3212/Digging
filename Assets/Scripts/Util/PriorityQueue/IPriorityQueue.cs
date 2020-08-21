public interface IPriorityQueue<T> {
	/// Inserts and item with a priority
	void Insert (T item, int priority);

	/// Returns the element with the highest priority
	T Top ();

	/// Deletes and returns the element with the highest priority
	T Pop ();
}