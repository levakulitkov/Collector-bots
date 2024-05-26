using System.Collections;
using UnityEngine;

public class BotCreationModule : MonoBehaviour
{
    [SerializeField] private CollectorBot _template;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _creationDuration = 2f;
    [SerializeField] private float _spawnForce = 5f;
    [SerializeField] private BotsBase _botsBase;

    private int _�reationRequestCount;
    private bool _inProgress;

    public bool InProgress => _inProgress;

    public bool TryStartCreation(Resource resource)
    {
        if (resource.Amount < Constants.BotPrice)
            return false;

        _�reationRequestCount++;

        if (!_inProgress)
            StartCoroutine(BotCreatingRoutine());

        return true;
    }

    private IEnumerator BotCreatingRoutine()
    {
        _inProgress = true;

        while (_�reationRequestCount > 0)
        {
            yield return new WaitForSeconds(_creationDuration);

            CollectorBot bot = Instantiate(_template,
                _spawnPoint.position, Quaternion.identity);

            if (bot.TryGetComponent(out Rigidbody rb))
                rb.AddForce(_spawnPoint.forward * _spawnForce, ForceMode.Impulse);

            _botsBase.AddBot(bot);
            _�reationRequestCount--;
        }

        _inProgress = false;
    }
}