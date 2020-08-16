using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenSlider : MonoBehaviour
{
    public ParticleSystem.MinMaxGradient IncreaseGradient;
    public ParticleSystem.MinMaxGradient DecreaseGradient;
    public float TweenTime;

    public Slider TargetSlider { get; private set; }
    public float TargetValue { get; private set; }

    private Tween _tween;
    private Image _fillImg;
    private Color _rawColor;
    private bool _isInit;

    public void Init(float min, float max, float value)
    {
        if (!_isInit)
        {
            TargetSlider = GetComponent<Slider>();
            _fillImg = TargetSlider.fillRect.GetComponentInChildren<Image>();
            _rawColor = _fillImg.color;
            _isInit = true;
        }
        
        TargetSlider.minValue = min;
        TargetSlider.maxValue = max;
        TargetSlider.value = value;
        TargetValue = value;
        _fillImg.color = _rawColor;
    }

    public Tween TweenTo(float endVal)
    {
        _tween?.Kill();

        var gradient = TargetValue > endVal ? DecreaseGradient : IncreaseGradient;
        TargetValue = Mathf.Clamp(endVal, TargetSlider.minValue, TargetSlider.maxValue);

        var val = TargetSlider.value;
        _tween = DOTween.To(() => val, v => val = v, TargetValue, TweenTime)
            .OnUpdate(() =>
            {
                TargetSlider.value = val;
                _fillImg.color = gradient.Evaluate(_tween.Elapsed() / _tween.Duration());
            })
            .OnComplete(() =>
            {
                TargetSlider.value = TargetValue;
                _fillImg.color = _rawColor;
            })
            .SetEase(Ease.OutQuad);
        return _tween;
    }

    public Tween TweenAdd(float add)
    {
        return TweenTo(TargetValue + add);
    }
}
