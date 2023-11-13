using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.UI.Match
{
    public interface IMatchWindow
    {
        UniTask UpdateMatchStats(Dictionary<Color, int> colorsMap, int steps);
    }
}