using _Workspace.CodeBase.GamePlay.Logic.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Vector3;

namespace _Workspace.CodeBase.GamePlay.Logic
{
    public class Tile : MonoBehaviour
    {
        public TileItem Item => _item;
        private TileItem _item;

        public async UniTask SetTileItem(TileItem value)
        {
            if (value == null)
                return;

            _item = value;
            Transform itemTransform = _item.transform;
            itemTransform.SetParent(transform, false);
            await itemTransform.DOLocalMove(zero.AddZ(-1), 1f)
                .ToUniTask();
        }
    }
}