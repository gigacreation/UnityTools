using System;
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
        /// CSV の指定された列を要素に持ったリストを作成して返します。
        /// </summary>
        /// <param name="request">抽出リクエストのデータ。</param>
        /// <returns>抽出したリスト。</returns>
        public static List<string> ExtractIntoList(CsvExtractRequest request)
        {
            if (!(request.ValueColumnIndexes?.Length > 0))
            {
                Debug.LogError("抽出する列のインデックスが指定されていません。");
                return null;
            }

            if (!TryLoadCsv(request.Path, out TextAsset csv))
            {
                Debug.LogError($"指定された CSV が存在していません：{request.Path}");
                return null;
            }

            var result = new List<string>();

            Extract(csv.text, request.HasHeader, request.GetMaxTargetColumnIndex(), splitLine =>
            {
                result.Add(string.Join("", request.ValueColumnIndexes.Select(idx => splitLine[idx])));
            });

            return result;
        }

        /// <summary>
        /// CSV の指定された列をキー・値とした辞書を作成して返します。
        /// </summary>
        /// <param name="request">抽出リクエストのデータ。</param>
        /// <returns>抽出した辞書。</returns>
        public static Dictionary<string, string> ExtractIntoDictionary(CsvExtractRequest request)
        {
            if (!(request.KeyColumnIndexes?.Length > 0) || !(request.ValueColumnIndexes?.Length > 0))
            {
                Debug.LogError("抽出する列のインデックスが指定されていません。");
                return null;
            }

            if (!TryLoadCsv(request.Path, out TextAsset csv))
            {
                Debug.LogError($"指定された CSV が存在していません：{request.Path}");
                return null;
            }

            var result = new Dictionary<string, string>();

            Extract(csv.text, request.HasHeader, request.GetMaxTargetColumnIndex(), splitLine =>
            {
                result.Add(
                    string.Join("", request.KeyColumnIndexes.Select(idx => splitLine[idx])),
                    string.Join("", request.ValueColumnIndexes.Select(idx => splitLine[idx]))
                );
            });

            return result;
        }

        private static bool TryLoadCsv(string path, out TextAsset csv)
        {
            csv = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            return csv;
        }

        private static void Extract(string csvText, bool hasHeader, int maxTargetColumnIndex, Action<string[]> action)
        {
            var reader = new StringReader(csvText);

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

                if (maxTargetColumnIndex >= splitLine.Length)
                {
                    continue;
                }

                action(splitLine);
            }
        }
    }
}
