using ZapiCore;

namespace ShoppingApi.Dto.Request
{

    public class SearchCustomerRequest : PageCondition
    {
        /// <summary>
        ///  姓名
        /// </summary>
        public string Name { get; set; }

    }
}
