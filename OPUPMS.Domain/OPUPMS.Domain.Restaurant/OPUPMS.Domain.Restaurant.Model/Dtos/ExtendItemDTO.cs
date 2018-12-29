namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class ExtendItemDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string ItemValue { get; set; }
        public int CompanyId { get; set; }
        public int Sort { get; set; }
    }

    public class ExtendItemSearchDTO : BaseSearch
    {
        public int TypeId { get; set; }
        public int CompanyId { get; set; }
    }
}
