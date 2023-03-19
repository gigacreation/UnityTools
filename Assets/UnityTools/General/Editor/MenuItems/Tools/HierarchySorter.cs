using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class HierarchySorter
    {
        private const int CategoryPriority = 29001;
        private const string Category = "Tools/GIGA CREATION/Sort Hierarchy/";
        private const string ByName = Category + "Sort Selected GameObjects by Name";
        private const string ByPositionXYZ = Category + "Sort Selected GameObjects by Position XYZ";
        private const string ByPositionYXZ = Category + "Sort Selected GameObjects by Position YXZ";

        [MenuItem(ByName, priority = CategoryPriority)]
        private static void SortByName()
        {
            foreach (IGrouping<Transform, Transform> group in Selection.transforms.GroupBy(s => s.parent))
            {
                ChangeSiblingIndex(group.OrderBy(trans => trans.name).ToArray());
            }
        }

        [MenuItem(ByPositionXYZ, priority = CategoryPriority + 1)]
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
        [MenuItem(ByPositionYXZ, priority = CategoryPriority + 2)]
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
