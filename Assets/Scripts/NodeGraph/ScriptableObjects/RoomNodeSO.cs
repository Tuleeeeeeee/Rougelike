using Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NodeGraph
{
    public class RoomNodeSO : ScriptableObject
    {
        [HideInInspector] public string id;
        /*[HideInInspector]*/
        public List<string> parentRoomNodeIDList = new List<string>();
        /*[HideInInspector]*/
        public List<string> childRoomNodeIDList = new List<string>();
        [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
        public RoomNodeTypeSO roomNodeType;
        [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;


        #region Editor
#if UNITY_EDITOR
        [HideInInspector] public Rect rect;
        [HideInInspector] public bool isLeftClickDragging;
        [HideInInspector] public bool isSelected;
        public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
        {
            this.rect = rect;
            this.id = Guid.NewGuid().ToString();
            this.name = "RoomNode";
            this.roomNodeGraph = nodeGraph;
            this.roomNodeType = roomNodeType;


            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }


        public void Draw(GUIStyle nodeStyle)
        {
            GUILayout.BeginArea(rect, nodeStyle);

            EditorGUI.BeginChangeCheck();

            if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
            {
                EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
            }
            else
            {

                int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

                int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypeToDisplay());

                roomNodeType = roomNodeTypeList.list[selection];

                if (roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor ||
                    !roomNodeTypeList.list[selected].isCorridor && roomNodeTypeList.list[selection].isCorridor ||
                    !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list[selection].isBossRoom)
                {
                    if (childRoomNodeIDList.Count > 0)
                    {
                        for (int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                        {
                            RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);

                            if (childRoomNode != null)
                            {
                                RemoveChildRoomNodeIdFromRoomNode(childRoomNode.id);

                                childRoomNode.RemoveParentRoomNodeIdFromRoomNode(id);
                            }
                        }
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndArea();
        }

        private string[] GetRoomNodeTypeToDisplay()
        {
            string[] roomArray = new string[roomNodeTypeList.list.Count];

            for (int i = 0; i < roomNodeTypeList.list.Count; i++)
            {
                if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
                {
                    roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
                }
            }
            return roomArray;
        }

        public void ProcessEvents(Event currentEvent)
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    ProcessMouseDownEvent(currentEvent);
                    break;
                case EventType.MouseUp:
                    ProcessMouseUpEvent(currentEvent);
                    break;
                case EventType.MouseDrag:
                    ProcessMouseDragEvent(currentEvent);
                    break;
                default:
                    break;
            }
        }


        private void ProcessMouseDownEvent(Event currentEvent)
        {
            if (currentEvent.button == 0) // left click
            {
                ProcessLeftClickDownEvent();
            }
            else if (currentEvent.button == 1)
            {
                ProcessRightClickDownEvent(currentEvent);
            }
        }

        private void ProcessLeftClickDownEvent()
        {
            Selection.activeObject = this;
            if (isSelected)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;
            }
        }
        private void ProcessRightClickDownEvent(Event currentEvent)
        {
            roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
        }
        private void ProcessMouseUpEvent(Event currentEvent)
        {
            if (currentEvent.button == 0) // left click
            {
                ProcessLeftClickUpEvent(currentEvent);
            }
        }
        private void ProcessLeftClickUpEvent(Event currentEvent)
        {
            if (isLeftClickDragging)
            {
                isLeftClickDragging = false;
            }
        }
        private void ProcessMouseDragEvent(Event currentEvent)
        {
            if (currentEvent.button == 0) // left click 
            {
                ProcessLeftClickDragEvent(currentEvent);
            }
        }

        private void ProcessLeftClickDragEvent(Event currentEvent)
        {
            isLeftClickDragging = true;
            DragNode(currentEvent.delta);
            GUI.changed = true;
        }

        public void DragNode(Vector2 delta)
        {
            rect.position += delta;
            EditorUtility.SetDirty(this);
        }


        public bool AddChildRoomNodeIdToRoomNode(string childId)
        {
            if (IsChildRoomValid(childId))
            {
                childRoomNodeIDList.Add(childId);
                return true;
            }
            return false;
        }

        private bool IsChildRoomValid(string childId)
        {
            bool isConnectBossRoomAlready = false;

            foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
            {
                if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
                {
                    isConnectBossRoomAlready = true;
                }
            }

            if (roomNodeGraph.GetRoomNode(childId).roomNodeType.isBossRoom && isConnectBossRoomAlready)
            {
                return false;
            }

            if (roomNodeGraph.GetRoomNode(childId).roomNodeType.isNone)
            {
                return false;
            }

            if (childRoomNodeIDList.Contains(childId))
            {
                return false;
            }

            if (id == childId)
            {
                return false;
            }

            if (parentRoomNodeIDList.Contains(childId))
            {
                return false;
            }

            if (roomNodeGraph.GetRoomNode(childId).parentRoomNodeIDList.Count > 0)
            {
                return false;
            }

           /* if (roomNodeGraph.GetRoomNode(childId).parentRoomNodeIDList.Count > Settings.maxChildConnection)
            {
                return false;
            }*/

            if (roomNodeGraph.GetRoomNode(childId).roomNodeType.isCorridor && roomNodeType.isCorridor)
            {
                return false;
            }

            if (!roomNodeGraph.GetRoomNode(childId).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            {
                return false;
            }

            if (roomNodeGraph.GetRoomNode(childId).roomNodeType.isCorridor && childRoomNodeIDList.Count > Settings.maxChildCorridors)
            {
                return false;
            }

            if (roomNodeGraph.GetRoomNode(childId).roomNodeType.isEntrance)
            {
                return false;
            }

            if (!roomNodeGraph.GetRoomNode(childId).roomNodeType.isCorridor & childRoomNodeIDList.Count > 0)
            {
                return false;
            }
            return true;
        }

        public bool AddParentRoomNodeIdToRoomNode(string parentId)
        {
            parentRoomNodeIDList.Add(parentId);
            return true;
        }

        public bool RemoveChildRoomNodeIdFromRoomNode(string childId)
        {
            if (childRoomNodeIDList.Contains(childId))
            {
                childRoomNodeIDList.Remove(childId);
                return true;
            }
            return false;
        }
        public bool RemoveParentRoomNodeIdFromRoomNode(string parentId)
        {
            if (parentRoomNodeIDList.Contains(parentId))
            {
                parentRoomNodeIDList.Remove(parentId);
                return true;
            }
            return false;
        }

#endif
        #endregion
    }
}