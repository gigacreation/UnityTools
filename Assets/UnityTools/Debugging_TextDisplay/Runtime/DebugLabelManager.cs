using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
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

        public TextMeshProUGUI Add(string name, int priority)
        {
            if (_labels.ContainsKey(priority))
            {
                Debug.LogError($"すでに同じ優先度のデバッグラベルが登録されています：{name}, {priority}");
                return null;
            }

            TextMeshProUGUI newLabel;

            if (_labelPrefab == null)
            {
                var go = new GameObject(name)
                {
                    transform =
                    {
                        parent = _transform,
                        localScale = Vector3.one
                    }
                };

                newLabel = go.AddComponent<TextMeshProUGUI>();
                newLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
            }
            else
            {
                newLabel = Instantiate(_labelPrefab, _transform);
                newLabel.name = name;
            }

            _labels.Add(priority, newLabel);

            TextMeshProUGUI[] sortedLabels = _labels
                .OrderBy(pair => pair.Key)
                .Select(pair => pair.Value)
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

        public void Remove(string className)
        {
            KeyValuePair<int, TextMeshProUGUI> childPair
                = _labels.SingleOrDefault(pair => (pair.Value != null) && (pair.Value.name == className));

            if (!childPair.Value)
            {
                Debug.LogWarning($"要求されたラベルが存在しません: {className}");
                return;
            }

            _labels.Remove(childPair.Key);
            Destroy(childPair.Value.gameObject);

            if (_autoLayoutSupporter)
            {
                _autoLayoutSupporter.ExecuteRebuilding();
            }
        }
    }
}
