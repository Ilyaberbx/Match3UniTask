using System.Collections.Generic;
using _Workspace.CodeBase.UI.Service.Factory;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Workspace.CodeBase.UI.Match
{
    public class MatchWindow : BaseWindow, IMatchWindow
    {
        [SerializeField] private TextMeshProUGUI _stepsCountText;
        [SerializeField] private VerticalLayoutGroup _vlg;
        private IMatchViewFactory _viewFactory;


        [Inject]
        public void Construct(IMatchViewFactory viewFactory)
            => _viewFactory = viewFactory;

        public async UniTask UpdateMatchStats(Dictionary<Color, int> colorsMap, int steps)
        {
            _stepsCountText.text = steps.ToString();
            
            ClearUp();
            foreach (KeyValuePair<Color, int> colorPair in colorsMap)
            {
                MatchView view = await _viewFactory.CreateMatchView(_vlg);
                view.UpdateView(colorPair.Key, colorPair.Value);
            }
        }

        private void ClearUp()
        {
            foreach (MatchView child in _vlg.GetComponentsInChildren<MatchView>())
                Destroy(child.gameObject);
        }
    }
}