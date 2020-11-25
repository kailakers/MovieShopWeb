using MovieShop.Core.Exceptions;

namespace MovieShop.MVC.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public ErrorResponseModel ErrorResponseModel { get; set; }
    }
}
