
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph
{
    [CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
    public class RoomNodeGraphSO : ScriptableObject
    {
        [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
        [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
        [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();



        private void Awake()
        {
            LoadRoomNodeDictionary();
        }

        private void LoadRoomNodeDictionary()
        {
            roomNodeDictionary.Clear();

            foreach (RoomNodeSO roomNode in roomNodeList)
            {
                roomNodeDictionary[roomNode.id] = roomNode;
            }
        }
        public RoomNodeSO GetRoomNode(string roomNodeId)
        {
            if (roomNodeDictionary.TryGetValue(roomNodeId, out RoomNodeSO roomNode))
            {
                return roomNode;
            }
            return null;
        }

        public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoomNode)
        {
            foreach (string childRoomNodeID in parentRoomNode.childRoomNodeIDList)
            {
                yield return GetRoomNode(childRoomNodeID);
            }
        }

        public RoomNodeSO GetRoomNode(RoomNodeTypeSO roomNodeType)
        {
            foreach (RoomNodeSO node in roomNodeList)
            {
                if (node.roomNodeType == roomNodeType)
                {
                    return node;
                }
            }
            return null;
        }

        #region Code
#if UNITY_EDITOR
        [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
        [HideInInspector] public Vector2 linePosition;

        public void OnValidate()
        {
            LoadRoomNodeDictionary();
        }

        public void SetNodeToDrawConnectionLineFrom(RoomNodeSO roomNode, Vector2 position)
        {
            roomNodeToDrawLineFrom = roomNode;
            linePosition = position;
        }
#endif
        #endregion
    }

}
