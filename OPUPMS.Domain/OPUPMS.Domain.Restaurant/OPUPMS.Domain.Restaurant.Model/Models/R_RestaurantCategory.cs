using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_RestaurantCategory
    {
        public int Id { get; set; }
        public int R_Restaurant_Id { get; set; }
        public int R_Category_Id { get; set; }
    }
}
