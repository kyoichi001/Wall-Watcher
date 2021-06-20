using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.Playables;

namespace RPGM.Gameplay
{

    public enum ConversationType
    {
        normal=0,
        events=1,
        subConversation=2,
    }

    [System.Serializable]
    public class Conversations
    {
       public ConversationType type;

        public string id;
        public string targetID;

        [Multiline]
        public string text;
        public string talker;
        //public TalkData.TalkType talkType;
        //public TalkData.FaceType talkFace;
        public PlayableDirector playableDirector;
        //�e�N�X�g��\������Ƃ��ɏo����ʉ�
        public AudioClip audio;
        public QuestDataSO quest;
        //�I����
        public List<ConversationOption> options;
        //�C�x���g���s�p�i�g���ĂȂ��j
        public string eventName;
        //�ʂ̉�b�ւ̃����N�i�g���ĂȂ��j
        public Conversations subConversation;
    }

}