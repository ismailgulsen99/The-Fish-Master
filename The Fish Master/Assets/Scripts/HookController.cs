using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HookController : MonoBehaviour
{
    public Transform hookTF;

    private Camera _mainCamera;
    private Collider2D _coll;

    private int _lenght;
    private int _strength;
    private int _fishCount;

    private bool _canMove = true;

    private Tweener _cameraTween;

    private List<Fish> _hookedFishesList;

    void Awake()
    {
        _mainCamera = Camera.main;
        _coll = GetComponent<Collider2D>();
        _hookedFishesList = new List<Fish>();
    }

    void Update()
    {
        if(_canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }

    public void StartFishing()
    {
        _lenght = IdleManager.instance.length - 20;
        _strength = IdleManager.instance.strength;
        _fishCount = 0;
        float time = (-_lenght) * 0.1f;

        _cameraTween = _mainCamera.transform.DOMoveY(_lenght, 1 + time * 0.25f, false).OnUpdate(delegate    //lenght is the furthest distance that hook will go
        {
            if (_mainCamera.transform.position.y <= -11f)
            {
                transform.SetParent(_mainCamera.transform);
            }
        }).OnComplete(delegate
        {
            _coll.enabled = true;
            _cameraTween = _mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate  //hook will come back to the main place
            {                                                                                   //I wrote as time*5 because it will go up faster
                if (_mainCamera.transform.position.y >= -25f)
                {
                    StopFishing();
                }
            });
        });

        //Screen(GAME)
        ScreenManager.instance.ChangeScreen(Screens.GAME);

        _coll.enabled = false;
        _canMove = true;
        _hookedFishesList.Clear(); 
    }

    public void StopFishing()
    {
        _canMove = false;
        _cameraTween.Kill(false);
        _cameraTween = _mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y >= -11f)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            _coll.enabled = true;
            int num = 0;

            for (int i = 0; i < _hookedFishesList.Count; i++)
            {
                _hookedFishesList[i].transform.SetParent(null);
                _hookedFishesList[i].ResetFish();
                num += _hookedFishesList[i].Type.price;
            }

            IdleManager.instance.totalGain = num;
            ScreenManager.instance.ChangeScreen(Screens.END);
        });

    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Fish") && _fishCount != _strength)
        {
            _fishCount++;
            Fish component = target.GetComponent<Fish>();
            component.Hooked();
            _hookedFishesList.Add(component);
            target.transform.SetParent(transform);          //hook is parent of fishes
            target.transform.position = hookTF.position;
            target.transform.rotation = hookTF.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;
            });

            if (_fishCount == _strength)
                StopFishing();
        }
    }
}
