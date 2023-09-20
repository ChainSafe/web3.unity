using UnityEngine;
using UnityEngine.UI;

public class QrCodePreviewer : MonoBehaviour
{
    [SerializeField] private Image _qrCodeImage;
    
    [SerializeField] private WalletConnectUnity _walletConnect;

    private void Start()
    {
        _walletConnect.OnConnected += data =>
        {
            Debug.LogError(data.Uri);
        };
        
        _walletConnect.OnSessionApproved += session =>
        {
            Debug.LogError(session.Peer.PublicKey);
            
            Debug.LogError(session.Self.PublicKey);
        };
    }
}
