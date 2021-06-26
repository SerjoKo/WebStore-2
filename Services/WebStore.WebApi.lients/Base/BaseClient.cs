﻿using System.Net.Http;

namespace WebStore.WebApi.lients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient Http { get; set; }

        protected string Address { get; }

        protected BaseClient(HttpClient Client, string Address)
        {
            Http = Client;
            this.Address = Address;
        }
    }
}
