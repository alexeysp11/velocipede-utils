using System.Threading.Tasks;
using Nethereum.Web3;

namespace VelocipedeUtils.Shared.Communication.NethereumAPI;

/// <summary>
/// Communication with ETH node (or imition of it)
/// </summary>
public class EthNodeAPIWeb3
{
    /// <summary>
    /// Address of the API node for retrieving ETH data 
    /// </summary>
    private string EthConnectionAddress { get; set; }

    /// <summary>
    /// Shows if connection with ETH node should be established or just imitated 
    /// </summary>
    private bool UseEthConnection { get; set; }
    
    /// <summary>
    /// Shows if the application is running in production or test mode. 
    /// If it is running in test mode, communication with ETH node will be imitated
    /// </summary>
    private string Environment { get; set; }

    /// <summary>
    /// Constructor of EthNodeAPIWeb3
    /// </summary>
    public EthNodeAPIWeb3() : this("", true, "production")
    {
    }
    
    /// <summary>
    /// Constructor of EthNodeAPIWeb3 
    /// </summary>
    public EthNodeAPIWeb3(string ethConnectionAddress, bool useEthConnection, string environment)
    {
        EthConnectionAddress = ethConnectionAddress;
        UseEthConnection = useEthConnection;
        Environment = environment;
    }

    /// <summary>
    /// Allows to get balance of a wallet 
    /// </summary>
    public Task<decimal> GetBalanceAsync(string address)
    {
        decimal ethAmount = 0m;
        if (string.IsNullOrEmpty(address)) 
            return Task.FromResult(ethAmount);
        Task task = Task.Run(async () => 
        {
            if (UseEthConnection && !string.IsNullOrEmpty(EthConnectionAddress))
            {
                // ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                var web3 = new Web3(EthConnectionAddress);
                var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
                ethAmount = Web3.Convert.FromWei(balance.Value);
            }
            else 
            {
                ethAmount = new Random().Next(0, 100);
            }
        });
        task.Wait();
        return Task.FromResult(ethAmount);
    }
}