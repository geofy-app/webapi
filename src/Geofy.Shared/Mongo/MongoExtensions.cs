﻿using System;
using MongoDB.Bson;

namespace Geofy.Shared.Mongo
{
    public static class MongoExtensions
    {
        public static DateTime DefaultDateTime = DateTime.Parse("1980/1/1");

        public static string GetString(this BsonDocument doc, string key, string defaultValue = "")
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsString)
                return defaultValue;

            return value.AsString;
        }

        public static DateTime GetDateTime(this BsonDocument doc, string key, DateTime defaultValue = default(DateTime))
        {
            var defaultDateTime = defaultValue == default(DateTime) ? DefaultDateTime : defaultValue;

            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsValidDateTime)
                return defaultDateTime;

            return value.ToUniversalTime();
        }


        public static Int32 GetInt32(this BsonDocument doc, string key, Int32 defaultValue = 0)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsInt32)
                return defaultValue;

            return value.AsInt32;
        }

        public static Int32 GetInt(this BsonDocument doc, string key, Int32 defaultValue = 0)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains)
                return defaultValue;

            if (value.IsInt32)
                return value.AsInt32;

            if (value.IsInt64)
                return Convert.ToInt32(value.AsInt64);
            return defaultValue;
        }

        public static bool GetBool(this BsonDocument doc, string key, bool defaultValue = false)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsBoolean)
                return defaultValue;

            return value.AsBoolean;
        }

        public static Double GetDouble(this BsonDocument doc, string key, Double defaultValue = 0)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsDouble)
                return defaultValue;

            return value.AsDouble;
        }

        public static BsonArray GetBsonArray(this BsonDocument doc, string key)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsBsonArray)
                return new BsonArray();

            return value.AsBsonArray;
        }

        public static BsonDocument GetBsonDocument(this BsonDocument doc, string key)
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsBsonDocument)
                return new BsonDocument();

            return value.AsBsonDocument;
        }


    }
}