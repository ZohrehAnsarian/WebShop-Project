using Model.Base;

namespace Model.ViewModels
{
    public class VmCountry : BaseViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int PhoneCode { get; set; }
        public string FlagUrl { get; set; }
    }
}
