using TVA;

public interface IAreaEntityListener
{
    void OnEnterTCArea(Direct direct, int rate);
    void OnExitTCArea(Direct direct);
}