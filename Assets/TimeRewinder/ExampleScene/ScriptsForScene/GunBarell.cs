using UnityEngine;

public class GunBarell : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;

    public void Fire()
    {
        var projectile = Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity);
        var rb = projectile.GetComponent<Rigidbody>();
        var rewindAbstract = projectile.GetComponent<RewindAbstract>();

        RewindManager.Instance.AddObjectForTracking(rewindAbstract,
            RewindManager.OutOfBoundsBehaviour.Disable); //Attaching it to the tracked in mid game

        rb.AddForce(
            (_spawnPoint.forward + new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1))) * 20,
            ForceMode.Impulse);
    }
}