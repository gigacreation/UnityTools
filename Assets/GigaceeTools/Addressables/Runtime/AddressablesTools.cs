using System.IO;
using UnityEngine;

namespace GigaceeTools
{
    public static class AddressablesTools
    {
        public static void ClearAllCachedAssetBundlesAndCatalog()
        {
            bool success = Caching.ClearCache();

            Debug.Log($"アセットバンドルのキャッシュクリアに{(success ? "成功" : "失敗")}しました。");

            var catalogDirPath = $"{Application.persistentDataPath}/com.unity.addressables";

            if (!Directory.Exists(catalogDirPath))
            {
                Debug.Log("コンテンツカタログのキャッシュディレクトリが存在していません。");
                return;
            }

            string[] files = Directory.GetFiles(catalogDirPath, "*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Debug.Log("コンテンツカタログのキャッシュディレクトリ内にファイルが存在していません。");
                return;
            }

            foreach (string file in files)
            {
                File.Delete(file);
            }

            Debug.Log("コンテンツカタログのキャッシュクリアに成功しました。");
        }
    }
}
