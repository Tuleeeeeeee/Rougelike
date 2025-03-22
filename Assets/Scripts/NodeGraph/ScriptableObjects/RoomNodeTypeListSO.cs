using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace NodeGraph
{
    [CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]
    public class RoomNodeTypeListSO : ScriptableObject
    {
        #region Header ROOM NODE TYPE LIST
        [Space(10)]
        [Header("ROOM NODE TYPE LIST")]
        #endregion

        #region Tooltip
        [Tooltip("This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
        #endregion

        public List<RoomNodeTypeSO> list;

#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
        }
#endif
    }
}