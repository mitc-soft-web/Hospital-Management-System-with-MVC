using System.ComponentModel;

namespace HMS.Models.Enums
{
    public enum Position
    {
        [Description("Senior Doctor")]
        SeniorDoctor = 1,

        [Description("Resident Doctor")]
        ResidentDoctor,

        [Description("Senior Resident Doctor")]
        SeniorResidentDoctor,
        Intern,

    }
}
