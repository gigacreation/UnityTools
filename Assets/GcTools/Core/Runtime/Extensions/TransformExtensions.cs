using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GcTools
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class TransformExtensions
    {
        private static Vector3 s_vector3;

        // =================================================================================================================
        //  Reset
        // =================================================================================================================

        public static void Reset(this Transform self)
        {
            self.position = Vector3.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }

        // =================================================================================================================
        //  SetPosition
        // =================================================================================================================

        public static void SetPosition(this Transform self, float x, float y, float z)
        {
            s_vector3.Set(x, y, z);
            self.position = s_vector3;
        }

        public static void SetPosition(this Transform self, float x, float y)
        {
            s_vector3.Set(x, y, self.position.z);
            self.position = s_vector3;
        }

        public static void SetPositionX(this Transform self, float x)
        {
            s_vector3 = self.position;
            s_vector3.Set(x, s_vector3.y, s_vector3.z);
            self.position = s_vector3;
        }

        public static void SetPositionY(this Transform self, float y)
        {
            s_vector3 = self.position;
            s_vector3.Set(s_vector3.x, y, s_vector3.z);
            self.position = s_vector3;
        }

        public static void SetPositionZ(this Transform self, float z)
        {
            s_vector3 = self.position;
            s_vector3.Set(s_vector3.x, s_vector3.y, z);
            self.position = s_vector3;
        }

        // =================================================================================================================
        //  AddPosition
        // =================================================================================================================

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

        // =================================================================================================================
        //  SetLocalPosition
        // =================================================================================================================

        public static void SetLocalPosition(this Transform self, float x, float y, float z)
        {
            s_vector3.Set(x, y, z);
            self.localPosition = s_vector3;
        }

        public static void SetLocalPosition(this Transform self, float x, float y)
        {
            s_vector3.Set(x, y, self.localPosition.z);
            self.localPosition = s_vector3;
        }

        public static void SetLocalPositionX(this Transform self, float x)
        {
            s_vector3 = self.localPosition;
            s_vector3.Set(x, s_vector3.y, s_vector3.z);
            self.localPosition = s_vector3;
        }

        public static void SetLocalPositionY(this Transform self, float y)
        {
            s_vector3 = self.localPosition;
            s_vector3.Set(s_vector3.x, y, s_vector3.z);
            self.localPosition = s_vector3;
        }

        public static void SetLocalPositionZ(this Transform self, float z)
        {
            s_vector3 = self.localPosition;
            s_vector3.Set(s_vector3.x, s_vector3.y, z);
            self.localPosition = s_vector3;
        }

        // =================================================================================================================
        //  AddLocalPosition
        // =================================================================================================================

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

        // =================================================================================================================
        //  SetLocalScale
        // =================================================================================================================

        public static void SetLocalScale(this Transform self, float x, float y, float z)
        {
            s_vector3.Set(x, y, z);
            self.localScale = s_vector3;
        }

        public static void SetLocalScaleX(this Transform self, float x)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(x, s_vector3.y, s_vector3.z);
            self.localScale = s_vector3;
        }

        public static void SetLocalScaleY(this Transform self, float y)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x, y, s_vector3.z);
            self.localScale = s_vector3;
        }

        public static void SetLocalScaleZ(this Transform self, float z)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x, s_vector3.y, z);
            self.localScale = s_vector3;
        }

        // =================================================================================================================
        //  AddLocalScale
        // =================================================================================================================

        public static void AddLocalScale(this Transform self, float x, float y, float z)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x + x, s_vector3.y + y, s_vector3.z + z);
            self.localScale = s_vector3;
        }

        public static void AddLocalScaleX(this Transform self, float x)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x + x, s_vector3.y, s_vector3.z);
            self.localScale = s_vector3;
        }

        public static void AddLocalScaleY(this Transform self, float y)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x, s_vector3.y + y, s_vector3.z);
            self.localScale = s_vector3;
        }

        public static void AddLocalScaleZ(this Transform self, float z)
        {
            s_vector3 = self.localScale;
            s_vector3.Set(s_vector3.x, s_vector3.y, s_vector3.z + z);
            self.localScale = s_vector3;
        }

        // =================================================================================================================
        //  SetEulerAngles
        // =================================================================================================================

        public static void SetEulerAngles(this Transform self, float x, float y, float z)
        {
            s_vector3.Set(x, y, z);
            self.eulerAngles = s_vector3;
        }

        public static void SetEulerAnglesX(this Transform self, float x)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(x, s_vector3.y, s_vector3.z);
            self.eulerAngles = s_vector3;
        }

        public static void SetEulerAnglesY(this Transform self, float y)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, y, s_vector3.z);
            self.eulerAngles = s_vector3;
        }

        public static void SetEulerAnglesZ(this Transform self, float z)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y, z);
            self.eulerAngles = s_vector3;
        }

        // =================================================================================================================
        //  AddEulerAngles
        // =================================================================================================================

        public static void AddEulerAngles(this Transform self, float x, float y, float z)
        {
            s_vector3 = self.eulerAngles;
            s_vector3.Set(s_vector3.x + x, s_vector3.y + y, s_vector3.z + z);
            self.eulerAngles = s_vector3;
        }

        public static void AddEulerAnglesX(this Transform self, float x)
        {
            s_vector3 = self.eulerAngles;
            s_vector3.Set(s_vector3.x + x, s_vector3.y, s_vector3.z);
            self.eulerAngles = s_vector3;
        }

        public static void AddEulerAnglesY(this Transform self, float y)
        {
            s_vector3 = self.eulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y + y, s_vector3.z);
            self.eulerAngles = s_vector3;
        }

        public static void AddEulerAnglesZ(this Transform self, float z)
        {
            s_vector3 = self.eulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y, s_vector3.z + z);
            self.eulerAngles = s_vector3;
        }

        // =================================================================================================================
        //  SetLocalEulerAngles
        // =================================================================================================================

        public static void SetLocalEulerAngles(this Transform self, float x, float y, float z)
        {
            s_vector3.Set(x, y, z);
            self.localEulerAngles = s_vector3;
        }

        public static void SetLocalEulerAnglesX(this Transform self, float x)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(x, s_vector3.y, s_vector3.z);
            self.localEulerAngles = s_vector3;
        }

        public static void SetLocalEulerAnglesY(this Transform self, float y)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, y, s_vector3.z);
            self.localEulerAngles = s_vector3;
        }

        public static void SetLocalEulerAnglesZ(this Transform self, float z)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y, z);
            self.localEulerAngles = s_vector3;
        }

        // =================================================================================================================
        //  AddLocalEulerAngles
        // =================================================================================================================

        public static void AddLocalEulerAngles(this Transform self, float x, float y, float z)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x + x, s_vector3.y + y, s_vector3.z + z);
            self.localEulerAngles = s_vector3;
        }

        public static void AddLocalEulerAnglesX(this Transform self, float x)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x + x, s_vector3.y, s_vector3.z);
            self.localEulerAngles = s_vector3;
        }

        public static void AddLocalEulerAnglesY(this Transform self, float y)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y + y, s_vector3.z);
            self.localEulerAngles = s_vector3;
        }

        public static void AddLocalEulerAnglesZ(this Transform self, float z)
        {
            s_vector3 = self.localEulerAngles;
            s_vector3.Set(s_vector3.x, s_vector3.y, s_vector3.z + z);
            self.localEulerAngles = s_vector3;
        }

        // =================================================================================================================
        //  Lerp
        // =================================================================================================================

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
            s_vector3 = self.position;
            self.position = Vector3.Lerp(s_vector3, new Vector3(to.x, to.y, s_vector3.z), t);
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

        // =================================================================================================================
        //  SmoothStep
        // =================================================================================================================

        public static void SmoothStep(this Transform self, Transform to, float t)
        {
            self.SmoothStepPosition(to.position, t);
            self.SmoothStepEulerAngles(to.eulerAngles, t);
            self.SmoothStepLocalScale(to.localScale, t);
        }

        public static void SmoothStepPosition(this Transform self, Vector3 to, float t)
        {
            s_vector3 = self.position;
            float newPositionX = Mathf.SmoothStep(s_vector3.x, to.x, t);
            float newPositionY = Mathf.SmoothStep(s_vector3.y, to.y, t);
            float newPositionZ = Mathf.SmoothStep(s_vector3.z, to.z, t);
            self.position = new Vector3(newPositionX, newPositionY, newPositionZ);
        }

        public static void SmoothStepPosition(this Transform self, Vector2 to, float t)
        {
            self.SmoothStepPosition(new Vector3(to.x, to.y, self.position.z), t);
        }

        public static void SmoothStepEulerAngles(this Transform self, Vector3 to, float t)
        {
            s_vector3 = self.eulerAngles;
            float eulerAnglesX = Mathf.SmoothStep(s_vector3.x, to.x, t);
            float eulerAnglesY = Mathf.SmoothStep(s_vector3.y, to.y, t);
            float eulerAnglesZ = Mathf.SmoothStep(s_vector3.z, to.z, t);
            self.SetEulerAngles(eulerAnglesX, eulerAnglesY, eulerAnglesZ);
        }

        public static void SmoothStepLocalScale(this Transform self, Vector3 to, float t)
        {
            s_vector3 = self.localScale;
            float localScaleX = Mathf.SmoothStep(s_vector3.x, to.x, t);
            float localScaleY = Mathf.SmoothStep(s_vector3.y, to.y, t);
            float localScaleZ = Mathf.SmoothStep(s_vector3.z, to.z, t);
            self.SetLocalScale(localScaleX, localScaleY, localScaleZ);
        }

        public static void SmoothStepLocalScale(this Transform self, Vector2 to, float t)
        {
            s_vector3 = self.localScale;
            float localScaleX = Mathf.SmoothStep(s_vector3.x, to.x, t);
            float localScaleY = Mathf.SmoothStep(s_vector3.y, to.y, t);
            self.SetLocalScale(localScaleX, localScaleY, s_vector3.z);
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

        // =================================================================================================================
        //  Clamp
        // =================================================================================================================

        public static void Clamp(this Transform self, Transform min, Transform max)
        {
            self.ClampPosition(min.position, max.position);
            self.ClampEulerAngles(min.eulerAngles, max.eulerAngles);
            self.ClampLocalScale(min.localScale, max.localScale);
        }

        public static void ClampPosition(this Transform self, Vector3 min, Vector3 max)
        {
            s_vector3 = self.position;
            float x = Mathf.Clamp(s_vector3.x, min.x, max.x);
            float y = Mathf.Clamp(s_vector3.y, min.y, max.y);
            float z = Mathf.Clamp(s_vector3.z, min.z, max.z);
            self.SetPosition(x, y, z);
        }

        public static void ClampPosition(this Transform self, Vector2 min, Vector2 max)
        {
            s_vector3 = self.position;
            float x = Mathf.Clamp(s_vector3.x, min.x, max.x);
            float y = Mathf.Clamp(s_vector3.y, min.y, max.y);
            self.SetPosition(x, y);
        }

        public static void ClampEulerAngles(this Transform self, Vector3 min, Vector3 max)
        {
            s_vector3 = self.eulerAngles;
            float x = Mathf.Clamp(s_vector3.x, min.x, max.x);
            float y = Mathf.Clamp(s_vector3.y, min.y, max.y);
            float z = Mathf.Clamp(s_vector3.z, min.z, max.z);
            self.SetEulerAngles(x, y, z);
        }

        public static void ClampLocalScale(this Transform self, Vector3 min, Vector3 max)
        {
            s_vector3 = self.localScale;
            float x = Mathf.Clamp(s_vector3.x, min.x, max.x);
            float y = Mathf.Clamp(s_vector3.y, min.y, max.y);
            float z = Mathf.Clamp(s_vector3.z, min.z, max.z);
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

        // =================================================================================================================
        //  LookAt2D
        // =================================================================================================================

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
            s_vector3 = self.position;
            s_vector3.Set(target.x - s_vector3.x, target.y - s_vector3.y, 0f);
            self.rotation = Quaternion.AngleAxis(angle + Mathf.Atan2(s_vector3.y, s_vector3.x) * Mathf.Rad2Deg, axis);
        }
    }
}
