using System;
using System.Collections;

namespace GodLesZ.Library.Amf.Collections {
	/// <summary>
	/// A Set that uses CopyOnWriteArray for all of its operations. Thus, it shares the same basic properties:
	/// It is best suited for applications in which set sizes generally stay small, read-only operations vastly outnumber mutative operations, and you need to prevent interference among threads during traversal.
	/// It is thread-safe.
	/// Mutative operations(add, set, remove, etc) are expensive since they usually entail copying the entire underlying array.
	/// Traversal via enumerators is fast and cannot encounter interference from other threads. Enumerators rely on unchanging snapshots of the array at the time the enumerators were constructed.
	/// </summary>
	public class CopyOnWriteArraySet : ICollection {
		private CopyOnWriteArray _array;

		/// <summary>
		/// Creates an empty set.
		/// </summary>
		public CopyOnWriteArraySet() {
			_array = new CopyOnWriteArray();
		}
		/// <summary>
		/// Creates a set containing all of the elements of the specified collection.
		/// </summary>
		/// <param name="collection"></param>
		public CopyOnWriteArraySet(ICollection collection) {
			_array = new CopyOnWriteArray();
			foreach (object obj in collection)
				_array.Add(obj);
		}

		/// <summary>
		/// Gets the number of elements contained in the CopyOnWriteArraySet.
		/// </summary>
		public int Count {
			get { return _array.Count; }
		}
		/// <summary>
		/// Returns <tt>true</tt> if this set contains no elements.
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty {
			get { return _array.Count == 0; }
		}
		/// <summary>
		/// Determines whether the CopyOnWriteArraySet contains a specific value.
		/// </summary>
		/// <param name="value">The Object to locate in the CopyOnWriteArraySet.</param>
		/// <returns>true if the Object is found in the CopyOnWriteArraySet; otherwise, false.</returns>        
		public bool Contains(object value) {
			return _array.Contains(value);
		}
		/// <summary>
		/// Removes all of the elements from this set.
		/// </summary>
		public void Clear() {
			_array.Clear();
		}
		/// <summary>
		/// Removes the specified element from this set if it is present.
		/// </summary>
		/// <param name="obj"></param>
		public void Remove(object obj) {
			_array.Remove(obj);
		}
		/// <summary>
		/// Adds the specified element to this set if it is not already present.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Add(object value) {
			return _array.AddIfAbsent(value);
		}
		/// <summary>
		/// Returns an enumerator that iterates through an CopyOnWriteArraySet.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the set.</returns>
		public IEnumerator GetEnumerator() {
			return _array.GetEnumerator();
		}


		#region ICollection Members

		/// <summary>
		/// Copies the elements of the CopyOnWriteArraySet to an Array, starting at a particular Array index.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from CopyOnWriteArraySet. The Array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which copying begins.</param>
		public void CopyTo(Array array, int index) {
			_array.CopyTo(array, index);
		}
		/// <summary>
		/// Gets a value indicating whether access to the CopyOnWriteArraySet is synchronized (thread safe).
		/// </summary>
		public bool IsSynchronized {
			get { return false; }
		}
		/// <summary>
		/// Gets an object that can be used to synchronize access to the CopyOnWriteArray.
		/// </summary>
		public object SyncRoot {
			get { return _array.SyncRoot; }
		}

		#endregion
	}
}
