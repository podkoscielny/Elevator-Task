using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class InteractableDistance : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;

        private WaitForSeconds _waitForHidePanel;

        private const float PANEL_APPEARANCE_SPEED = 0.5f;
        private const float HIDE_PANEL_DELAY = 2f;

        private void OnEnable() => Interactable.OnGetCloserPanelShowed += ShowGetCloserPanel;

        private void OnDisable() => Interactable.OnGetCloserPanelShowed -= ShowGetCloserPanel;

        private void Start()
        {
            HidePanelOnStart();
            CacheWaitForSeconds();
        }

        private void ShowGetCloserPanel()
        {
            StopAllCoroutines();
            StartCoroutine(ShowGetCloserPanelCoroutine());
            StartCoroutine(HideGetCloserPanelCoroutine());
        }

        private IEnumerator ShowGetCloserPanelCoroutine()
        {
            float step = 0f;

            while (canvasGroup.alpha < 0.99f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, step);

                step += PANEL_APPEARANCE_SPEED * Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator HideGetCloserPanelCoroutine()
        {
            yield return _waitForHidePanel;

            float step = 0f;

            while (canvasGroup.alpha > 0.01f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, step);

                step += PANEL_APPEARANCE_SPEED * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 0;
        }

        private void HidePanelOnStart() => canvasGroup.alpha = 0;

        private void CacheWaitForSeconds() => _waitForHidePanel = new WaitForSeconds(HIDE_PANEL_DELAY);
    }
}
