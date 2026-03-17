using DG.Tweening;
using UnityEngine;

public class StartAuction : MonoBehaviour
{
    public UITransitionManagerValue UITransitionManagerValue;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        
        UITransitionManagerValue.Value.MoveAllOut();
    }
}
