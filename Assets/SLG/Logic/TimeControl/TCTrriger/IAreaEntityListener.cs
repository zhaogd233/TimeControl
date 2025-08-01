using System.Collections;
using System.Collections.Generic;
using TVA;
using UnityEngine;

public interface IAreaEntityListener 
{
    void OnEnterTCArea(Direct direct,int rate);
    void OnStayInTCArea(float deltaTime);
    void OnExitTCArea(Direct direct);
}
