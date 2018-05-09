using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Campaign.Interface
{
    public interface ICampaign
    {
        void ProcessLead(Stream lead);

        bool ValidLead(Stream lead);
    }
}
