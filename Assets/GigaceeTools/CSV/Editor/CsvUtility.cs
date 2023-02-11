using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GigaceeTools.Csv
{
    public static class CsvUtility
    {
        /// <summary>
        /// CSV の指定した列のリストを生成します。
        /// </summary>
        /// <param name="csvPath">CSV のパス。</param>
        /// <param name="columnNum">リスト化する列のインデックス。</param>
        /// <param name="hasHeader">true なら、CSV の 1 行目をヘッダーとして扱います。</param>
        /// <returns>生成されたリスト。</returns>
        public static List<string> GenerateList(string csvPath, int columnNum, bool hasHeader = false)
        {
            var csv = AssetDatabase.LoadAssetAtPath<TextAsset>(csvPath);

            if (!csv)
            {
                Debug.LogError($"指定された CSV が存在していません：{csvPath}");
                return null;
            }

            var reader = new StringReader(csv.text);
            var result = new List<string>();

            if (hasHeader)
            {
                // ヘッダーをスキップする
                reader.ReadLine();
            }

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] splitLine = line.Split(',');

                if (columnNum >= splitLine.Length)
                {
                    continue;
                }

                result.Add(splitLine[columnNum]);
            }

            return result;
        }

        /// <summary>
        /// CSV の指定した列を Key, Value にした辞書を生成します。
        /// </summary>
        /// <param name="csvPath">CSV の名前（拡張子を除いたファイル名）。</param>
        /// <param name="keyColumnNum">Key にする列。</param>
        /// <param name="hasHeader">true なら、CSV の 1 行目をヘッダーとして扱います。</param>
        /// <param name="valueColumnNums">Value にする列。複数指定すると、それらの文字列を結合します。</param>
        /// <returns>生成した辞書。</returns>
        public static Dictionary<string, string> GenerateDictionary(
            string csvPath, int keyColumnNum, bool hasHeader = false, params int[] valueColumnNums
        )
        {
            var csv = AssetDatabase.LoadAssetAtPath<TextAsset>(csvPath);

            if (!csv)
            {
                Debug.LogError($"指定された CSV が存在していません：{csvPath}");
                return null;
            }

            var reader = new StringReader(csv.text);
            var result = new Dictionary<string, string>();

            if (hasHeader)
            {
                // ヘッダーをスキップする
                reader.ReadLine();
            }

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] splitLine = line.Split(',');

                if ((keyColumnNum >= splitLine.Length) || (Mathf.Max(valueColumnNums) >= splitLine.Length))
                {
                    continue;
                }

                result.Add(splitLine[keyColumnNum], string.Join("", valueColumnNums.Select(x => splitLine[x])));
            }

            return result;
        }

        /// <summary>
        /// CSV の指定した列を Key, Value にした辞書を生成します。
        /// </summary>
        /// <param name="csvPath">CSV の名前（拡張子を除いたファイル名）。</param>
        /// <param name="valueColumnNum">Value にする列。</param>
        /// <param name="hasHeader">true なら、CSV の 1 行目をヘッダーとして扱います。</param>
        /// <param name="keyColumnNums">Key にする列。複数指定すると、それらの文字列を結合します。</param>
        /// <returns>生成した辞書。</returns>
        public static Dictionary<string, string> GenerateKeyMergedDictionary(
            string csvPath, int valueColumnNum, bool hasHeader = false, params int[] keyColumnNums
        )
        {
            var csv = AssetDatabase.LoadAssetAtPath<TextAsset>(csvPath);

            if (!csv)
            {
                Debug.LogError($"指定された CSV が存在していません：{csvPath}");
                return null;
            }

            var reader = new StringReader(csv.text);
            var result = new Dictionary<string, string>();

            if (hasHeader)
            {
                // ヘッダーをスキップする
                reader.ReadLine();
            }

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] splitLine = line.Split(',');

                if ((Mathf.Max(keyColumnNums) >= splitLine.Length) || (valueColumnNum >= splitLine.Length))
                {
                    continue;
                }

                result.Add(string.Join("", keyColumnNums.Select(x => splitLine[x])), splitLine[valueColumnNum]);
            }

            return result;
        }
    }
}
