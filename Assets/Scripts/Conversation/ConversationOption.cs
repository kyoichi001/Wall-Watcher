using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// A choice in a conversation script.
    /// 選択肢
    /// </summary>
    [System.Serializable]
    public struct ConversationOption
    {
        public string text;//はい・いいえなど
        public AudioClip audio;
        public string targetId;
        public bool enabled;

    }
}