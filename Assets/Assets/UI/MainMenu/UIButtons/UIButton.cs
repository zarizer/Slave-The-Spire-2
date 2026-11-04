using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float BaseSize = 1;
    [SerializeField] float InflatedSize = 1.1f;
    [SerializeField] float InflateSpeed = 0.2f;
    [SerializeField] Vector3 SelfSize = Vector3.one;
    [SerializeField] float CurrentSize;
    [SerializeField] protected SoundType SoundsType = SoundType.Menu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CurrentSize = InflatedSize;       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentSize = BaseSize;
    }

    void Awake()
    {
        CurrentSize = BaseSize;
        SelfSize = transform.localScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, SelfSize * CurrentSize, InflateSpeed);
    }

    private void OnDisable()
    {
        CurrentSize = BaseSize;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        switch (SoundsType) 
        { 
            case SoundType.Menu:
            {
                SoundMain.instance.Accept();
                return;
            }
            case SoundType.Attack:
            {
                SoundMain.instance.Accept();
                return;
            }
            case SoundType.Move:
            {
                SoundMain.instance.Accept();
                return;
            }
            case SoundType.Pull:
            {
                SoundMain.instance.Accept();
                return;
            }
        }
    }

    protected enum SoundType
    {
        Menu,
        Attack,
        Move,
        Pull,
    }
}
