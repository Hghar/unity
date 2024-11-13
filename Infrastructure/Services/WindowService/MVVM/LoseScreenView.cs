using DG.Tweening;
using Infrastructure.Services.WindowService.ViewFactory;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class LoseScreenView : View<LoseScreenHierarchy, LoseScreenViewModel>
    {
        public LoseScreenView(LoseScreenHierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
        {
        }

        protected override void UpdateViewModel(LoseScreenViewModel viewModel)
        {
            BindClick(Hierarchy.CloseClick, viewModel.OnCloseClick);

        }

        public void OpenAnimation()
        {
            Hierarchy.HelpText.alpha = 0;
            Hierarchy.CloseClick.enabled = false;
            Hierarchy.VictoryImage.localScale = new Vector3(0, 1, 1);
            Hierarchy.ContinueText.enabled = false;
            Hierarchy.MainGroup.alpha = 0;
            Hierarchy.ItemsBackground.localScale = new Vector3(1,0,1);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(Hierarchy.MainGroup.DOFade(1, 1.3f).SetEase(Ease.InCirc));
            sequence.Append(Hierarchy.VictoryImage.DOScale(Vector3.one, 1));
            sequence.Append(Hierarchy.ItemsBackground.DOScale(Vector3.one, 0.3f));
            sequence.Append(Hierarchy.HelpText.DOFade(1, 1f));


            sequence.OnComplete(OnMainAnimationEnd);
            sequence.SetEase(Ease.Linear);

        }
        private void OnMainAnimationEnd()
        {
            CreateContinueTextAnimation();
            Hierarchy.CloseClick.enabled = true;
        }
        private Tween CreateContinueTextAnimation()
        {
            Hierarchy.ContinueText.enabled = true;

            Hierarchy.ContinueText.alpha = 0;
            Hierarchy.ContinueText.transform.localScale = new Vector3(0.8f,0.8f,0.8f);

            float duration = 0.6f;
            
            Sequence textSequence = DOTween.Sequence();
          
            Sequence positiveScale = DOTween.Sequence();
            positiveScale.Join(Hierarchy.ContinueText.DOFade(1, duration));
            positiveScale.Join(Hierarchy.ContinueText.transform.DOScale(1f, duration));

            Sequence negativeScale = DOTween.Sequence();
            negativeScale.Join(Hierarchy.ContinueText.DOFade(0, duration));
            negativeScale.Join(Hierarchy.ContinueText.transform.DOScale(0.8f, duration));

            textSequence.Append(positiveScale);
            textSequence.Append(negativeScale);

            textSequence.SetLoops(-1);
            return textSequence;
        }
    }
}