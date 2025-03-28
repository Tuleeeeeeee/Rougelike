﻿using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// empty string check
    /// </summary>
    public static class HelperUtilities
    {

        public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
        {
            if (stringToCheck == "")
            {
                Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// List empty or contains null value check - returns true if there is an error
        /// </summary>
        public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
        {
            bool error = false;
            int count = 0;

            if (enumerableObjectToCheck == null)
            {
                Debug.Log(fieldName + "is null in object" + thisObject.name.ToString());
                return true;
            }

            foreach (var item in enumerableObjectToCheck)
            {
                if (item == null)
                {
                    Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                    error = true;
                }
                else
                {
                    count++;
                }
            }

            if (count == 0)
            {
                Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
                error = true;
            }

            return error;
        }
        public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
        {
            if (objectToCheck == null)
            {
                Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
                return true;
            }
            return false;
        }
    }
}