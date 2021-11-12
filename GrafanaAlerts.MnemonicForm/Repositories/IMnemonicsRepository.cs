using System.Collections.Generic;
using GrafanaAlerts.MnemonicForm.DTO;

namespace GrafanaAlerts.MnemonicForm.Repositories
{
    public interface IMnemonicsRepository
    {
        public List<AppDTO> LoadKe();
        public List<RoleDTO> LoadRoles();
        public List<string> LoadPriorities();
        public List<string> LoadInitiatorTypes();
    }
}