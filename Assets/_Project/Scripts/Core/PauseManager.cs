using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private readonly HashSet<Object> pauseRequesters = new HashSet<Object>();

    public bool IsPaused => pauseRequesters.Count > 0;
    public int PauseRequestCount => pauseRequesters.Count;

    public void RequestPause(Object requester)
    {
        if (requester == null)
        {
            Debug.LogWarning("[PauseManager] requester가 null이라 일시정지 요청이 무시되었습니다.", this);
            return;
        }

        pauseRequesters.Add(requester);
        UpdateTimeScale();
    }

    public void ReleasePause(Object requester)
    {
        if (requester == null)
        {
            return;
        }

        pauseRequesters.Remove(requester);
        UpdateTimeScale();
    }

    public void ClearAllPauseRequests()
    {
        pauseRequesters.Clear();
        UpdateTimeScale();
    }

    private void OnDestroy()
    {
        // 씬 전환/종료 시 게임이 멈춘 상태로 남지 않도록 안전하게 복구합니다.
        Time.timeScale = 1f;
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}
