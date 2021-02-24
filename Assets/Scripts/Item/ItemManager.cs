﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Kyoichi
{
    public class ItemManager : SingletonMonoBehaviour<ItemManager>, ILoadableAsync,ISaveable
    {
        enum LoadState
        {
            notLoaded,
            loading,
            loaded
        }
        LoadState m_state = LoadState.notLoaded;

        [SerializeField] private AssetLabelReference _labelReference;
        Dictionary<string, ItemSO> m_data = new Dictionary<string, ItemSO>();
        public IEnumerable<KeyValuePair<string, ItemSO>> Data
        {
            get
            {
                return m_data;
            }
        }
        List<Inventry> inventries = new List<Inventry>();
        public void AddInventry(Inventry inventry) { inventries.Add(inventry); }

        // Start is called before the first frame update
        void Awake()
        {
            SaveLoadManager.Instance.SetLoadable(this);
            SaveLoadManager.Instance.SetSaveable(this);
            if (m_state == LoadState.loaded)//エディタ上でロードしたとき
            {
                Addressables.Release(m_handle);
                m_state = LoadState.notLoaded;
            }
        }

        AsyncOperationHandle<IList<ItemSO>> m_handle;
        private CancellationTokenSource _cancellationTokenSource;
        //ゲーム開始時の部屋を読み込む
        public async Task Load(CancellationToken cancellationToken)
        {
            if (m_state != LoadState.notLoaded)
            {
                Debug.Log("<color=#4a19bd>item loaded or loading</color>");
                return;
            }
            Debug.Log("<color=#4a19bd>Try load item</color>");
            m_data.Clear();
            m_state = LoadState.loading;
            m_handle = Addressables.LoadAssetsAsync<ItemSO>(_labelReference, null);
            await m_handle.Task;
            foreach (var res in m_handle.Result)
            {
                Debug.Log($"<color=#4a19bd>item '{res.name}'</color>");
                m_data.Add(res.name, res);
            }
            m_state = LoadState.loaded;
        }

        public void Save()
        {
            for (int i=0;i<inventries.Count;)
            {
                inventries[i].SaveToFile();
                inventries.RemoveAt(i);
            }
        }

        void OnDisable()
        {
            Addressables.Release(m_handle);
        }
        public List<Kyoichi.ItemStack> LoadItemFrom(string directory, string filename)
        {
            var res = new List<Kyoichi.ItemStack>();
            if (m_state != LoadState.loaded)
            {
                Debug.LogError("item not loaded");
                return res;
            }
            var dat = CSVReader.Read(directory, filename);
            foreach (var i in dat)
            {
                if (!m_data.ContainsKey(i[0]))
                {
                    Debug.Log($"error item not found '{i[0]}'");
                    continue;
                }
                res.Add(
                    new Kyoichi.ItemStack(GetItem(i[0]), int.Parse(i[1]))
                    );
            }
            return res;
        }
        public void SaveItemTo(string directory, string filename, IEnumerable<Kyoichi.ItemStack> target)
        {
            var dat = new List<List<string>>();
            foreach (var i in target)
            {
                var lis = new List<string>();
                lis.Add(i.item.name);
                lis.Add(i.count.ToString());
                dat.Add(lis);
            }
            CSVReader.Write(directory, filename, dat);
        }
        public ItemSO GetItem(string name)
        {
            if (m_state != LoadState.loaded)
            {
                Debug.LogError("item not loaded");
                return null;
            }
            if (!m_data.ContainsKey(name))
            {
                Debug.LogError($"item not exist '{name}'");
                return m_data["error"];
            }
            return m_data[name];
        }

    }
}
