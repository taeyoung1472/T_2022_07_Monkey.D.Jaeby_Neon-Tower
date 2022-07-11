using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IUserInterface
{
    public UnityEvent OnOpenUI { get; set; }
    public UnityEvent OnCloseUI { get; set; }

    public void OpenUI();
    public void CloseUI();
}
