using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Workspace.CodeBase.UI.Match
{
    public class MatchView : MonoBehaviour
    {
        [SerializeField] private Image _colorIcon;
        [SerializeField] private TextMeshProUGUI _collectedCountText;
        
        public void UpdateView(Color color, int collectedCount)
        {
            _colorIcon.color = color;
            _collectedCountText.text = collectedCount.ToString();
        }
    }
}