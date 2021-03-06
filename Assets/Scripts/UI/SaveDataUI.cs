﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// セーブデータ１つ１つの表示を制御する
/// </summary>
public class SaveDataUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI chapterNum;
    [SerializeField] Button button;
    [SerializeField,ReadOnly]int index;
    public void Initialize(string dataFileName,string chapterNum_,int saveFileIndex,UnityAction<int> func)
    {
        index = saveFileIndex;
        text.text = dataFileName;
        chapterNum.text = chapterNum_;
        button.onClick.AddListener(() =>
        {
            func(index);
        });
    }
}
