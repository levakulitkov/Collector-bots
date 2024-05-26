using System.Collections;
using UnityEngine;

public class BotCreationModule : MonoBehaviour
{
    [SerializeField] private CollectorBot _template;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _creationDuration = 2f;
    [SerializeField] private float _spawnForce = 5f;
    [SerializeField] private BotsBase _botsBase;

    private int _ñreationRequestCount;
    private bool _inProgress;

    public bool InProgress => _inProgress;

    public bool TryStartCreation(Resource resource)
    {
        if (resource.Amount < Constants.BotPrice)
            return false;

        _ñreationRequestCount++;

        if (!_inProgress)
            StartCoroutine(BotCreatingRoutine());

        return true;
    }

    private IEnumerator BotCreatingRoutine()
    {
        _inProgress = true;

        while (_ñreationRequestCount > 0)
        {
            yield return new WaitForSeconds(_creationDuration);

            CollectorBot bot = Instantiate(_template,
                _spawnPoint.position, Quaternion.identity);

            if (bot.TryGetComponent(out Rigidbody rb))
                rb.AddForce(_spawnPoint.forward * _spawnForce, ForceMode.Impulse);

            _botsBase.AddBot(bot);
            _ñreationRequestCount--;
        }

        _inProgress = false;
    }
}