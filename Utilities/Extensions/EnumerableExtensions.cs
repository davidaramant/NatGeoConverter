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
	}
}

