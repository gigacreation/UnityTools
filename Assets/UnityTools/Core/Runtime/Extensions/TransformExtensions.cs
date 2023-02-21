using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class TransformExtensions
    {
        // =============================================================================================================
        //  Reset
        // =============================================================================================================

        public static void Reset(this Transform self)
        {
            self.position = Vector3.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }

        // =============================================================================================================
        //  SetPosition
        // =============================================================================================================

        public static void SetPosition(this Transform self, float x, float y, float z)
        {
            self.position = new Vector3(x, y, z);
        }

        public static void SetPosition(this Transform self, float x, float y)
        {
            self.position = new Vector3(x, y, self.position.z);
        }

        public static void SetPositionX(this Transform self, float x)
        {
            Vector3 position = self.position;
            self.position = new Vector3(x, position.y, position.z);
        }

        public static void SetPositionY(this Transform self, float y)
        {
            Vector3 position = self.position;
            self.position = new Vector3(position.x, y, position.z);
        }

        public static void SetPositionZ(this Transform self, float z)
        {
            Vector3 position = self.position;
            self.position = new Vector3(position.x, position.y, z);
        }

        // =============================================================================================================
        //  AddPosition
        // =============================================================================================================

        public static void AddPosition(this Transform self, float x, float y, float z)
        {
            self.Translate(x, y, z);
        }

        public static void AddPositionX(this Transform self, float x)
        {
            self.Translate(x, 0f, 0f);
        }

        public static void AddPositionY(this Transform self, float y)
        {
            self.Translate(0f, y, 0f);
        }

        public static void AddPositionZ(this Transform self, float z)
        {
            self.Translate(0f, 0f, z);
        }

        // =============================================================================================================
        //  SetLocalPosition
        // =============================================================================================================

        public static void SetLocalPosition(this Transform self, float x, float y, float z)
        {
            self.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalPosition(this Transform self, float x, float y)
        {
            self.localPosition = new Vector3(x, y, self.localPosition.z);
        }

        public static void SetLocalPositionX(this Transform self, float x)
        {
            Vector3 localPosition = self.localPosition;
            self.localPosition = new Vector3(x, localPosition.y, localPosition.z);
        }

        public static void SetLocalPositionY(this Transform self, float y)
        {
            Vector3 localPosition = self.localPosition;
            self.localPosition = new Vector3(localPosition.x, y, localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform self, float z)
        {
            Vector3 localPosition = self.localPosition;
            self.localPosition = new Vector3(localPosition.x, localPosition.y, z);
        }

        // =============================================================================================================
        //  AddLocalPosition
        // =============================================================================================================

        public static void AddLocalPosition(this Transform self, float x, float y, float z)
        {
            self.Translate(x, y, z, Space.Self);
        }

        public static void AddLocalPositionX(this Transform self, float x)
        {
            self.Translate(x, 0f, 0f, Space.Self);
        }

        public static void AddLocalPositionY(this Transform self, float y)
        {
            self.Translate(0f, y, 0f, Space.Self);
        }

        public static void AddLocalPositionZ(this Transform self, float z)
        {
            self.Translate(0f, 0f, z, Space.Self);
        }

        public static void AddLocalPosition(this Transform self, float x, float y, float z, Transform relativeTo)
        {
            self.Translate(x, y, z, relativeTo);
        }

        public static void AddLocalPositionX(this Transform self, float x, Transform relativeTo)
        {
            self.Translate(x, 0f, 0f, relativeTo);
        }

        public static void AddLocalPositionY(this Transform self, float y, Transform relativeTo)
        {
            self.Translate(0f, y, 0f, relativeTo);
        }

        public static void AddLocalPositionZ(this Transform self, float z, Transform relativeTo)
        {
            self.Translate(0f, 0f, z, relativeTo);
        }

        // =============================================================================================================
        //  SetLocalScale
        // =============================================================================================================

        public static void SetLocalScale(this Transform self, float x, float y, float z)
        {
            self.localScale = new Vector3(x, y, z);
        }

        public static void SetLocalScaleX(this Transform self, float x)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(x, localScale.y, localScale.z);
        }

        public static void SetLocalScaleY(this Transform self, float y)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x, y, localScale.z);
        }

        public static void SetLocalScaleZ(this Transform self, float z)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x, localScale.y, z);
        }

        // =============================================================================================================
        //  AddLocalScale
        // =============================================================================================================

        public static void AddLocalScale(this Transform self, float x, float y, float z)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x + x, localScale.y + y, localScale.z + z);
        }

        public static void AddLocalScaleX(this Transform self, float x)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x + x, localScale.y, localScale.z);
        }

        public static void AddLocalScaleY(this Transform self, float y)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x, localScale.y + y, localScale.z);
        }

        public static void AddLocalScaleZ(this Transform self, float z)
        {
            Vector3 localScale = self.localScale;
            self.localScale = new Vector3(localScale.x, localScale.y, localScale.z + z);
        }

        // =============================================================================================================
        //  SetEulerAngles
        // =============================================================================================================

        public static void SetEulerAngles(this Transform self, float x, float y, float z)
        {
            self.eulerAngles = new Vector3(x, y, z);
        }

        public static void SetEulerAnglesX(this Transform self, float x)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.eulerAngles = new Vector3(x, localEulerAngles.y, localEulerAngles.z);
        }

        public static void SetEulerAnglesY(this Transform self, float y)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.eulerAngles = new Vector3(localEulerAngles.x, y, localEulerAngles.z);
        }

        public static void SetEulerAnglesZ(this Transform self, float z)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.eulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, z);
        }

        // =============================================================================================================
        //  AddEulerAngles
        // =============================================================================================================

        public static void AddEulerAngles(this Transform self, float x, float y, float z)
        {
            Vector3 eulerAngles = self.eulerAngles;
            self.eulerAngles = new Vector3(eulerAngles.x + x, eulerAngles.y + y, eulerAngles.z + z);
        }

        public static void AddEulerAnglesX(this Transform self, float x)
        {
            Vector3 eulerAngles = self.eulerAngles;
            self.eulerAngles = new Vector3(eulerAngles.x + x, eulerAngles.y, eulerAngles.z);
        }

        public static void AddEulerAnglesY(this Transform self, float y)
        {
            Vector3 eulerAngles = self.eulerAngles;
            self.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y + y, eulerAngles.z);
        }

        public static void AddEulerAnglesZ(this Transform self, float z)
        {
            Vector3 eulerAngles = self.eulerAngles;
            self.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + z);
        }

        // =============================================================================================================
        //  SetLocalEulerAngles
        // =============================================================================================================

        public static void SetLocalEulerAngles(this Transform self, float x, float y, float z)
        {
            self.localEulerAngles = new Vector3(x, y, z);
        }

        public static void SetLocalEulerAnglesX(this Transform self, float x)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(x, localEulerAngles.y, localEulerAngles.z);
        }

        public static void SetLocalEulerAnglesY(this Transform self, float y)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x, y, localEulerAngles.z);
        }

        public static void SetLocalEulerAnglesZ(this Transform self, float z)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, z);
        }

        // =============================================================================================================
        //  AddLocalEulerAngles
        // =============================================================================================================

        public static void AddLocalEulerAngles(this Transform self, float x, float y, float z)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x + x, localEulerAngles.y + y, localEulerAngles.z + z);
        }

        public static void AddLocalEulerAnglesX(this Transform self, float x)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x + x, localEulerAngles.y, localEulerAngles.z);
        }

        public static void AddLocalEulerAnglesY(this Transform self, float y)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y + y, localEulerAngles.z);
        }

        public static void AddLocalEulerAnglesZ(this Transform self, float z)
        {
            Vector3 localEulerAngles = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, localEulerAngles.z + z);
        }

        // =============================================================================================================
        //  Lerp
        // =============================================================================================================

        public static void Lerp(this Transform self, Transform to, float t)
        {
            self.LerpPosition(to.position, t);
            self.LerpRotate(to.rotation, t);
            self.LerpScale(to.localScale, t);
        }

        public static void LerpPosition(this Transform self, Vector3 to, float t)
        {
            self.position = Vector3.Lerp(self.position, to, t);
        }

        public static void LerpPosition(this Transform self, Vector2 to, float t)
        {
            Vector3 position = self.position;
            self.position = Vector3.Lerp(position, new Vector3(to.x, to.y, position.z), t);
        }

        public static void LerpRotate(this Transform self, Quaternion to, float t)
        {
            self.rotation = Quaternion.Lerp(self.rotation, to, t);
        }

        public static void LerpRotate(this Transform self, Vector3 to, float t)
        {
            self.LerpRotate(Quaternion.Euler(to), t);
        }

        public static void LerpScale(this Transform self, Vector3 to, float t)
        {
            self.localScale = Vector3.Lerp(self.localScale, to, t);
        }

        public static void LerpPositionX(this Transform self, float to, float t)
        {
            self.SetPositionX(Mathf.Lerp(self.position.x, to, t));
        }

        public static void LerpPositionY(this Transform self, float to, float t)
        {
            self.SetPositionY(Mathf.Lerp(self.position.y, to, t));
        }

        public static void LerpPositionZ(this Transform self, float to, float t)
        {
            self.SetPositionZ(Mathf.Lerp(self.position.z, to, t));
        }

        public static void LerpEulerAnglesX(this Transform self, float to, float t)
        {
            self.SetEulerAnglesX(Mathf.LerpAngle(self.eulerAngles.x, to, t));
        }

        public static void LerpEulerAnglesY(this Transform self, float to, float t)
        {
            self.SetEulerAnglesY(Mathf.LerpAngle(self.eulerAngles.y, to, t));
        }

        public static void LerpEulerAnglesZ(this Transform self, float to, float t)
        {
            self.SetEulerAnglesZ(Mathf.LerpAngle(self.eulerAngles.z, to, t));
        }

        public static void LerpLocalScaleX(this Transform self, float to, float t)
        {
            self.SetLocalScaleX(Mathf.Lerp(self.localScale.x, to, t));
        }

        public static void LerpScaleY(this Transform self, float to, float t)
        {
            self.SetLocalScaleY(Mathf.Lerp(self.localScale.y, to, t));
        }

        public static void LerpScaleZ(this Transform self, float to, float t)
        {
            self.SetLocalScaleZ(Mathf.Lerp(self.localScale.z, to, t));
        }

        // =============================================================================================================
        //  SmoothStep
        // =============================================================================================================

        public static void SmoothStep(this Transform self, Transform to, float t)
        {
            self.SmoothStepPosition(to.position, t);
            self.SmoothStepEulerAngles(to.eulerAngles, t);
            self.SmoothStepLocalScale(to.localScale, t);
        }

        public static void SmoothStepPosition(this Transform self, Vector3 to, float t)
        {
            Vector3 position = self.position;
            float newPositionX = Mathf.SmoothStep(position.x, to.x, t);
            float newPositionY = Mathf.SmoothStep(position.y, to.y, t);
            float newPositionZ = Mathf.SmoothStep(position.z, to.z, t);
            self.position = new Vector3(newPositionX, newPositionY, newPositionZ);
        }

        public static void SmoothStepPosition(this Transform self, Vector2 to, float t)
        {
            self.SmoothStepPosition(new Vector3(to.x, to.y, self.position.z), t);
        }

        public static void SmoothStepEulerAngles(this Transform self, Vector3 to, float t)
        {
            Vector3 eulerAngles = self.eulerAngles;
            float eulerAnglesX = Mathf.SmoothStep(eulerAngles.x, to.x, t);
            float eulerAnglesY = Mathf.SmoothStep(eulerAngles.y, to.y, t);
            float eulerAnglesZ = Mathf.SmoothStep(eulerAngles.z, to.z, t);
            self.SetEulerAngles(eulerAnglesX, eulerAnglesY, eulerAnglesZ);
        }

        public static void SmoothStepLocalScale(this Transform self, Vector3 to, float t)
        {
            Vector3 localScale = self.localScale;
            float localScaleX = Mathf.SmoothStep(localScale.x, to.x, t);
            float localScaleY = Mathf.SmoothStep(localScale.y, to.y, t);
            float localScaleZ = Mathf.SmoothStep(localScale.z, to.z, t);
            self.SetLocalScale(localScaleX, localScaleY, localScaleZ);
        }

        public static void SmoothStepLocalScale(this Transform self, Vector2 to, float t)
        {
            Vector3 localScale = self.localScale;
            float localScaleX = Mathf.SmoothStep(localScale.x, to.x, t);
            float localScaleY = Mathf.SmoothStep(localScale.y, to.y, t);
            self.SetLocalScale(localScaleX, localScaleY, localScale.z);
        }

        public static void SmoothStepPositionX(this Transform self, float to, float t)
        {
            self.SetPositionX(Mathf.SmoothStep(self.position.x, to, t));
        }

        public static void SmoothStepPositionY(this Transform self, float to, float t)
        {
            self.SetPositionY(Mathf.SmoothStep(self.position.y, to, t));
        }

        public static void SmoothStepPositionZ(this Transform self, float to, float t)
        {
            self.SetPositionZ(Mathf.SmoothStep(self.position.z, to, t));
        }

        public static void SmoothStepEulerAnglesX(this Transform self, float to, float t)
        {
            self.SetEulerAnglesX(Mathf.SmoothStep(self.eulerAngles.x, to, t));
        }

        public static void SmoothStepEulerAnglesY(this Transform self, float to, float t)
        {
            self.SetEulerAnglesY(Mathf.SmoothStep(self.eulerAngles.y, to, t));
        }

        public static void SmoothStepEulerAnglesZ(this Transform self, float to, float t)
        {
            self.SetEulerAnglesZ(Mathf.SmoothStep(self.eulerAngles.z, to, t));
        }

        public static void SmoothStepLocalScaleX(this Transform self, float to, float t)
        {
            self.SetLocalScaleX(Mathf.SmoothStep(self.localScale.x, to, t));
        }

        public static void SmoothStepScaleY(this Transform self, float to, float t)
        {
            self.SetLocalScaleY(Mathf.SmoothStep(self.localScale.y, to, t));
        }

        public static void SmoothStepScaleZ(this Transform self, float to, float t)
        {
            self.SetLocalScaleZ(Mathf.SmoothStep(self.localScale.z, to, t));
        }

        // =============================================================================================================
        //  Clamp
        // =============================================================================================================

        public static void Clamp(this Transform self, Transform min, Transform max)
        {
            self.ClampPosition(min.position, max.position);
            self.ClampEulerAngles(min.eulerAngles, max.eulerAngles);
            self.ClampLocalScale(min.localScale, max.localScale);
        }

        public static void ClampPosition(this Transform self, Vector3 min, Vector3 max)
        {
            Vector3 position = self.position;
            float x = Mathf.Clamp(position.x, min.x, max.x);
            float y = Mathf.Clamp(position.y, min.y, max.y);
            float z = Mathf.Clamp(position.z, min.z, max.z);
            self.SetPosition(x, y, z);
        }

        public static void ClampPosition(this Transform self, Vector2 min, Vector2 max)
        {
            Vector3 position = self.position;
            float x = Mathf.Clamp(position.x, min.x, max.x);
            float y = Mathf.Clamp(position.y, min.y, max.y);
            self.SetPosition(x, y);
        }

        public static void ClampEulerAngles(this Transform self, Vector3 min, Vector3 max)
        {
            Vector3 eulerAngles = self.eulerAngles;
            float x = Mathf.Clamp(eulerAngles.x, min.x, max.x);
            float y = Mathf.Clamp(eulerAngles.y, min.y, max.y);
            float z = Mathf.Clamp(eulerAngles.z, min.z, max.z);
            self.SetEulerAngles(x, y, z);
        }

        public static void ClampLocalScale(this Transform self, Vector3 min, Vector3 max)
        {
            Vector3 localScale = self.localScale;
            float x = Mathf.Clamp(localScale.x, min.x, max.x);
            float y = Mathf.Clamp(localScale.y, min.y, max.y);
            float z = Mathf.Clamp(localScale.z, min.z, max.z);
            self.SetLocalScale(x, y, z);
        }

        public static void ClampPositionX(this Transform self, float min, float max)
        {
            self.SetPositionX(Mathf.Clamp(self.position.x, min, max));
        }

        public static void ClampPositionY(this Transform self, float min, float max)
        {
            self.SetPositionY(Mathf.Clamp(self.position.y, min, max));
        }

        public static void ClampPositionZ(this Transform self, float min, float max)
        {
            self.SetPositionZ(Mathf.Clamp(self.position.z, min, max));
        }

        public static void ClampEulerAnglesX(this Transform self, float min, float max)
        {
            self.SetEulerAnglesX(Mathf.Clamp(self.eulerAngles.x, min, max));
        }

        public static void ClampEulerAnglesY(this Transform self, float min, float max)
        {
            self.SetEulerAnglesY(Mathf.Clamp(self.eulerAngles.y, min, max));
        }

        public static void ClampEulerAnglesZ(this Transform self, float min, float max)
        {
            self.SetEulerAnglesZ(Mathf.Clamp(self.eulerAngles.z, min, max));
        }

        public static void ClampLocalScaleX(this Transform self, float min, float max)
        {
            self.SetLocalScaleX(Mathf.Clamp(self.localScale.x, min, max));
        }

        public static void ClampLocalScaleY(this Transform self, float min, float max)
        {
            self.SetLocalScaleY(Mathf.Clamp(self.localScale.y, min, max));
        }

        public static void ClampLocalScaleZ(this Transform self, float min, float max)
        {
            self.SetLocalScaleZ(Mathf.Clamp(self.localScale.z, min, max));
        }

        // =============================================================================================================
        //  LookAt2D
        // =============================================================================================================

        public static void LookAt2D(this Transform self, Transform target)
        {
            LookAt2D(self, target.position, Vector3.forward);
        }

        public static void LookAt2D(this Transform self, Vector2 target)
        {
            LookAt2D(self, target, Vector3.forward);
        }

        public static void LookAt2D(this Transform self, Transform target, float angle)
        {
            LookAt2D(self, target.position, Vector3.forward, angle);
        }

        public static void LookAt2D(this Transform self, Vector2 target, float angle)
        {
            LookAt2D(self, target, Vector3.forward, angle);
        }

        public static void LookAt2D(this Transform self, Transform target, Vector3 axis)
        {
            LookAt2D(self, target.position, axis);
        }

        public static void LookAt2D(this Transform self, Transform target, Vector3 axis, float angle)
        {
            LookAt2D(self, target.position, axis, angle);
        }

        public static void LookAt2D(this Transform self, Vector2 target, Vector3 axis, float angle = 0f)
        {
            Vector3 position = self.position;
            var difference = new Vector3(target.x - position.x, target.y - position.y, 0f);
            self.rotation = Quaternion.AngleAxis(angle + Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg, axis);
        }
    }
}
