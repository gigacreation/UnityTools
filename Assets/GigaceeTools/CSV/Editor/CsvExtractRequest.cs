using UnityEngine;

namespace GigaceeTools.Csv
{
    public class CsvExtractRequest
    {
        /// <summary>
        /// CSV のファイルパス。
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// true なら、CSV の 1 行目をヘッダーとして扱います。
        /// </summary>
        public bool HasHeader { get; }

        /// <summary>
        /// 値として抽出する列のインデックス。
        /// </summary>
        public int[] ValueColumnIndexes { get; }

        /// <summary>
        /// キーとして抽出する列のインデックス。
        /// </summary>
        public int[] KeyColumnIndexes { get; private set; }

        public CsvExtractRequest(string path, bool hasHeader = false, params int[] valueIndexes)
        {
            Path = path;
            HasHeader = hasHeader;
            ValueColumnIndexes = valueIndexes;
        }

        public void SetKeyColumnIndexes(params int[] indexes)
        {
            KeyColumnIndexes = indexes;
        }

        public int GetMaxTargetColumnIndex()
        {
            return Mathf.Max(
                KeyColumnIndexes == null ? 0 : Mathf.Max(KeyColumnIndexes),
                ValueColumnIndexes == null ? 0 : Mathf.Max(ValueColumnIndexes)
            );
        }
    }
}
