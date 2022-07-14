using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Glitch;
using static DefineCamera;


namespace Samples
{
    sealed class SampleController : MonoBehaviour
    {
        public static SampleController instance;

        [SerializeField] DigitalGlitchFeature _digitalGlitchFeature = default;
        [SerializeField] AnalogGlitchFeature _analogGlitchFeature = default;

        [Header("Digital")]
        [SerializeField, Range(0f, 1f)] float _intensity = default;

        [Header("Analog")]
        [SerializeField, Range(0f, 1f)] float _scanLineJitter = default;
        [SerializeField, Range(0f, 1f)] float _verticalJump = default;
        [SerializeField, Range(0f, 1f)] float _horizontalShake = default;
        [SerializeField, Range(0f, 1f)] float _colorDrift = default;



        public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData additionalCameraData;
        private void Awake()
        {
            instance = this;
            additionalCameraData =
                MainCam.transform.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                StartCoroutine(StartCutScene());
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ZeroValue();
            }


            _digitalGlitchFeature.Intensity = _intensity;

            _analogGlitchFeature.ScanLineJitter = _scanLineJitter;
            _analogGlitchFeature.VerticalJump = _verticalJump;
            _analogGlitchFeature.HorizontalShake = _horizontalShake;
            _analogGlitchFeature.ColorDrift = _colorDrift;
        }
        public void ZeroValue()
        {
            _digitalGlitchFeature.Intensity = 0;

            _analogGlitchFeature.ScanLineJitter = 0;
            _analogGlitchFeature.VerticalJump = 0;
            _analogGlitchFeature.HorizontalShake = 0;
            _analogGlitchFeature.ColorDrift = 0;
        }
        public void StartSceneValue()
        {
            _digitalGlitchFeature.Intensity = _intensity;

            _analogGlitchFeature.ScanLineJitter = _scanLineJitter;
            _analogGlitchFeature.VerticalJump = _verticalJump;
            _analogGlitchFeature.HorizontalShake = _horizontalShake;
            _analogGlitchFeature.ColorDrift = _colorDrift;
        }
        public void GraySceneValue()
        {
            _digitalGlitchFeature.Intensity = _intensity;

            _analogGlitchFeature.ScanLineJitter = _scanLineJitter;
            _analogGlitchFeature.VerticalJump = _verticalJump;
            _analogGlitchFeature.HorizontalShake = _horizontalShake;
            _analogGlitchFeature.ColorDrift = _colorDrift;
        }
        public void LoadGameCutScene()
        {
            StartCoroutine(StartCutScene());
        }
        public void StartGameCutScene()
        {
            _intensity = 0.8f;
            StartCoroutine(GameStartCutScene());
        }

        IEnumerator GameStartCutScene()
        {
            while (_intensity > 0.005f)
            {
                _intensity -= 0.05f;

                yield return new WaitForSeconds(0.05f);
            }
            _intensity = 0.001f;
        }
        IEnumerator StartCutScene()
        {
            while (_intensity < 1f)
            {
                _intensity += 0.05f;

                yield return new WaitForSeconds(0.05f);
            }
            _intensity = 0.001f;
        }
        public void ChangeRenderModeOne()
        {
            additionalCameraData.SetRenderer(1);
        }
        public void ChangeRenderModeZero()
        {
            additionalCameraData.SetRenderer(0);
        }
    }
}
