using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Match
{
    public interface IMatchModel 
    {
        event Func<Dictionary<UnityEngine.Color, int>, int, UniTask> OnUpdated;
    }
}