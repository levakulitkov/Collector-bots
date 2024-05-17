using System.Collections;
using UnityEngine;

public class BotConstructionModule : MonoBehaviour
{
    [SerializeField] private CollectorBot _template;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private BotsBase _botsBase;
    [SerializeField] private float _creationDuration = 2f;
    [SerializeField] private float _spawnForce = 5f;

    private bool _inProgress;

    public bool InProgress => _inProgress;

    public bool TryCreateBot()
    {
        if (_inProgress)
            return false;

        StartCoroutine(BotCreatingRoutine());
        return true;
    }

    private IEnumerator BotCreatingRoutine()
    {
        _inProgress = true;

        yield return new WaitForSeconds(_creationDuration);

        CollectorBot bot = Instantiate(_template, _spawnPoint.position, Quaternion.identity);

        bot.GetComponent<Rigidbody>().AddForce(_spawnPoint.forward * _spawnForce, ForceMode.Impulse);

        _botsBase.AddBot(bot);

        _inProgress = false;
    }
}