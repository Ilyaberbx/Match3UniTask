using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.Service.EventBus;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic
{
    public class TileItem : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private Color _color;

        private IEventBusService _eventBus;

        private int _y;

        private int _x;

        private bool _selected;

        public Color GetColor()
            => _color;

        public int GetX()
            => _x;

        public int GetY()
            => _y;

        [Inject]
        public void Construct(IEventBusService eventBus)
            => _eventBus = eventBus;

        private void OnMouseDown()
        {
            Debug.Log(Selected());
            RaiseClick();
        }

        public async UniTask UpdateContent(Color color)
        {
            _color = color;
            Reset();
            UpdateRenderer(color);
            await Appear();
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void Select()
            => _selected = true;

        public void Deselect()
            => _selected = false;

        public bool Selected()
            => _selected;

        public async UniTask Dissolve() =>
            await transform.DOScale(Vector3.zero, 2f)
                .ToUniTask();

        private async UniTask Appear() =>
            await transform.DOMoveY(10, 1f).SetEase(Ease.OutExpo)
                .From()
                .ToUniTask();

        private void Reset()
        {
            transform.localScale = Vector3.one;
            Deselect();
        }

        private void UpdateRenderer(Color color)
            => _renderer.material.color = color;

        private void RaiseClick()
        {
            _eventBus.RaiseEvent<IItemClickedHandler>
                (handler => handler.HandleTileItemClicked(this));
        }
    }
}