using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageValue : MonoBehaviour
{
    public TextMeshProUGUI text;

    [Header("Настройки движения")]
    [SerializeField] private float _moveUpDistance = 5f;   
    [SerializeField] private float _moveDuration = 2f;     
    [SerializeField] private float _swingSpeed = 0.5f;    
    [SerializeField] private float _swingAmount = 30f;       
    [SerializeField] private float _fadeDuration = 1.5f;     
    [SerializeField] private float _fadeStart = 0.7f;

    [Header("Рандомизация")]
    [SerializeField] private bool _randomizeOnStart = true;   
    [SerializeField] private float _randomAngleMin = -45f;     
    [SerializeField] private float _randomAngleMax = 45f;      

    private LTDescr _moveTween;
    private LTDescr _swingTween;
    private LTDescr _fadeTween;
    private Vector3 _startPosition;
    private float _swingAngleOffset;
    public void Init(Damage damage, float proportion, bool is_heal = false, bool is_blocked = false)
    {
        _startPosition = transform.position;
        _swingAngleOffset = transform.eulerAngles.z;
        text.text = damage.damage.ToString();
        text.fontSize = 0.2f + proportion * 4;
        



        if (damage.element == Element.None)
        {
            text.color = new Color(60f / 255f, 60f / 255f, 60f / 255f);
        }
        else if (damage.element == Element.fire)
        {
            text.color = new Color(255f / 255f, 130f / 255f, 70f / 255f);
        }
        else if (damage.element == Element.water)
        {
            text.color = new Color(40f / 255f, 40f / 255f, 255f / 255f);
        }
        else if (damage.element == Element.dendro)
        {
            text.color = new Color(200f / 255f, 255f / 255f, 200f / 255f);
        }
        else if (damage.element == Element.light)
        {
            text.color = new Color(255f / 255f, 255f / 255f, 200f / 255f);
        }
        else if (damage.element == Element.darkness)
        {
            text.color = new Color(0f / 255f, 0f / 255f, 40f / 255f);
        }
        else
        {
            text.color = new Color(20f / 255f, 20f / 255f, 20f / 255f);
        }
        if (is_heal) { text.color = Color.green; text.text = "+" + text.text; }
        if (is_blocked) { text.text = "(" + text.text + ")"; }
        StartFloatingAnimation();
    }

    private void StartFloatingAnimation()
    {
        float randomAngle = Random.Range(_randomAngleMin, _randomAngleMax);
        Vector3 randomDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;

        Vector3 targetPosition = _startPosition + randomDirection * _moveUpDistance;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        bool hasSprite = spriteRenderer != null;
        bool hasCanvas = canvasGroup != null;

        float startAlpha = 1f;
        if (hasSprite) startAlpha = spriteRenderer.color.a;
        else if (hasCanvas) startAlpha = canvasGroup.alpha;

        _moveTween = LeanTween.move(gameObject, targetPosition, _moveDuration)
            .setEase(LeanTweenType.easeOutCubic) 
            .setOnUpdate((float progress) =>
            {

                LeanTween.delayedCall(_fadeStart, () => {
                    float currentAlpha  = Mathf.Lerp(startAlpha, 0f, progress);
                    if (hasSprite)
                    {
                        Color color = spriteRenderer.color;
                        color.a = currentAlpha;
                        spriteRenderer.color = color;
                    }
                    else if (hasCanvas)
                    {
                        if (canvasGroup != null)  canvasGroup.alpha = currentAlpha;
                        
                    }
                });
                
                if (_swingTween == null)
                {
                    float swingProgress = Mathf.Sin(progress * Mathf.PI * 2 * _swingSpeed * 2) * _swingAmount;
                    Vector3 rotation = transform.eulerAngles;
                    rotation.z = _swingAngleOffset + swingProgress;
                    transform.eulerAngles = rotation;
                }
            })
            .setOnComplete(() =>
            {
                SetAlpha(0f);
                DestroyObject();
            });

        _swingTween = LeanTween.rotateZ(gameObject, _swingAmount, _swingSpeed)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();

        if (_randomizeOnStart)
        {
            float randomOffset = Random.Range(0f, _swingSpeed);
            _swingTween.setDelay(randomOffset);
        }

        _swingAngleOffset = transform.eulerAngles.z;
    }

    private void SetAlpha(float alpha)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        else if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    private void DestroyObject()
    {
        LeanTween.cancel(gameObject);

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
}
