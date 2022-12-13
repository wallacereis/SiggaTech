using System.Threading.Tasks;

namespace SiggaTechAPP.ViewModels.Base
{
    abstract class BasePageViewModel : BaseViewModel
    {
        public virtual Task Initialize(params object[] args) => Task.CompletedTask;
    }
}
