using MijiaScanner.Models;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MijiaScanner.Droid.TemplateSelectors
{
    public class DeviceTemplateSelector : MvxTemplateSelector<MijiaDevice>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return Resource.Layout.itemtemplate_device;
        }

        protected override int SelectItemViewType(MijiaDevice forItemObject)
        {
            return 1;
        }
    }
}