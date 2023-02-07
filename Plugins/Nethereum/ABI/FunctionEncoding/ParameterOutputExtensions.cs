﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;
using Newtonsoft.Json.Linq;

namespace Nethereum.ABI.FunctionEncoding
{
    public static class ParameterOutputExtensions
    {

        public static string ConvertToString(this IEnumerable<ParameterOutput> parameterOutputs, int level = 0)
        {

            var stringBuilder = new StringBuilder();

            foreach (var item in parameterOutputs)
            {
                if (item.Parameter.ABIType is ArrayType)
                {
                    AppendObjectTitle(stringBuilder, item.Parameter.Name, level);
                    stringBuilder.AppendLine(ConvertArrayToString((IEnumerable)item.Result, level + 1));
                }
                else
                {
                    if (item.Parameter.ABIType is TupleType)
                    {
                        var itemTupleResult = (List<ParameterOutput>)item.Result;
                        AppendObjectTitle(stringBuilder, item.Parameter.Name, level);
                        stringBuilder.AppendLine(ConvertToString(itemTupleResult, level + 1));
                    }
                    else
                    {
                        AppendLevel(stringBuilder, item.Parameter.Name, item.Result, level);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private static string ConvertArrayToString(IEnumerable item, int level)
        {
            var stringBuilder = new StringBuilder();

            if (item == null) return null;
            var counter = 0;
            foreach (var innerItem in item)
            {
                if (innerItem is List<ParameterOutput> innerItemStruct)
                {
                    AppendObjectTitle(stringBuilder, "item[" + counter + "]", level);
                    stringBuilder.AppendLine(ConvertToString(innerItemStruct, level + 1));
                }
                else
                {
                    if (innerItem is IEnumerable array)
                    {
                        var value = innerItem;
                        if (value is byte[] val)
                        {
                            value = val.ToHex();
                            AppendLevel(stringBuilder, "item[" + counter + "]", value, level);
                        }
                        else
                        {

                            AppendObjectTitle(stringBuilder, "item[" + counter + "]", level);
                            stringBuilder.AppendLine(ConvertArrayToString(array, level + 1));
                        }
                    }
                    else
                    {
                        AppendLevel(stringBuilder, "item[" + counter + "]", innerItem, level);
                    }
                }
                counter++;
            }

            return stringBuilder.ToString();
        }

        private static void AppendLevel(StringBuilder builder, string name, object value, int level)
        {
            if (value is byte[] bytes)
            {
                value = bytes.ToHex();
            }

            builder.AppendLine(new string(' ', level * 3) + name + ":" + value.ToString());
        }

        private static void AppendObjectTitle(StringBuilder builder, string name, int level)
        {
            builder.AppendLine(new string(' ', level * 3) + name + ":");
        }


        public static Dictionary<string, object> ConvertToObjectDictionary(this IEnumerable<ParameterOutput> parameterOutputs)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var item in parameterOutputs)
            {
                if (item.Parameter.ABIType is ArrayType)
                {
                    var innerItems = ConvertArrayToObjectDictionary((IEnumerable)item.Result);
                    dictionary.Add(item.Parameter.Name, innerItems);
                }
                else
                {
                    if (item.Parameter.ABIType is TupleType)
                    {
                        var itemTupleResult = (List<ParameterOutput>)item.Result;
                        dictionary.Add(item.Parameter.Name, ConvertToObjectDictionary(itemTupleResult));
                    }
                    else
                    {
                        dictionary.Add(item.Parameter.Name, item.Result);
                    }
                }
            }

            return dictionary;
        }

        private static List<object> ConvertArrayToObjectDictionary(IEnumerable item)
        {
            if (item == null) return null;
            var innerItems = new List<object>();
            foreach (var innerItem in item)
            {

                if (innerItem is string)
                {
                    innerItems.Add(innerItem);
                }
                else
                if (innerItem is List<ParameterOutput> innerItemStruct)
                {
                    innerItems.Add(ConvertToObjectDictionary(innerItemStruct));
                }
                else
                {
                    if (innerItem is IEnumerable array)
                    {
                        innerItems.Add(ConvertArrayToObjectDictionary(array));
                    }
                    else
                    {
                        innerItems.Add(innerItem);
                    }
                }
            }

            return innerItems;
        }

#if !NET35
        public static Dictionary<string, dynamic> ConvertToDynamicDictionary(this IEnumerable<ParameterOutput> parameterOutputs)
        {
            var dictionary = new Dictionary<string, dynamic>();
            foreach (var item in parameterOutputs)
            {
                if (item.Parameter.ABIType is ArrayType)
                {
                    var innerItems = ConvertArrayToDynamicDictionary((IEnumerable)item.Result);
                    dictionary.Add(item.Parameter.Name, innerItems);
                }
                else
                {
                    if (item.Parameter.ABIType is TupleType)
                    {
                        var itemTupleResult = (List<ParameterOutput>)item.Result;
                        dictionary.Add(item.Parameter.Name, ConvertToDynamicDictionary(itemTupleResult));
                    }
                    else
                    {
                        dictionary.Add(item.Parameter.Name, item.Result);
                    }
                }
            }

            return dictionary;
        }

        private static List<dynamic> ConvertArrayToDynamicDictionary(IEnumerable item)
        {
            if (item == null) return null;
            var innerItems = new List<dynamic>();
            foreach (var innerItem in item)
            {
                if (innerItem is string)
                {
                    innerItems.Add(innerItem);
                }
                else if (innerItem is List<ParameterOutput> innerItemStruct)
                {
                    innerItems.Add(ConvertToDynamicDictionary(innerItemStruct));
                }
                else
                {
                    if (innerItem is IEnumerable array)
                    {
                        innerItems.Add(ConvertArrayToDynamicDictionary(array));
                    }
                    else
                    {
                        innerItems.Add(innerItem);
                    }
                }
            }

            return innerItems;
        }

#endif
        public static JObject ConvertToJObject(this IEnumerable<ParameterOutput> parameterOutputs)
        {
            var jObject = new JObject();
            foreach (var item in parameterOutputs)
            {
                if (item.Parameter.ABIType is ArrayType)
                {
                    var innerItems = ConvertToJArray((IEnumerable)item.Result);
                    jObject.Add(item.Parameter.Name, innerItems);
                }
                else
                {
                    if (item.Parameter.ABIType is TupleType)
                    {
                        var itemTupleResult = (List<ParameterOutput>)item.Result;
                        jObject.Add(item.Parameter.Name, ConvertToJObject(itemTupleResult));
                    }
                    else
                    {
                        var value = item.Result;
                        if (value is byte[] val)
                        {
                            value = val.ToHex();
                        }
                        //bigInt as string
                        if (item.Parameter.ABIType is IntType)
                        {
                            value = value.ToString();
                        }

                        jObject.Add(item.Parameter.Name, JToken.FromObject(value));

                    }
                }
            }

            return jObject;
        }

        private static JArray ConvertToJArray(IEnumerable item)
        {
            if (item == null) return null;
            var jArray = new JArray();
            foreach (var innerItem in item)
            {
                if (innerItem is List<ParameterOutput> innerItemStruct)
                {
                    jArray.Add(ConvertToJObject(innerItemStruct));
                }
                else
                {
                    if (innerItem is string)
                    {
                        var value = innerItem;
                        jArray.Add(value);
                    }
                    else if (innerItem is IEnumerable array)
                    {
                        var value = innerItem;
                        if (value is byte[] val)
                        {
                            value = val.ToHex();
                            jArray.Add(value);
                        }
                        else
                        {
                            jArray.Add(ConvertToJArray(array));
                        }

                    }
                    else
                    {
                        var value = innerItem;
                        jArray.Add(value);
                    }
                }
            }

            return jArray;
        }

    }
}