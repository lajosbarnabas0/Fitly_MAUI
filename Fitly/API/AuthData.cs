﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitly.Models;

namespace Fitly.API
{
    public static class AuthData
    {
        public static async Task<LoginUserResponse> LoginUser(string url, object requestData)
        {
            return await HTTPRequest<LoginUserResponse>.Post(url, requestData);
        }

        public static async Task<RegisterUserResponse> RegisterUser(string url, object requestData)
        {
            return await HTTPRequest<RegisterUserResponse>.Post(url, requestData);
        }
    }
}
