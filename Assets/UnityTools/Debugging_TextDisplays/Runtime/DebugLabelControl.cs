using System.Collections.Generic;
using System.Linq;
using GigaCreation.Tools.Ui;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugLabelControl : MonoBehaviour
    {
        [SerializeField] private GameObject _labelPrefab;
        [SerializeField] private AutoLayoutSupporter _autoLayoutSupporter;

        private readonly Dictionary<int, TextMeshProUGUI> _labels = new();

        protected IEnumerable<TextMeshProUGUI> SortedLabels => _labels
            .OrderBy(static pair => pair.Key)
            .Select(static pair => pair.Value);

        private void Reset()
        {
            _autoLayoutSupporter = GetComponent<AutoLayoutSupporter>();
        }

        /// <summary>
        /// デバッグラベルを生成して返します。
        /// </summary>
        /// <param name="priority">ラベルの優先度。この順番でソートされて表示されます。</param>
        /// <returns>生成したラベル。</returns>
        public TextMeshProUGUI Add(int priority)
        {
            if (_labels.ContainsKey(priority))
            {
                Debug.LogError($"すでに同じ優先度のデバッグラベルが登録されています：{priority}");
                return null;
            }

            var newLabel = CreateLabel($"DebugLabel_{priority}");
            _labels.Add(priority, newLabel);
            SortLabels();
            RebuildLayout();
            return newLabel;
        }

        /// <summary>
        /// 指定されたデバッグラベルを削除します。
        /// </summary>
        /// <param name="priority">削除するラベルの優先度。</param>
        public void Remove(int priority)
        {
            if (!_labels.Remove(priority, out var label))
            {
                Debug.LogWarning($"要求されたラベルが存在しません：{priority}");
                return;
            }

            Destroy(label.gameObject);
            SortLabels();
            RebuildLayout();
        }

        protected virtual TextMeshProUGUI CreateLabel(string gameObjectName)
        {
            GameObject go;

            if (_labelPrefab != null)
            {
                go = Instantiate(_labelPrefab, transform);
                go.name = gameObjectName;
                return go.GetComponentInChildren<TextMeshProUGUI>();
            }

            go = new GameObject(gameObjectName)
            {
                transform =
                {
                    parent = transform,
                    localScale = Vector3.one
                }
            };

            return go.AddComponent<TextMeshProUGUI>();
        }

        protected virtual void SortLabels()
        {
            var sortedLabels = SortedLabels.ToArray();

            for (var i = 0; i < sortedLabels.Length; i++)
            {
                sortedLabels[i].transform.SetSiblingIndex(i);
            }
        }

        private void RebuildLayout()
        {
            if (_autoLayoutSupporter)
            {
                _autoLayoutSupporter.ExecuteRebuilding();
            }
        }
    }
}
