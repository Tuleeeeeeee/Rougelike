using Enums;
using UnityEngine;

namespace Dungeon
{
    [System.Serializable]
    public class Doorway
    {
        public Vector2Int position;
        public Orientation orientation;
        public GameObject doorPrefab;

        #region Header

        [Header("The upper left position to start copyinng from")]
        #endregion 
        public Vector2Int doorwayStartCopyPosition;

        #region Header

        [Header("The width of tiles im tje doorway to copy over")]

        #endregion

        public int doorwayCopyTileWidth;

        #region Header

        [Header("The height of tiles im tje doorway to copy over")]

        #endregion

        public int doorwayCopyTileHeight;

        [HideInInspector]
        public bool isConnected = false;

        [HideInInspector]
        public bool isUnavailable = false;
    }
}