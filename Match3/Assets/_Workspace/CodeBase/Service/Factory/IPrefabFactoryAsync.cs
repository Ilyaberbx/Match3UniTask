﻿using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.Service.Factory
{
    public interface IPrefabFactoryAsync
    {
        UniTask<T> Create<T>(string address) where T : Component;
        UniTask<T> Create<T>(string address, Vector3 at, Transform container) where T : Component;
        Task<T> Create<T>(string address, Transform container) where T : Component;
    }
}