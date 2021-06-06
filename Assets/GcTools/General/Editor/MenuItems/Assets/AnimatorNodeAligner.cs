using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GcTools
{
    public static class AnimatorNodeAligner
    {
        [MenuItem("Assets/GC Tools/Align Animator Nodes")]
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
                    ?.Invoke(animatorWindow, new object[] {false});
            }
        }
    }
}
