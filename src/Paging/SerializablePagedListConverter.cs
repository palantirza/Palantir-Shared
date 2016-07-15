namespace Palantir.Paging
{
	using System;
	using Newtonsoft.Json;
	using PagedList;
	using System.Collections;
	using Newtonsoft.Json.Linq;
	using System.Linq;

	/// <summary>
	/// A Json converter for the Seriazable
	/// </summary>
	public class SerializablePagedListConverter : JsonConverter
	{
		private readonly static Type pagedListInterfaceType = typeof(IPagedList<>);

		/// <summary>
		/// Indicates whether the type can be converted.
		/// </summary>
		/// <param name="objectType">The type.</param>
		/// <returns>true if it can be converted, false otherwise.</returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType != null && typeof(IPagedList).IsAssignableFrom(objectType) && objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(SerializablePagedList<>);
		}

		/// <summary>
		/// Reads the JSON and returns the deserialized object.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="objectType">The object type.</param>
		/// <param name="existingValue">The existing value.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns>The deserialized object.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Read())
			{
				int pageNumber;
				int pageSize;
				int totalItemCount;
				if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$PageNumber" && reader.Read())
				{
					pageNumber = Convert.ToInt32(reader.Value);
				}
				else
					throw new FormatException(SR.Err_InvalidFormatFor(objectType.Name));

				if (reader.Read() && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$PageSize" && reader.Read())
				{
					pageSize = Convert.ToInt32(reader.Value);
				}
				else
					throw new FormatException(SR.Err_InvalidFormatFor(objectType.Name));

				if (reader.Read() && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$TotalItemCount" && reader.Read())
				{
					totalItemCount = Convert.ToInt32(reader.Value);
				}
				else
					throw new FormatException(SR.Err_InvalidFormatFor(objectType.Name));

				if (reader.Read() && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$Items" && reader.Read() && reader.TokenType == JsonToken.StartArray && reader.Read())
				{
					var elementType = objectType.GetGenericArguments()[0];

					var items = new ArrayList();
					while (reader.TokenType != JsonToken.EndArray)
					{
						var item = serializer.Deserialize(reader, elementType);

						items.Add(item);

						reader.Read();
					}
					reader.Read();

					var array = Array.CreateInstance(elementType, items.Count);
					for (int i = 0; i < items.Count; i++)
						array.SetValue(items[i], i);

					return Activator.CreateInstance(objectType, array, pageNumber, pageSize, totalItemCount);
				}
			}

			throw new FormatException(SR.Err_InvalidFormatFor(objectType.Name));
		}

		/// <summary>
		/// Writes the JSON.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value to serialize.</param>
		/// <param name="serializer">The serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var collection = (IEnumerable)value;
			var list = (IPagedList)value;
			writer.WriteStartObject();
			writer.WritePropertyName("$PageNumber");
			writer.WriteValue(list.PageNumber);
			writer.WritePropertyName("$PageSize");
			writer.WriteValue(list.PageSize);
			writer.WritePropertyName("$TotalItemCount");
			writer.WriteValue(list.TotalItemCount);

			writer.WritePropertyName("$Items");
			writer.WriteStartArray();

			var pagedListType = value.GetType().GetInterfaces().Where(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == pagedListInterfaceType).Single();
			var elementType = pagedListType.GetGenericArguments()[0];

			foreach (var item in collection)
			{
				serializer.Serialize(writer, item, elementType);
			}
			writer.WriteEndArray();

			writer.WriteEndObject();
		}
	}
}