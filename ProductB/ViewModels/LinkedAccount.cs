using System.ComponentModel;

namespace ProductB.ViewModels
{
    public class LinkedAccount
    {
        public int ID { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }
    }
}