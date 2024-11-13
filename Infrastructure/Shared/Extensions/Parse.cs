using System;
using System.Globalization;
using UnityEngine;

namespace Infrastructure.Shared.Extensions
{
    public static class Parse
    {
        public static float Float(string value, out float parsedValue)
        {
            try
            {
                parsedValue = float.Parse(value, CultureInfo.InvariantCulture);
                return parsedValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"Cannot parse '{value}'");
                Debug.LogException(e);
                parsedValue = 0;
                return 0;
            }
        }

        public static float Float(string value, out float parsedValue, string uid)
        {
            try
            {
                parsedValue = float.Parse(value, CultureInfo.InvariantCulture);
                return parsedValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"Cannot parse '{value}' with uid '{uid}'");
                Debug.LogException(e);
                parsedValue = 0;
                return 0;
            }
        }
    }
}