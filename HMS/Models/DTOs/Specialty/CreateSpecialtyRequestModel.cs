using HMS.Models.Enums;

namespace HMS.Models.DTOs.Specialty
{
    public class CreateSpecialtyRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Position Position { get; set; }
    }
}
