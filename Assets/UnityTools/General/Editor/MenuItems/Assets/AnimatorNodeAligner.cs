using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class AnimatorNodeAligner
    {
        private const int CategoryPriority = 1000;
        private const string Category = "Assets/GIGA CREATION/";
        private const string AlignAnimatorNodesName = Category + "Align Animator Nodes";

        [MenuItem(AlignAnimatorNodesName, priority = CategoryPriority)]
        private static void AlignAnimatorNodes()
        {
            var animatorControllerToolType = Assembly
                .Load("UnityEditor.Graphs")
                .GetModule("UnityEditor.Graphs.dll")
                .GetType("UnityEditor.Graphs.AnimatorControllerTool");

            var animatorWindow = EditorWindow.GetWindow(animatorControllerToolType);

            var selectedAnimatorControllers = Selection
                .objects
                .Select(obj => obj as AnimatorController)
                .Where(ac => ac);

            foreach (var ac in selectedAnimatorControllers)
            {
                foreach (var layer in ac.layers.ToList())
                {
                    layer.stateMachine.entryPosition = Vector3.zero;
                    layer.stateMachine.anyStatePosition = Vector3.up * 50f;
                    layer.stateMachine.exitPosition = Vector3.up * 100f;
                }

                animatorControllerToolType
                    .GetMethod("RebuildGraph", BindingFlags.Public | BindingFlags.Instance)
                    ?.Invoke(animatorWindow, new object[] { false });
            }
        }

        [MenuItem(AlignAnimatorNodesName, true)]
        private static bool NoAnimatorControllerSelection()
        {
            return Selection.objects.All(obj => obj as AnimatorController);
        }
    }
}
