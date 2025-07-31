using UnityEngine;

public class autoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Animation>().Play("Armature_IdleGround");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}