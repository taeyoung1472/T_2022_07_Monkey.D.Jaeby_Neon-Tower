﻿using UnityEngine;

using UnityEngine.Rendering.Universal.Glitch;
using static DefineCamera;


namespace Samples
{
    sealed class SampleController : MonoBehaviour
    {
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
            additionalCameraData =
                MainCam.transform.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ChangeRenderModeOne();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ChangeRenderModeZero();

            }


            _digitalGlitchFeature.Intensity = _intensity;

            _analogGlitchFeature.ScanLineJitter = _scanLineJitter;
            _analogGlitchFeature.VerticalJump = _verticalJump;
            _analogGlitchFeature.HorizontalShake = _horizontalShake;
            _analogGlitchFeature.ColorDrift = _colorDrift;
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
