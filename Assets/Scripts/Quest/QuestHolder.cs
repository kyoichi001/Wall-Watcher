﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーが請け負ったクエストを管理するクラス
/// </summary>
[DisallowMultipleComponent]
public class QuestHolder : MonoBehaviour
{
    [SerializeField,ReadOnly]
    /// <summary>
    /// 請け負った、または完了したクエスト
    /// </summary>
    List<QuestDataSO> m_quests = new List<QuestDataSO>();
    public IEnumerable<QuestDataSO> Data { get => m_quests; }
    private void Start()
    {
        OnQuestAdd.AddListener(() => {
            EffectGenerator.Instance.Generate("quest");

        
        });
    }
    public void AddQuest(QuestDataSO quest)
    {
        m_quests.Add(quest);
        OnQuestAdd.Invoke();
    }
    /// <summary>
    /// クエストを請け負ったときに実行されるイベント
    /// </summary>
    public UnityEvent OnQuestAdd { get; } = new UnityEvent();

}
