using System;
using Nancy;

namespace WebApp
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World!";
        }
    }
}

