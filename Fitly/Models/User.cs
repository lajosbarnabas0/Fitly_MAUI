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
        public string name { get; set; }
        public string email { get; set; }

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
        public float? height { get; set; }
        public double? weight { get; set; }
        public double? recommended_calories { get; set; }
        public double? lose_or_gain { get; set; }
        public double? goal_weight { get; set; }
        public bool? admin { get; set; }

    }
    public enum Gender
    {
        [EnumMember(Value = "female")]
        female,

        [EnumMember(Value = "male")]
        male,

        [EnumMember(Value = "other")]
        other
    }

}
