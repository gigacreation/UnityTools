using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static GigaceeTools.ToolsMenuItemConstants;

namespace GigaceeTools
{
    public static class HierarchySorter
    {
        private const int CategoryPriority = BasePriority + 400;
        private const string Category = BasePath + CategoryPrefix + "Sort Hierarchy" + CategorySuffix;
        private const string ByName = BasePath + "Sort Selected GameObjects By Name";
        private const string ByPositionXYZ = BasePath + "Sort Selected GameObjects By Position XYZ";
        private const string ByPositionYXZ = BasePath + "Sort Selected GameObjects By Position YXZ";

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ByName, priority = CategoryPriority + 1)]
        private static void SortByName()
        {
            foreach (IGrouping<Transform, Transform> group in Selection.transforms.GroupBy(s => s.parent))
            {
                ChangeSiblingIndex(group.OrderBy(trans => trans.name).ToArray());
            }
        }

        [MenuItem(ByPositionXYZ, priority = CategoryPriority + 2)]
        private static void SortByPosXYZ()
        {
            foreach (IGrouping<Transform, Transform> group in Selection.transforms.GroupBy(s => s.parent))
            {
                ChangeSiblingIndex(
                    group.OrderBy(trans => trans.position.x)
                        .ThenBy(trans => trans.position.y)
                        .ThenBy(trans => trans.position.z)
                        .ToArray()
                );
            }
        }

        // ReSharper disable once InconsistentNaming
        [MenuItem(ByPositionYXZ, priority = CategoryPriority + 3)]
        private static void SortByPosYXZ()
        {
            foreach (IGrouping<Transform, Transform> group in Selection.transforms.GroupBy(s => s.parent))
            {
                ChangeSiblingIndex(
                    group.OrderBy(trans => trans.position.y)
                        .ThenBy(trans => trans.position.x)
                        .ThenBy(trans => trans.position.z)
                        .ToArray()
                );
            }
        }

        private static void ChangeSiblingIndex(IReadOnlyList<Transform> transforms)
        {
            for (var i = 0; i < transforms.Count; i++)
            {
                Undo.RecordObject(transforms[i], "Sort Selected GameObjects");
                transforms[i].SetSiblingIndex(i);
                EditorUtility.SetDirty(transforms[i]);
            }
        }

        // オブジェクトが 2 つ以上選択されていなければメニューを無効化する
        [MenuItem(ByName, true)]
        [MenuItem(ByPositionXYZ, true)]
        [MenuItem(ByPositionYXZ, true)]
        private static bool ValidateSort()
        {
            return Selection.transforms.Length >= 2;
        }
    }
}
