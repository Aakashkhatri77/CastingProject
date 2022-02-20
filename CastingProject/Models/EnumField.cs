using System.Data;

namespace CastingProject.Models
{
    public class EnumField
    {
        public enum Gender
        {
            Male,
            Female,
            Other
        }

        public enum SkinColor
        {
            Fair,
            Medium,
            Olive,
            Dark
        }

        public enum EyeColor
        {
            Brown,
            Black,
            Blue,
            Green
        }

        public enum HairColor
        {
            Brown,
            Black,
            Blonde,
            Other
        }

        public enum SkinType
        {
            Oily,
            Dry,
            Combination
        }

        public enum HairLength
        {
            Short,
            Medium,
            Long
        }

        public enum HairType
        {
            Curly,
            Straight,
            Frizzy
        }

        public enum MaritalStatus
        {
            Married,
            Unmarried
        }
    }
}
