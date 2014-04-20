using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;

namespace Utilities {
	public static class JsonUtility {
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
			using( var ms = new MemoryStream( Encoding.Unicode.GetBytes( json ) ) ) {
				var serializer = GetSerializer( obj.GetType() );
				obj = (T)serializer.ReadObject( ms );
				return obj;
			} 
		}

		private const string INDENT_STRING = "    ";
		public static string FormatJson( string str )
		{
			var indent = 0;
			var quoted = false;
			var sb = new StringBuilder();
			for (var i = 0; i < str.Length; i++)
			{
				var ch = str[i];
				switch (ch)
				{
					case '{':
					case '[':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							foreach( var item in Enumerable.Range(0, ++indent))
							{
								sb.Append( INDENT_STRING );
							}
						}
						break;
					case '}':
					case ']':
						if (!quoted)
						{
							sb.AppendLine();
							foreach( var item in Enumerable.Range(0, --indent) )
							{
								sb.Append(INDENT_STRING);
							}
						}
						sb.Append(ch);
						break;
					case '"':
						sb.Append(ch);
						bool escaped = false;
						var index = i;
						while (index > 0 && str[--index] == '\\')
							escaped = !escaped;
						if (!escaped)
							quoted = !quoted;
						break;
					case ',':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							foreach( var item in Enumerable.Range(0, indent) ) 
							{
								sb.Append( INDENT_STRING );
							}
						}
						break;
					case ':':
						sb.Append(ch);
						if (!quoted)
							sb.Append(" ");
						break;
					default:
						sb.Append(ch);
						break;
				}
			}
			return sb.ToString();
		}
	}
}

