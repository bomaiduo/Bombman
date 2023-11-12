using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // ���ڲ���֧�ֵ�ƽ̨����ֵ������Ϊ null

    void Awake()
    {
        // ��ȡ��ǰƽ̨�� Ad Unit ID����浥Ԫ ID����
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // ��׼����չʾ���֮ǰ���øð�ť��
        _showAdButton.interactable = false;
    }

    // �����ݼ��ص���浥Ԫ�У�
    public void LoadAd()
    {
        // ��Ҫ�����ڳ�ʼ��֮���ټ������ݣ��ڴ�ʾ���У���ʼ������һ���ű��д�����
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // ��������سɹ�������ť��Ӽ���������������
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // ���øð�ť�ڵ���ʱ���� ShowAd() ������
            _showAdButton.onClick.AddListener(ShowAd);
            // �����û������ť��
            _showAdButton.interactable = true;
        }
    }

    // ʵ�ֵ��û������ťʱִ�еķ�����
    public void ShowAd()
    {
        // ���øð�ť��
        _showAdButton.interactable = false;
        // Ȼ��չʾ��棺
        Advertisement.Show(_adUnitId, this);
    }

    // ʵ�� Show Listener �� OnUnityAdsShowComplete �ص��������ж��û��Ƿ��ý�����
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // ���轱����
        }
    }

    // ʵ�� Load �� Show Listener ����ص���
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ʹ�ô�����ϸ��Ϣ��ȷ���Ƿ�Ҫ���Լ�����һ����档
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ʹ�ô�����ϸ��Ϣ��ȷ���Ƿ�Ҫ���Լ�����һ����档
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // ����ť�ļ�������
        _showAdButton.onClick.RemoveAllListeners();
    }
}