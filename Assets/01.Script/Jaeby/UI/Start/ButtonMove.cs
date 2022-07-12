using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonMove : MonoBehaviour
{
    private bool _isAc = false;

    void Update()
    {
        //UI위에 커서가 있을때 = ture/ 따라서 UI위에 커서가 없을때만 ClickAction함수를 실행
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            if(_isAc == false)
            {
                _isAc = true;
                transform.DOLocalMoveX(100f, 0.5f);
            }
        }
        else
        {
            if (_isAc == true)
            {
                _isAc = false;
                transform.DOLocalMoveX(14f, 0.5f);
            }
        }
    }
}
