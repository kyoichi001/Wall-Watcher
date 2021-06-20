﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using RPGM.Gameplay;

[CustomEditor(typeof(ConversationData), true)]
public class AudioClipDataEditor : Editor
{
    ReorderableList m_list;
    ConversationData m_script;

    void OnEnable()
    {
        m_script = target as ConversationData;
        // list = new ReorderableList(serializedObject, serializedObject.FindProperty("items"), true, true, true, true);
        m_list = new ReorderableList(m_script.items, typeof(Conversations), true, true, true, true);
        m_list.drawElementCallback = OnDrawElement;
        //list.onAddCallback += OnAdd;
        m_list.onRemoveCallback += OnRemove;
        m_list.drawHeaderCallback += OnDrawHeader;
        m_list.onSelectCallback += OnSelect;
        m_list.onReorderCallback += OnReorder;
        Undo.undoRedoPerformed -= OnUndoRedo;
        Undo.undoRedoPerformed += OnUndoRedo;
        m_list.elementHeight =  50;
        m_list.onAddDropdownCallback = (rect, list) =>
        {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Conversation"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(m_script, ConversationType.normal);
                }
                );
            menu.AddItem(
                new GUIContent("Event"),
                false,
                () =>
                {
                    ConversationPieceWizard.New(m_script, ConversationType.events);
                }
                );
            menu.DropDown(rect);
        };
    }

    void OnSelect(ReorderableList list)
    {
    }
    void OnReorder(ReorderableList list)
    {
    }
    void OnDrawHeader(Rect rect)
    {
        GUI.Label(rect, "Conversation Script Items");
        if (m_list.list.Count != 0)
            m_script.m_firstConversation = ((Conversations)m_list.list[0]).id;
    }

    void OnUndoRedo()
    {
        if (serializedObject != null)
            serializedObject.Update();
    }


    /// <summary>
    /// reordableListで-ボタンを押したときの動作
    /// </summary>
    /// <param name="list"></param>
    void OnRemove(ReorderableList list)
    {
        var item = m_script.items[list.index];
        Undo.RecordObject(target, "Remove Item");
        m_script.Delete(item.id);
    }
    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = (Conversations)m_list.list[index];
        switch (item.type)
        {
            case ConversationType.normal:
                GUI.color = Color.white;
                break;
            case ConversationType.events:
                GUI.color = new Color32(200, 160, 120,255);
                break;
            default:
                break;
        }
        var idRect = rect;
        idRect.width = rect.width * 0.2f;
        GUI.Label(idRect, item.id, EditorStyles.boldLabel);
        var textRect = rect;
        textRect.x += idRect.width;
        textRect.width = rect.width * 0.7f;
        switch (item.type)
        {
            case ConversationType.normal:
                GUI.Label(textRect, item.text);
                break;
            case ConversationType.events:
                GUI.Label(textRect, item.eventName);
                break;
            default:
                break;
        }


        var targetRect = rect;
        targetRect.y += 15;
        //targetIDが存在しないIDだったり、自身のIDと同じだったら赤文字で表示
        if (!m_script.ContainsKey(item.targetID) || item.targetID == item.id)
        {
            GUI.color = Color.red;
        }
        GUI.Label(targetRect, item.targetID);
        GUI.color = Color.white;

        Rect buttonRect = rect;
        buttonRect.width = rect.width / 5;
        buttonRect.height = rect.height;
        buttonRect.x = rect.x + rect.width / 5 * 4;
        if (m_list.index == index)
        {
            if (GUI.Button(buttonRect, "Edit", EditorStyles.miniButton))
            {
                ConversationPieceWizard.Edit(m_script, item);
            }
        }
    }

    public override void OnInspectorGUI()
    {

       // EditorGUILayout.PrefixLabel("Left Talker");
        //script.m_left = (TalkerData)EditorGUILayout.ObjectField(script.m_left, typeof(TalkerData), false);
       // EditorGUILayout.PrefixLabel("Right Talker");
        //script.m_right = (TalkerData)EditorGUILayout.ObjectField(script.m_right, typeof(TalkerData), false);
        GUI.enabled = false;
        EditorGUILayout.TextField("FirstConversation", m_script.m_firstConversation);
        GUI.enabled = true;
        EditorGUILayout.Space();
        //var questProperty = serializedObject.FindProperty("quest");
       // if (questProperty != null)
        //    EditorGUILayout.PropertyField(questProperty, true);
        serializedObject.ApplyModifiedProperties();
        m_list.DoLayoutList();
    }
}
