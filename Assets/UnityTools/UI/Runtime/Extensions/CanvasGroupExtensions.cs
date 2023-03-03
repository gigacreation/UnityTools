using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools.Ui
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class CanvasGroupExtensions
    {
        public static void Activate(this CanvasGroup self)
        {
            self.alpha = 1f;
            self.interactable = true;
            self.blocksRaycasts = true;
        }

        public static void Deactivate(this CanvasGroup self)
        {
            self.alpha = 0f;
            self.interactable = false;
            self.blocksRaycasts = false;
        }
    }
}
