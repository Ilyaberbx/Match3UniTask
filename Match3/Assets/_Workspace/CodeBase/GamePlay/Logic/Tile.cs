using System;
using System.Threading.Tasks;
using _Workspace.CodeBase.GamePlay.Logic.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Vector3;

namespace _Workspace.CodeBase.GamePlay.Logic
{
    public class Tile : MonoBehaviour
    {
        public TileItem Item { get; private set; }
        public int _x;
        public int _y;

        public void SetItem(TileItem value)
        {
            Item = value ? value : throw new NullReferenceException();
            Item.SetPosition(_x,_y);
            ApplyItemToTile();
        }

        public void SwapItems(Tile tile)
        {
            TileItem swapItem = tile.Item;
            tile.SetItem(Item);
            SetItem(swapItem);
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        private void ApplyItemToTile()
        {
            Transform itemTransform = Item.transform;
            itemTransform.SetParent(transform, false);
            itemTransform.localPosition = zero.AddZ(-1);
        }
    }
}