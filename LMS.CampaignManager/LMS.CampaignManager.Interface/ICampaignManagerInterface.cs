using System.Threading.Tasks;

namespace LMS.CampaignManager.Interface
{
    public interface ICampaignManager
    {
        string[] ProcessCampaigns(string message);
    }
}

