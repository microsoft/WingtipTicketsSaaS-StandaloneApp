using Events_Tenant.Common.Utilities;

namespace Events_Tenant.Common.Interfaces
{
    public interface IUtilities
    {
        byte[] ConvertIntKeyToBytesArray(int key);
        void ResetEventDates(string connString);
    }
}
