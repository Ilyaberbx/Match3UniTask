using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.Service.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Workspace.CodeBase.UI.GameOver
{
    public class GameOverWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _stepsCount;
        [SerializeField] private Button _restartButton;
        private IEventBusService _eventBus;

        [Inject]
        public void Construct(IEventBusService eventBus) 
            => _eventBus = eventBus;

        private void Start() 
            => _restartButton.onClick.AddListener(Restart);

        private void OnDestroy() 
            => _restartButton.onClick.RemoveListener(Restart);

        private void Restart() 
            => _eventBus.RaiseEvent
                <IGameRestartHandler>(handler => handler.HandleGameRestart());

        public void UpdateContent(int steps) 
            => _stepsCount.text = steps.ToString();
    }
}