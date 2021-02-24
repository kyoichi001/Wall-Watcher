using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// A choice in a conversation script.
    /// �I����
    /// </summary>
    [System.Serializable]
    public struct ConversationOption
    {
        public string text;//�͂��E�������Ȃ�
        public AudioClip audio;
        public string targetId;
        public bool enabled;

    }
}