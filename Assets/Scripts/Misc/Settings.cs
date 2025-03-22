using System.Collections;
using UnityEngine;

namespace Misc
{
    public static class Settings
    {
        #region DUNGEON BUILD SETTINGS
        public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
        public const int maxDungeonBuildAttempts = 10;
        #endregion

        #region ROOM SETTINGS
        public const float fadeInTime = 0.5f;
        public const int maxChildCorridors = 3;
        public const int maxChildConnection = 2;
        public const float doorUnlockDelay = 1f;
        #endregion

        #region ANIMATOR PARAMETERS
        public static int open = Animator.StringToHash("open");
        public static int destroy = Animator.StringToHash("destroy");
        public static string stateDestroyed = "Destroyed";
        #endregion

        #region GAMEOBJECT TAGS
        public const string playerTag = "Player";
        public const string playerWeapon = "playerWeapon";
        #endregion
    }
}