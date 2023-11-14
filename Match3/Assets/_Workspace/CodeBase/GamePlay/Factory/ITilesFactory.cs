using _Workspace.CodeBase.GamePlay.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Factory
{
    public interface ITilesFactory
    {
        void Initialize(float width, float height);
        UniTask<Tile> CreateTile(int x, int y, Transform container);
    }
}