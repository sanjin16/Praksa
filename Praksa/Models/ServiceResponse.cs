﻿namespace Praksa.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; } = true;

        public string message { get; set; } = null;
    }
}
