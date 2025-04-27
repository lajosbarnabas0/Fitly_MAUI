using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class User
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        private string _gender;

        public string gender
        {
            get => _gender;
            set
            {
                if (Enum.TryParse(value, true, out Gender parsedGender))
                    GenderEnum = parsedGender;
                _gender = value;
            }
        }

        [JsonIgnore]
        public Gender GenderEnum { get; private set; }
        public string birthday { get; set; }
        public TimeSpan? email_verified_at { get; set; }
        public double? height { get; set; }
        public double? weight { get; set; }
        public double? recommended_calories { get; set; }

        private string _loseOrGain;

        public string lose_or_gain
        {
            get => _loseOrGain;
            set
            {
                if (Enum.TryParse(value, true, out LoseOrGain parsedValue))
                    LoseOrGainEnum = parsedValue;
                _loseOrGain = value;
            }
        }

        [JsonIgnore]
        public LoseOrGain LoseOrGainEnum
        {
            get => lose_or_gain == "lose" ? LoseOrGain.Fogyás : LoseOrGain.Hízás;
            set => lose_or_gain = value == LoseOrGain.Fogyás ? "lose" : "gain";
        }
        public double? goal_weight { get; set; }
        public bool? admin { get; set; }

        public string? avatar {  get; set; }

    }
    public enum Gender
    {
        [EnumMember(Value = "female")]
        female,

        [EnumMember(Value = "male")]
        male,
    }

    public enum LoseOrGain
    {
        [EnumMember(Value = "lose")]
        Fogyás,

        [EnumMember(Value = "gain")]
        Hízás
    }

}
