using TVA;

public interface IAreaEntityListener
{
    void OnEnterTCArea(Direct direct, int rate);
    void OnStayInTCArea(float deltaTime);
    void OnExitTCArea(Direct direct);
}