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

        public static void Enable(this CanvasGroup self)
        {
            self.interactable = true;
            self.blocksRaycasts = true;
        }

        public static void Disable(this CanvasGroup self)
        {
            self.interactable = false;
            self.blocksRaycasts = false;
        }
    }
}
