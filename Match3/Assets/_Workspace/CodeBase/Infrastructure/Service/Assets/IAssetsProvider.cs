﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Workspace.CodeBase.Infrastructure.Service.Assets
{
    public interface IAssetsProvider
    {
        UniTask InitializeAsync();
        UniTask<TAsset> Load<TAsset>(string key) where TAsset : class;
        UniTask<TAsset> Load<TAsset>(AssetReference assetReference) where TAsset : class;
        UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
        UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null);
        UniTask<TAsset[]> LoadAll<TAsset>(List<string> keys) where TAsset : class;
        UniTask WarmUpAssetsByLabel(string label);
        UniTask ReleaseAssetsByLabel(string label);
        void CleanUp();
    }
}