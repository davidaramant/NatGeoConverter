using System.Linq;
using System.Collections.Generic;
using System;

namespace Utilities.EnumerableExtensions {
	public static class Extensions {
		public static IEnumerable<IEnumerable<T>> GetBatchesOfSize<T>(this IEnumerable<T> source, int size) {
			if( size <= 0 ) {
				throw new ArgumentOutOfRangeException( "size", "Must be greater than zero." );
			}

			using( IEnumerator<T> enumerator = source.GetEnumerator() ) {
				while( enumerator.MoveNext() ) {
					yield return TakeIEnumerator( enumerator, size );
				}
			}
		}

		private static IEnumerable<T> TakeIEnumerator<T>(IEnumerator<T> source, int size) {
			int i = 0;
			do
			{
				yield return source.Current;
			}
			while (++i < size && source.MoveNext());
		}

		public static T ElementWithMax<T>( this IEnumerable<T> sequence, Func<T,int> selector )
		{
			if( sequence == null ) {
				throw new ArgumentNullException( "sequence" );
			}
			if( selector == null ) {
				throw new ArgumentNullException( "selector" );
			}

			var maxElement = default(T);
			var max = Int32.MinValue;

			foreach( var element in sequence ) {
				var value = selector( element );
				if( value >= max ) {
					max = value;
					maxElement = element;
				}
			}

			return maxElement;
		}

		public static T ElementWithMin<T>( this IEnumerable<T> sequence, Func<T,int> selector )
		{
			if( sequence == null ) {
				throw new ArgumentNullException( "sequence" );
			}
			if( selector == null ) {
				throw new ArgumentNullException( "selector" );
			}

			var minElement = default(T);
			var min = Int32.MaxValue;

			foreach( var element in sequence ) {
				var value = selector( element );
				if( value <= min ) {
					min = value;
					minElement = element;
				}
			}

			return minElement;
		}

		public sealed class ElementContext<T> where T : class
		{
			public readonly T Previous;
			public readonly T Current;
			public readonly T Next;

			public ElementContext( T previous, T current, T next )
			{
				Previous = previous;
				Current = current;
				Next = next;
			}
		}

		public static IEnumerable<ElementContext<T>> WithContext<T>( this IEnumerable<T> sequence ) where T : class{
			if( sequence == null ) {
				throw new ArgumentNullException( "sequence" );
			}
			return WithContextWorker( sequence );
		}

		private static IEnumerable<ElementContext<T>> WithContextWorker<T>( this IEnumerable<T> sequence ) where T : class{
			T previous = default(T);
			T current = default(T);
			T next = default(T);

			bool skippedFirst = false;
			foreach( var element in sequence ) {
				previous = current;
				current = next;
				next = element;

				if( skippedFirst ) {
					yield return new ElementContext<T>( previous, current, next );
				}
				skippedFirst = true;
			}

			yield return new ElementContext<T>( previous: current, current: next, next: default(T) );
		}
	}
}

