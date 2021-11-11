using System.Collections.Generic;

namespace GrafanaAlerts.MnemonicForm.Repositories
{
    public interface IMnemonicsRepository
    {
        public List<string> LoadKe();
        public List<string> LoadRoles();
        public List<string> LoadPriorities();
        public List<string> LoadInitiatorTypes();
    }
}