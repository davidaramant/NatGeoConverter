using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Utilities {
	public static class JsonDeserializer {
		private static readonly Dictionary<Type,DataContractJsonSerializer> _serializerCache = 
			new Dictionary<Type, DataContractJsonSerializer>();

		private static DataContractJsonSerializer GetSerializer( Type type ) {
			DataContractJsonSerializer serializer = null;
			if( !_serializerCache.TryGetValue( type, out serializer ) ) {
				serializer = new DataContractJsonSerializer( type );
				_serializerCache[ type ] = serializer;
			}
			return serializer;
		}

		public static T Deserialise<T>( string json ) {
			T obj = Activator.CreateInstance<T>();
			using( MemoryStream ms = new MemoryStream( Encoding.Unicode.GetBytes( json ) ) ) {
				var serializer = GetSerializer( obj.GetType() );
				obj = (T)serializer.ReadObject( ms );
				return obj;
			} 
		}
	}
}

