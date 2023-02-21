// Original code from WarpTextExample.cs in com.unity.textmeshpro
// Licensed under https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.1/license/LICENSE.html

using System.Collections;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools
{
    [RequireComponent(typeof(TMP_Text))]
    public class WarpText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textComponent;

        [SerializeField] private AnimationCurve _vertexCurve = new(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f)
        );

        [SerializeField] private float _curveScale = 1f;

        private void Reset()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            StartCoroutine(WarpTextCo());
        }

        private static AnimationCurve CopyAnimationCurve(AnimationCurve curve)
        {
            return new AnimationCurve { keys = curve.keys };
        }

        private IEnumerator WarpTextCo()
        {
            _vertexCurve.preWrapMode = WrapMode.Clamp;
            _vertexCurve.postWrapMode = WrapMode.Clamp;

            // Mesh mesh = m_TextComponent.textInfo.meshInfo[0].mesh;

            // Need to force the TextMeshPro Object to be updated.
            _textComponent.havePropertiesChanged = true;
            _curveScale *= 10f;
            float oldCurveScale = _curveScale;
            AnimationCurve oldCurve = CopyAnimationCurve(_vertexCurve);

            while (true)
            {
                if (!_textComponent.havePropertiesChanged &&
                    Mathf.Approximately(oldCurveScale, _curveScale) &&
                    Mathf.Approximately(oldCurve.keys[1].value, _vertexCurve.keys[1].value))
                {
                    yield return null;
                    continue;
                }

                oldCurveScale = _curveScale;
                oldCurve = CopyAnimationCurve(_vertexCurve);

                // Generate the mesh and populate the textInfo with data we can use and manipulate.
                _textComponent.ForceMeshUpdate();

                TMP_TextInfo textInfo = _textComponent.textInfo;
                int characterCount = textInfo.characterCount;

                if (characterCount == 0)
                {
                    continue;
                }

                // vertices = textInfo.meshInfo[0].vertices;
                // int lastVertexIndex = textInfo.characterInfo[characterCount - 1].vertexIndex;

                float boundsMinX = _textComponent.bounds.min.x; // textInfo.meshInfo[0].mesh.bounds.min.x;
                float boundsMaxX = _textComponent.bounds.max.x; // textInfo.meshInfo[0].mesh.bounds.max.x;

                for (var i = 0; i < characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible)
                    {
                        continue;
                    }

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the index of the mesh used by this character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    // Compute the baseline mid point for each character
                    Vector3 offsetToMidBaseline = new Vector2(
                        (vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2,
                        textInfo.characterInfo[i].baseLine
                    );

                    // float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f); // Random.Range(-0.25f, 0.25f);

                    // Apply offset to adjust our pivot point.
                    vertices[vertexIndex + 0] += -offsetToMidBaseline;
                    vertices[vertexIndex + 1] += -offsetToMidBaseline;
                    vertices[vertexIndex + 2] += -offsetToMidBaseline;
                    vertices[vertexIndex + 3] += -offsetToMidBaseline;

                    // Compute the angle of rotation for each character based on the animation curve

                    // Character's position relative to the bounds of the mesh.
                    float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX);
                    float x1 = x0 + 0.0001f;
                    float y0 = _vertexCurve.Evaluate(x0) * _curveScale;
                    float y1 = _vertexCurve.Evaluate(x1) * _curveScale;

                    var horizontal = new Vector3(1, 0, 0);

                    // Vector3 normal = new Vector3(-(y1 - y0), (x1 * (boundsMaxX - boundsMinX) + boundsMinX) - offsetToMidBaseline.x, 0);
                    Vector3 tangent
                        = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1)
                        - new Vector3(offsetToMidBaseline.x, y0);

                    float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                    Vector3 cross = Vector3.Cross(horizontal, tangent);
                    float angle = cross.z > 0 ? dot : 360 - dot;

                    Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                    vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                    vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                    vertices[vertexIndex + 0] += offsetToMidBaseline;
                    vertices[vertexIndex + 1] += offsetToMidBaseline;
                    vertices[vertexIndex + 2] += offsetToMidBaseline;
                    vertices[vertexIndex + 3] += offsetToMidBaseline;
                }

                // Upload the mesh with the revised information
                _textComponent.UpdateVertexData();

                yield return new WaitForSeconds(0.025f);
            }

            // ReSharper disable once IteratorNeverReturns
        }
    }
}
