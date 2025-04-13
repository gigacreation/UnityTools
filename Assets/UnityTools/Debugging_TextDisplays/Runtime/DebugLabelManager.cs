using System.Collections.Generic;
using System.Linq;
using GigaCreation.Tools.Ui;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    [RequireComponent(typeof(RectTransform))]
    public class DebugLabelManager : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private TextMeshProUGUI _labelPrefab;

        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private AutoLayoutSupporter _autoLayoutSupporter;

        private readonly Dictionary<int, TextMeshProUGUI> _labels = new();

        private void Reset()
        {
            _transform = transform;
            _autoLayoutSupporter = GetComponent<AutoLayoutSupporter>();
        }

        /// <summary>
        /// デバッグラベルを生成して返します。
        /// </summary>
        /// <param name="priority">ラベルの優先度。この順番でソートされて表示されます。</param>
        /// <returns></returns>
        public TextMeshProUGUI Add(int priority)
        {
            if (_labels.ContainsKey(priority))
            {
                Debug.LogError($"すでに同じ優先度のデバッグラベルが登録されています：{priority}");
                return null;
            }

            var gameObjectName = $"DebugLabel_{priority}";
            TextMeshProUGUI newLabel;

            if (_labelPrefab == null)
            {
                var go = new GameObject(gameObjectName)
                {
                    transform =
                    {
                        parent = _transform,
                        localScale = Vector3.one
                    }
                };

                newLabel = go.AddComponent<TextMeshProUGUI>();
            }
            else
            {
                newLabel = Instantiate(_labelPrefab, _transform);
                newLabel.name = gameObjectName;
            }

            _labels.Add(priority, newLabel);

            TextMeshProUGUI[] sortedLabels = _labels
                .OrderBy(static pair => pair.Key)
                .Select(static pair => pair.Value)
                .ToArray();

            for (var i = 0; i < sortedLabels.Length; i++)
            {
                sortedLabels[i].transform.SetSiblingIndex(i);
            }

            if (_autoLayoutSupporter)
            {
                _autoLayoutSupporter.ExecuteRebuilding();
            }

            return newLabel;
        }

        /// <summary>
        /// 指定されたデバッグラベルを削除します。
        /// </summary>
        /// <param name="priority">削除するラベルの優先度。</param>
        public void Remove(int priority)
        {
            if (!_labels.Remove(priority, out TextMeshProUGUI label))
            {
                Debug.LogWarning($"要求されたラベルが存在しません：{priority}");
                return;
            }

            Destroy(label.gameObject);

            if (_autoLayoutSupporter)
            {
                _autoLayoutSupporter.ExecuteRebuilding();
            }
        }
    }
}
