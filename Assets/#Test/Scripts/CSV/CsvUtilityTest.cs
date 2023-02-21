using System.Collections.Generic;
using GigaCreation.Tools.Csv;
using UnityEngine;

namespace GigaCreation.Tools.Test
{
    public class CsvUtilityTest : MonoBehaviour
    {
        [SerializeField] private bool _testList;
        [SerializeField] private bool _testDictionary;

        [Space]
        [SerializeField] private string _csvPath;
        [SerializeField] private bool _hasHeader;
        [SerializeField] private int[] _keyColumnIndexes;
        [SerializeField] private int[] _valueColumnIndexes;

        private void Start()
        {
            if (_testList)
            {
                TestListExtracting();
            }

            if (_testDictionary)
            {
                TestDictionaryExtracting();
            }
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
