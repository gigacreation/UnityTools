using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static GigaceeTools.AssetsMenuItemConstants;

namespace GigaceeTools
{
    public static class AnimatorNodeAligner
    {
        private const int CategoryPriority = BasePriority;
        private const string AlignAnimatorNodes = BasePath + "Align Animator Nodes";

        [MenuItem(AlignAnimatorNodes, priority = CategoryPriority)]
        private static void AlignAnimator()
        {
            Type animatorControllerToolType = Assembly
                .Load("UnityEditor.Graphs")
                .GetModule("UnityEditor.Graphs.dll")
                .GetType("UnityEditor.Graphs.AnimatorControllerTool");

            EditorWindow animatorWindow = EditorWindow.GetWindow(animatorControllerToolType);

            IEnumerable<AnimatorController> selectedAnimatorControllers = Selection.objects
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

        [MenuItem(AlignAnimatorNodes, true)]
        private static bool NoAnimatorControllerSelection()
        {
            return Selection.objects.All(x => x as AnimatorController);
        }
    }
}
