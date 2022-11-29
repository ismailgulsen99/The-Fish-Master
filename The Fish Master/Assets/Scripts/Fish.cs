using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Fish : MonoBehaviour
{

    private Fish.FishType _type;

    private CircleCollider2D _coll2D;

    private SpriteRenderer _fishSR;

    private float _screenLeft;  //for fishes stay inside the canvas

    private Tweener _tweener;

    public Fish.FishType Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            _coll2D.radius = _type.colliderRadius;
            _fishSR.sprite = _type.FishSprite;
        }
    }

    void Awake()
    {
        _coll2D = GetComponent<CircleCollider2D>();
        _fishSR = GetComponentInChildren<SpriteRenderer>();
        _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    public void ResetFish()
    {
        if (_tweener != null)
            _tweener.Kill(false);

        float num = UnityEngine.Random.Range(_type.minLenght, _type.maxLenght);
        _coll2D.enabled = true;

        Vector3 position = transform.position;
        position.y = num;
        position.x = _screenLeft;
        transform.position = position;

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 v = new Vector2(-position.x, y);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        _tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate //with SetLoops fish go left and right forever
        {                                                                                                                                    //with SetEase fish will go left and right
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });

    }

    public void Hooked()
    {
        _coll2D.enabled = false;
        _tweener.Kill(false);
    }
    
    [Serializable]
    public class FishType
    {
        public int price;

        public float fishCount;
        public float minLenght;
        public float maxLenght;
        public float colliderRadius;

        public Sprite FishSprite;
    }

}
