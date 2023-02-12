using System.Collections.Generic;
using GigaceeTools.Csv;
using UnityEngine;

namespace GigaceeTools.Sample
{
    public class CsvUtilitySample : MonoBehaviour
    {
        [SerializeField] private string _csvPath;
        [SerializeField] private bool _hasHeader;
        [SerializeField] private int[] _keyColumnIndexes;
        [SerializeField] private int[] _valueColumnIndexes;

        private void Start()
        {
            TestListExtracting();
            TestDictionaryExtracting();
        }

        private void TestListExtracting()
        {
            var request = new CsvExtractRequest(_csvPath, _hasHeader, _valueColumnIndexes);
            List<string> response = CsvUtility.ExtractIntoList(request);

            if (response == null)
            {
                return;
            }

            foreach (string value in response)
            {
                Debug.Log(value);
            }
        }

        private void TestDictionaryExtracting()
        {
            var request = new CsvExtractRequest(_csvPath, _hasHeader, _valueColumnIndexes);
            request.SetKeyColumnIndexes(_keyColumnIndexes);
            Dictionary<string, string> response = CsvUtility.ExtractIntoDictionary(request);

            if (response == null)
            {
                return;
            }

            foreach ((string key, string value) in response)
            {
                Debug.Log($"{key}: {value}");
            }
        }
    }
}
