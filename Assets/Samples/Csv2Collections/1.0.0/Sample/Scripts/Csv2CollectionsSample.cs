using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Csv2Collections.Sample
{
    public class Csv2CollectionsSample : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _keysLabel;
        [SerializeField] private TextMeshProUGUI _valuesLabel;
        [SerializeField] private TextMeshProUGUI _equalityOperatorLabel;

        [Header("Parameters")]
        [SerializeField] private TestTarget _testTarget;
        [SerializeField] private string _csvPath;
        [SerializeField] private bool _hasHeader;
        [SerializeField] private string _separator;
        [SerializeField] private int[] _keyColumnIndexes;
        [SerializeField] private int[] _valueColumnIndexes;

        private readonly StringBuilder _keysBuilder = new();
        private readonly StringBuilder _valuesBuilder = new();
        private readonly StringBuilder _equalityOperatorBuilder = new();

        private void Start()
        {
            string csv = Resources.Load<TextAsset>(_csvPath).text;

            switch (_testTarget)
            {
                case TestTarget.None:
                    break;

                case TestTarget.List:
                    TestListExtracting(csv);
                    break;

                case TestTarget.Dictionary:
                    TestDictionaryExtracting(csv);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TestListExtracting(string csv)
        {
            var request = new CsvExtractRequest(csv, _hasHeader, _separator, _valueColumnIndexes);
            List<string> response = CsvUtility.ExtractIntoList(request);

            if (response == null)
            {
                return;
            }

            ClearStringBuilders();

            for (var i = 0; i < response.Count; i++)
            {
                _keysBuilder.AppendLine($"[{i}]");
                _valuesBuilder.AppendLine(response[i]);
                _equalityOperatorBuilder.AppendLine("=");
            }

            _keysLabel.SetText(_keysBuilder);
            _valuesLabel.SetText(_valuesBuilder);
            _equalityOperatorLabel.SetText(_equalityOperatorBuilder);
        }

        private void TestDictionaryExtracting(string csv)
        {
            var request = new CsvExtractRequest(csv, _hasHeader, _separator, _valueColumnIndexes);
            request.SetKeyColumnIndexes(_keyColumnIndexes);
            Dictionary<string, string> response = CsvUtility.ExtractIntoDictionary(request);

            if (response == null)
            {
                return;
            }

            ClearStringBuilders();

            foreach ((string key, string value) in response)
            {
                _keysBuilder.AppendLine($"[\"{key}\"]");
                _valuesBuilder.AppendLine(value);
                _equalityOperatorBuilder.AppendLine("=");
            }

            _keysLabel.SetText(_keysBuilder);
            _valuesLabel.SetText(_valuesBuilder);
            _equalityOperatorLabel.SetText(_equalityOperatorBuilder);
        }

        private void ClearStringBuilders()
        {
            _keysBuilder.Clear();
            _valuesBuilder.Clear();
            _equalityOperatorBuilder.Clear();
        }

        private enum TestTarget
        {
            None,
            List,
            Dictionary
        }
    }
}
