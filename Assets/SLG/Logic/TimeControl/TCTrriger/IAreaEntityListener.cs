using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaEntityListener 
{
    void OnEnterTCArea(float rate);
    void OnStayInTCArea(float deltaTime);
    void OnExitTCArea();
}
