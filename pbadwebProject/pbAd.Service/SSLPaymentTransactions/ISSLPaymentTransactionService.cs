using pbAd.Data.Models;
using System.Threading.Tasks;

namespace pbAd.Service.SSLPaymentTransactions
{
    public interface ISSLPaymentTransactionService
    {
        Task<SSLPaymentTransaction> GetDetailsById(int id);
        Task<bool> Add(SSLPaymentTransaction sslPaymentTransaction);
        Task<bool> Update(SSLPaymentTransaction sslPaymentTransaction);
        Task<bool> Remove(SSLPaymentTransaction sslPaymentTransaction);
    }
}
