using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GcTools
{
    public static class HierarchySorter
    {
        private const int BasePriority = -2099999600;
        private const string Category = "Tools/GC Tools/-------- Sort Hierarchy --------";
        private const string ByName = "Tools/GC Tools/Sort Selected GameObjects By Name";
        private const string ByPositionXYZ = "Tools/GC Tools/Sort Selected GameObjects By Position XYZ";
        private const string ByPositionYXZ = "Tools/GC Tools/Sort Selected GameObjects By Position YXZ";

        [MenuItem(Category, priority = BasePriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ByName, priority = BasePriority + 1)]
        private static void SortByName()
        {
            foreach (IGrouping<Transform, Transform> group in Selection.transforms.GroupBy(s => s.parent))
            {
                ChangeSiblingIndex(group.OrderBy(trans => trans.name).ToArray());
            }
        }

        [MenuItem(ByPositionXYZ, priority = BasePriority + 2)]
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
        [MenuItem(ByPositionYXZ, priority = BasePriority + 3)]
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
        [MenuItem(ByName, true), MenuItem(ByPositionXYZ, true), MenuItem(ByPositionYXZ, true)]
        private static bool ValidateSort()
        {
            return Selection.transforms.Length >= 2;
        }
    }
}
