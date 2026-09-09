using System.Collections.Generic;
using System.Linq;
using GigaCreation.Tools.Ui;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugLabelControl : MonoBehaviour
    {
        [SerializeField] private GameObject _debugTextPrefab;
        [SerializeField] private AutoLayoutSupporter _autoLayoutSupporter;

        private readonly Dictionary<int, GameObject> _debugTexts = new();

        protected IEnumerable<GameObject> SortedDebugTexts => _debugTexts
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
        /// <param name="emphasis">true なら、ラベルを強調します。</param>
        /// <returns>生成したラベル。</returns>
        public TextMeshProUGUI Add(int priority, bool emphasis)
        {
            if (_debugTexts.ContainsKey(priority))
            {
                Debug.LogError($"すでに同じ優先度のデバッグラベルが登録されています：{priority}");
                return null;
            }

            var newGameObject = CreateLabel($"DebugLabel_{priority}", emphasis);
            _debugTexts.Add(priority, newGameObject);
            SortLabels();
            RebuildLayout();
            return newGameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// 指定されたデバッグラベルを削除します。
        /// </summary>
        /// <param name="priority">削除するラベルの優先度。</param>
        public void Remove(int priority)
        {
            if (!_debugTexts.Remove(priority, out var label))
            {
                Debug.LogWarning($"要求されたラベルが存在しません：{priority}");
                return;
            }

            Destroy(label);
            SortLabels();
            RebuildLayout();
        }

        protected virtual GameObject CreateLabel(string gameObjectName, bool emphasis)
        {
            GameObject go;

            if (_debugTextPrefab != null)
            {
                go = Instantiate(_debugTextPrefab, transform);
                go.name = gameObjectName;
            }
            else
            {
                go = new GameObject(gameObjectName, typeof(TextMeshProUGUI));
                go.transform.SetParent(transform);
                go.transform.localScale = Vector3.one;
            }

            if (emphasis)
            {
                Emphasis(go);
            }

            return go;
        }

        protected virtual void Emphasis(GameObject go)
        {
            var label = go.GetComponentInChildren<TextMeshProUGUI>();
            label.color = Color.red;
            label.fontStyle = FontStyles.Bold;
        }

        protected virtual void SortLabels()
        {
            var sorted = SortedDebugTexts.ToArray();

            for (var i = 0; i < sorted.Length; i++)
            {
                sorted[i].transform.SetSiblingIndex(i);
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
