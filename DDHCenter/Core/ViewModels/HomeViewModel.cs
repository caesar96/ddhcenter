using DDHCenter.Core.Interfaces;

namespace DDHCenter.Core.ViewModels
{
    public class HomeViewModel : ViewModel, IViewModel
    {
        public string Name
        {
            get
            {
                return "Home Page";
            }
        }
    }
}
