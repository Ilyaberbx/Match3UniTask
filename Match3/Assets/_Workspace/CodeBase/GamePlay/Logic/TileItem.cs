using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.GamePlay.Logic.Extensions;
using _Workspace.CodeBase.Service.EventBus;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic
{
    public class TileItem : MonoBehaviour
    {
        [SerializeField] private Vector2 _itemAppearPosition = new(0, 10);
        [SerializeField] private Renderer _renderer;

        private Color _color;

        private IEventBusService _eventBus;

        private int _y;

        private int _x;

        private bool _selected;

        private Vector3 _cachedScale;

        public Color GetColor()
            => _color;

        public int GetX()
            => _x;

        public int GetY()
            => _y;

        [Inject]
        public void Construct(IEventBusService eventBus)
            => _eventBus = eventBus;

        private void Awake()
            => _cachedScale = transform.localScale;

        private void OnMouseDown()
            => RaiseClick();

        public void UpdateContent(Color color)
        {
            _color = color;
            UpdateRenderer(color);
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
            await transform.DOScale(Vector3.zero, 0.5f)
                .ToUniTask();

        public async UniTask Appear()
        {
            transform.localPosition = Vector3.zero.AddZ(-1);
            await MoveFromPositionAsync(10);
        }

        public void Reset()
        {
            transform.localScale = _cachedScale;
            Deselect();
        }

        private void UpdateRenderer(Color color)
            => _renderer.material.color = color;

        private void RaiseClick()
        {
            _eventBus.RaiseEvent<IItemClickedHandler>
                (handler => handler.HandleTileItemClicked(this));
        }

        public async UniTask MoveFromPositionAsync(float y) =>
            await transform.DOMoveY(y, 0.5f).SetEase(Ease.OutExpo)
                .From()
                .ToUniTask();
    }
}