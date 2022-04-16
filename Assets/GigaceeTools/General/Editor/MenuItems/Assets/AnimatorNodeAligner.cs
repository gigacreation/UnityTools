using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GigaceeTools
{
    public static class AnimatorNodeAligner
    {
        private const int CategoryPriority = 2100000000;
        private const string Category = "Assets/Gigacee Tools/";
        private const string AlignAnimatorNodesName = Category + "Align Animator Nodes";

        [MenuItem(AlignAnimatorNodesName, priority = CategoryPriority)]
        private static void AlignAnimatorNodes()
        {
            Type animatorControllerToolType = Assembly
                .Load("UnityEditor.Graphs")
                .GetModule("UnityEditor.Graphs.dll")
                .GetType("UnityEditor.Graphs.AnimatorControllerTool");

            EditorWindow animatorWindow = EditorWindow.GetWindow(animatorControllerToolType);

            IEnumerable<AnimatorController> selectedAnimatorControllers = Selection
                .objects
                .Select(x => x as AnimatorController)
                .Where(x => x);

            foreach (AnimatorController ac in selectedAnimatorControllers)
            {
                foreach (AnimatorControllerLayer layer in ac.layers.ToList())
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
            return Selection.objects.All(x => x as AnimatorController);
        }
    }
}
